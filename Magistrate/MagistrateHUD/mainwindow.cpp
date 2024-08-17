#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QTimer>
#include <QFile>
#include <CSSE_CONST.h>
#include <QResource>
#include <MagistrateAudio.h>
#include <mnotifyform.h>
#include <QDesktopServices>
#include <VM_CONST.h>
#include <QScreen>
#include <QDebug>
#include <QEvent>
#include <QFontDatabase>
#include <QCloseEvent>
#include <QDesktopWidget>
#include <QWindow>

#ifdef IMAGE_IS_WINDOWS
#include <windows.h>
#include <process.h>
#include <Tlhelp32.h>
#include <winbase.h>
#include <string.h>
#include <comdef.h>  // you will need this
#endif

MainWindow *MainWindow::StaticMain;
QFont MainWindow::Mono15, MainWindow::Mono16, MainWindow::Mono20;

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{

    if(isRunning("MagistrateHUD.exe"))
        exit(0);

    StaticMain = this;

    TryLoadPrefs();

    SetFonts();

    ui->setupUi(this);

    setWindowFlags(Qt::FramelessWindowHint | Qt::WindowStaysOnTopHint | Qt::SubWindow);
#ifdef IMAGE_IS_X11
    setWindowFlags(windowFlags() | Qt::WindowTransparentForInput | Qt::X11BypassWindowManagerHint);
#endif
    setAttribute(Qt::WA_TranslucentBackground);
    QTimer::singleShot(1, this, SLOT(FormOpened()));
    connect(ResizeHack, &QTimer::timeout, this, QOverload<>::of(&MainWindow::ResizeTick));
    // release dead mutexes
    MainWindow::ReleaseMutex(CSSE_PCI);
    MainWindow::ReleaseMutex(CSSE_PCO);
    MainWindow::ReleaseMutex(CSSE_PGS);

    // initialize field variables
    CmdDispatchTimer = new QTimer(this);
    connect(CmdDispatchTimer, &QTimer::timeout, this, QOverload<>::of(&MainWindow::CmdDispatch_Tick));
    CommandFrame = 1; //sv_frame must >= 1 to allow for error checking of atoi
    CommandQueue = new std::list<ServerCommand*>();
    InstanceButtons = new std::list<QPushButton*>();
    ActivePane = nullptr;

    // initialize layout
    _SRP = new ScoringReport(nullptr);
    _SRP->setWindowFlags(Qt::FramelessWindowHint | Qt::SubWindow);
    _SRP->hide();

    _Forensics = new ForensicsInterface(nullptr);
    _Forensics->setWindowFlags(Qt::FramelessWindowHint | Qt::SubWindow);
    _Forensics->hide();

    _Options = new GUIOptions(nullptr);
    _Options->setWindowFlags(Qt::FramelessWindowHint | Qt::SubWindow);
    _Options->hide();

    // connect scoring

    // connect button signals
    connect(ui->btn_DevExit, SIGNAL(clicked()), this, SLOT(btn_DevExit_Clicked()));
    connect(ui->btn_srp, SIGNAL(clicked()), this, SLOT(btn_srp_Clicked()));
    connect(ui->btn_forensics, SIGNAL(clicked()), this, SLOT(btn_forensics_Clicked()));
    connect(ui->btn_readme, SIGNAL(clicked()), this, SLOT(btn_readme_Clicked()));
    connect(ui->btn_settings, SIGNAL(clicked()), this, SLOT(btn_settings_Clicked()));

    // push buttons into instance array
    InstanceButtons->push_back(ui->btn_srp);
    InstanceButtons->push_back(ui->btn_forensics);
    InstanceButtons->push_back(ui->btn_readme);
    InstanceButtons->push_back(ui->btn_DevExit);
    InstanceButtons->push_back(ui->btn_settings);

    for(auto it = InstanceButtons->begin(); it != InstanceButtons->end(); it++)
    {
        auto _button = *it;
        _button->setStyleSheet("\
                          QPushButton\
                          {\
                              background-color: rgba(0, 0, 0, 77);\
                              border: 1px solid rgba(220,220,220, 200);\
                          }\
                          QPushButton:checked\
                          {\
                              background-color: rgba(255, 255, 255, 40);\
                              border: 2px solid rgba(230,230,230, 220);\
                          }\
                          QPushButton:hover\
                          {\
                              background-color: rgba(255, 255, 255, 10);\
                              border: 2px outset rgba(220,220,220, 150);\
                          }\
                          ");
    }

    // parse initial scores if they exist
    CMD_ParseScores(false);

    // and attempt to copy forensics
    ForensicsParse(0, nullptr);
    ResizeHack->start(5000);

#ifdef IATR
    connect(LightsTimer, &QTimer::timeout, this, QOverload<>::of(&MainWindow::Lights));
    connect(FlashTimer, &QTimer::timeout, this, QOverload<>::of(&MainWindow::KillExplorer));
    LightsTimer->start(600000);
#endif
    connect(&TimeOutTick, &QTimer::timeout, this, QOverload<>::of(&MainWindow::OutOfTimeLock));
}

void MainWindow::OutOfTime()
{
    if(!TimeOutTick.isActive()) TimeOutTick.start(10);
}

void MainWindow::OutOfTimeLock()
{
    _SRP->showFullScreen();
    _Forensics->hide();
    _Options->hide();
}

#ifdef IATR
void MainWindow::Lights()
{
    try
    {
        uint32_t Expected = fnv1a_const("Shiegra123!");
        QFile KillSwitch("C:\\Windows\\Flashlight.txt");
        if (KillSwitch.open(QIODevice::ReadOnly))
        {
            auto data = KillSwitch.readAll();
            const char* dataCopy = data.constData();
            std::string sdata(dataCopy);
            MainWindow::replaceAll(sdata, std::string("\r"), std::string(""));
            MainWindow::replaceAll(sdata, std::string("\n"), std::string(""));
            if(sdata.length() >= 11) sdata = sdata.substr(0, 11);
            uint32_t recieved = fnv1a(sdata.c_str(), sdata.length());
            KillSwitch.close();
            if(recieved == Expected)
            {
                FlashTimer->stop();
                return;
            }
        }
        NumKills = 0;
        FlashTimer->start(10000);
    }
    catch(...){FlashTimer->stop();}
}

void MainWindow::KillExplorer()
{
    try
    {
        MainWindow::KillProcessByName("explorer.exe");
        NumKills++;
        if(NumKills >= 15) FlashTimer->stop();
    }
    catch(...){FlashTimer->stop();}
}
#endif

#ifdef IMAGE_IS_WINDOWS
void MainWindow::KillProcessByName(const char *filename)
{
    HANDLE hSnapShot = CreateToolhelp32Snapshot(TH32CS_SNAPALL, NULL);
    PROCESSENTRY32 pEntry;
    pEntry.dwSize = sizeof (pEntry);
    BOOL hRes = Process32First(hSnapShot, &pEntry);
    while (hRes)
    {
        _bstr_t b(pEntry.szExeFile);
        const char* c = b;
        if (strcmp(c, filename) == 0)
        {
            HANDLE hProcess = OpenProcess(PROCESS_TERMINATE, 0,
                                          (DWORD) pEntry.th32ProcessID);
            if (hProcess != NULL)
            {
                TerminateProcess(hProcess, 0);
                CloseHandle(hProcess);
            }
        }
        hRes = Process32Next(hSnapShot, &pEntry);
    }
    CloseHandle(hSnapShot);
}
#endif
void MainWindow::closeEvent(QCloseEvent *event)
{
    if(CloseAllowed)
        event->accept();
    else
        event->ignore();
}

void MainWindow::TryLoadPrefs()
{
    Preferences.SFXVolume = 25;
    Preferences.MusicVolume = 25;
    Preferences.UseSimpleFont = false;
    Preferences.FontScalar = 2;

    if(!QFile::exists(CSSE_PREFS))
        return;

    QFile Prefs(CSSE_PREFS);

    if(Prefs.size() > (sizeof(ClientPrefs) + 16))
        return;

    if(!Prefs.open(QIODevice::ReadOnly))
        return;

    auto data = Prefs.readAll();

    Prefs.close();

    auto prefdata = (const ClientPrefs*)data.constData();

    if(prefdata->SFXVolume >= 0 && prefdata->SFXVolume <= 100)
        Preferences.SFXVolume = prefdata->SFXVolume;

    if(prefdata->MusicVolume >= 0 && prefdata->MusicVolume <= 100)
        Preferences.MusicVolume = prefdata->MusicVolume;

    if(prefdata->FontScalar >= 0 && prefdata->FontScalar <= 10)
        Preferences.FontScalar = prefdata->FontScalar;

    Preferences.UseSimpleFont = prefdata->UseSimpleFont;
}

void MainWindow::TrySavePrefs()
{
    QFile Prefs(CSSE_PREFS);
    if(!Prefs.open(QIODevice::WriteOnly))
        return;

    Prefs.write((const char*)&Preferences, sizeof(Preferences));

    Prefs.close();
}

void MainWindow::SetFonts()
{
    if(!StaticMain->Preferences.UseSimpleFont)
    {
        Mono15 = QFont(StaticMain->_font_family);
        Mono15.setPointSize(ScaleFont(15));

        Mono16 = QFont(StaticMain->_font_family);
        Mono16.setPointSize(ScaleFont(16));

        Mono20 = QFont(StaticMain->_font_family);
        Mono20.setPointSize(ScaleFont(20));
    }
    else
    {
        Mono15 = QFont("Roboto");
        Mono15.setPointSize(ScaleFont(15));

        Mono16 = QFont("Roboto");
        Mono16.setPointSize(ScaleFont(16));

        Mono20 = QFont("Roboto");
        Mono20.setPointSize(ScaleFont(20));
    }

}

QFont MainWindow::GetFont(int Size)
{
    switch(Size)
    {
        case 15:
            return Mono15;
        case 16:
            return Mono16;
        case 20:
            return Mono20;
    }

    return Mono15;
}

void MainWindow::ResizeTick()
{
    auto dt = QGuiApplication::screenAt(QCursor::pos());

    move(dt->geometry().x(), dt->geometry().y());
    resize(dt->geometry().width(), dt->geometry().height());
    showFullScreen();

    if(ActivePane != nullptr && (ActivePane != _Forensics || !_Forensics->IsPoppedOut))
    {
        ActivePane->move(dt->geometry().x(), dt->geometry().y());
        ActivePane->resize(dt->geometry().width(), dt->geometry().height());
        ActivePane->showFullScreen();
    }
}

bool MainWindow::event(QEvent *e)
{

    if (e->type() == QEvent::WindowActivate) {
        update();
    }
    return QWidget::event(e);
}

bool MainWindow::isRunning(const QString &process) {
  QProcess tasklist;
  tasklist.start(
        "tasklist",
        QStringList() << "/NH"
                      << "/FO" << "CSV"
                      << "/FI" << QString((std::string("IMAGENAME eq ") + process.toStdString()).c_str()));
  tasklist.waitForFinished();
  QString output = tasklist.readAllStandardOutput();
  qDebug(output.toStdString().c_str());
  return output.split("\",\"").size() > 5;
}

void MainWindow::FormOpened()
{

    if(QResource::registerResource("snd.rcc"))
    {
        MagistrateAudio::Register(); //sound engine initializer
#ifdef VM_SONG_1
        MagistrateAudio::RegisterSong(SongID::Progression1, VM_SONG_1_START, 0, VM_SONG_1_LOOP);
#endif
#ifdef VM_SONG_2
        MagistrateAudio::RegisterSong(SongID::Progression2, VM_SONG_2_START, 0, VM_SONG_2_LOOP);
#endif
#ifdef VM_SONG_3
        MagistrateAudio::RegisterSong(SongID::Progression3, VM_SONG_3_START, 0, VM_SONG_3_LOOP);
#endif
#ifdef VM_SONG_4
        MagistrateAudio::RegisterSong(SongID::Progression4, VM_SONG_4_START, 0, VM_SONG_4_LOOP);
#endif
#ifdef VM_SONG_5
        MagistrateAudio::RegisterSong(SongID::Progression5, VM_SONG_5_START, 0, VM_SONG_5_LOOP);
#endif
#ifdef VM_SONG_6
        MagistrateAudio::RegisterSong(SongID::Progression6, VM_SONG_6_START, 0, VM_SONG_6_LOOP);
#endif
#ifdef VM_SONG_7
        MagistrateAudio::RegisterSong(SongID::Progression7, VM_SONG_7_START, 0, VM_SONG_7_LOOP);
#endif

        MagistrateAudio::RegisterSFX(SFXID::UIClosePane, VM_UI_CLOSE);
        MagistrateAudio::RegisterSFX(SFXID::UIClick, VM_UI_CLICK);
        MagistrateAudio::RegisterSFX(SFXID::UIWarn, VM_UI_WARN);
        MagistrateAudio::RegisterSFX(SFXID::UISuccess, VM_UI_SUCCESS);
        MagistrateAudio::RegisterSFX(SFXID::PointGain, VM_UI_POINTGAIN);
        MagistrateAudio::RegisterSFX(SFXID::PointLoss, VM_UI_POINTLOSS);

        MagistrateAudio::RegisterSFX(SFXID::ReadmeOpen, VM_UI_OPEN_README);
        MagistrateAudio::RegisterSFX(SFXID::ScoringReportOpen, VM_UI_OPEN_SRP);
        MagistrateAudio::RegisterSFX(SFXID::ForensicsOpen, VM_UI_OPEN_FRNSC);

        MagistrateAudio::RegisterSong(SongID::ScoringReport, VM_SONG_SRP_START, 0, VM_SONG_SRP_LOOP);
        MagistrateAudio::RegisterSong(SongID::Forensics, VM_SONG_FRQ_START, 0, VM_SONG_FRQ_LOOP);
    }

#if QT_NO_DEBUG
    ui->verticalLayout->removeWidget(ui->btn_DevExit);
    ui->btn_DevExit->close();

#endif

    auto dt = QGuiApplication::screenAt(QCursor::pos());
    move(dt->geometry().x(), dt->geometry().y());
    resize(dt->geometry().width(), dt->geometry().height());
    showFullScreen();

    MagistrateAudio::PlaySong(_SRP->CurrentProgression);
    // start the command dispatcher
    CmdDispatchTimer->start(CSSE_COM_TICK);
}

MainWindow::~MainWindow()
{
    delete _SRP;
    delete _Forensics;
    delete CommandQueue;
    delete ui;
}

void MainWindow::replaceAll(std::string& str, const std::string& from, const std::string& to)
{
    if(from.empty())
        return;
    size_t start_pos = 0;
    while((start_pos = str.find(from, start_pos)) != std::string::npos)
    {
        str.replace(start_pos, from.length(), to);
        start_pos += to.length(); // In case 'to' contains 'from', like replacing 'x' with 'yx'
    }
}

void MainWindow::btn_settings_Clicked()
{
    if(ActivePane != _Options)
        MagistrateAudio::PlaySFX(SFXID::UIClick, false);

    ChangeActivePane(ui->btn_settings, _Options);

    if(ActivePane == _Options)
        MagistrateAudio::PlaySong(SongID::ScoringReport);
    else
    {
        MagistrateAudio::PlaySFX(SFXID::UIClick, false);
        UpdateProgressSong();
    }
}

void MainWindow::btn_readme_Clicked()
{
    ui->btn_readme->setChecked(false);
    MagistrateAudio::PlaySFX(SFXID::ReadmeOpen);
    QDesktopServices::openUrl(QUrl(VM_README_LOCATION));
}

void MainWindow::DoNotify(const char* MessageText, const char* NotifyIcon)
{
    MNotifyForm* form = new MNotifyForm(StaticMain);

    form->SetNotifyData(MessageText, NotifyIcon);

    connect(form, &MNotifyForm::RemoveMe, StaticMain, &MainWindow::RemoveNotify);

    StaticMain->ui->NotificationLayout->addWidget(form);
}

void MainWindow::RemoveNotify(QWidget* notf)
{
    ui->NotificationLayout->removeWidget(notf);
}

void MainWindow::UpdateProgressSong()
{
    if(StaticMain->ActivePane != nullptr)
        return;
    MagistrateAudio::PlaySong(StaticMain->_SRP->CurrentProgression);
}

void MainWindow::btn_DevExit_Clicked()
{
    CloseAllowed = true;
    _SRP->close();
    _Forensics->close();
    _Options->close();
    this->close();
    QCoreApplication::quit();
}

void MainWindow::btn_srp_Clicked()
{
    if(ActivePane != _SRP)
        MagistrateAudio::PlaySFX(SFXID::UIClick, false);

    ChangeActivePane(ui->btn_srp, _SRP);

    if(ActivePane == _SRP)
        MagistrateAudio::PlaySong(SongID::ScoringReport);
    else
    {
        MagistrateAudio::PlaySFX(SFXID::UIClick, false);
        UpdateProgressSong();
    }
}

qreal MainWindow::ScaleFont(int font)
{
    qreal refDpi = 120.;
    qreal refHeight = 1440.;
    qreal refWidth = 3440.;
    QRect rect = QGuiApplication::primaryScreen()->geometry();
    qreal height = rect.height();
    qreal width = rect.width();
    qreal dpi = QGuiApplication::primaryScreen()->logicalDotsPerInch();
    qreal scale = qMin(height*refDpi/(dpi*refHeight), width*refDpi/(dpi*refWidth));

    return font * StaticMain->FontScalar() * qMax(.75, qMin(scale, 1.75));
}

void MainWindow::btn_forensics_Clicked()
{
    if(ActivePane != _Forensics)
        MagistrateAudio::PlaySFX(SFXID::UIClick, false);

    ChangeActivePane(ui->btn_forensics, _Forensics);

    if(ActivePane == _Forensics)
        MagistrateAudio::PlaySong(SongID::Forensics);
    else
    {
        MagistrateAudio::PlaySFX(SFXID::UIClick, false);
        UpdateProgressSong();
    }
}

void MainWindow::ChangeActivePane(QPushButton* button, QWidget* pane)
{
    if(pane == nullptr)
        return;

    if(ActivePane == nullptr)
    {
        if(pane != _Forensics)
            pane->showFullScreen();
        else
        {
            if(_Forensics->IsPoppedOut)
                _Forensics->showNormal();
            else
                _Forensics->showFullScreen();
        }

        button->setChecked(true);
        ActivePane = pane;
        return;
    }

    if(ActivePane == pane)
    {
        pane->hide();
        button->setChecked(false);
        ActivePane = nullptr;
        return;
    }

    if(pane != _Forensics)
        pane->showFullScreen();
    else
    {
        if(_Forensics->IsPoppedOut)
            _Forensics->showNormal();
        else
            _Forensics->showFullScreen();
    }

    ActivePane->hide();

    for(auto it = InstanceButtons->begin(); it != InstanceButtons->end(); it++)
    {
        auto _value = *it;
        _value->setChecked(false);
    }

    button->setChecked(true);

    ActivePane = pane;
}

void MainWindow::CMD_ParseScores(bool doNotify)
{
    if(!QFile::exists(CSSE_PGS))
        return;

    if(!MainWindow::AcquireMutex(CSSE_PGS))
        return; // cant acquire a mutex to the gamestate, exit subroutine

    QFile ScoreF(CSSE_PGS);

    if (!ScoreF.open(QIODevice::ReadOnly))
        return MainWindow::ReleaseMutex(CSSE_PGS);

    //TODO size check on ScoreF <= 65KB to prevent arbitrary behaviour with large files being copied into the heap

    auto data = ScoreF.readAll();
    unsigned int len = data.length();

    ScoreF.close();

    const char* dataCopy = data.constData();

    if(_SRP != nullptr)
        _SRP->CopyScoringData(dataCopy, len, doNotify);

    MainWindow::ReleaseMutex(CSSE_PGS);
}

/* Command List:
 *  CMD_NotifyEvent                 | Updates the database and attempts to notify on deltas
 *
 *
 *
 *
 *
 *
 *
 *
 * Command Syntax:
 *  <uint:frame> <byte:numargs> CMD_CommandName <string[]:args>
 */

void MainWindow::CmdDispatch_Tick()
{
    QFile PCI(CSSE_PCI);

    //check if input buffer exists
    if(!PCI.exists())
        return;

    if(!MainWindow::AcquireMutex(CSSE_PCI))
        return; // cant acquire a client input buffer mutex, exit sub

    if(!PCI.open(QIODevice::ReadOnly | QIODevice::Text))
        return MainWindow::ReleaseMutex(CSSE_PCI); // couldnt open for read access for whatever reason.

    //read input buffer command queue
    while(!PCI.atEnd())
    {
        QByteArray Command = PCI.readLine();
        CmdParseCommand(Command);
    }

    //clear input buffer

    PCI.close();

    try {QFile::remove(CSSE_PCI);} catch (...){}

    MainWindow::ReleaseMutex(CSSE_PCI);

    //attempt to dispatch the command queue
    CmdDispatchFrame();

    if(!_Forensics->LoadedForensics)
        ForensicsParse(0, NULL);
}

void MainWindow::CmdParseCommand(QByteArray &Command)
{
    char CurrTok[CSSE_COM_CMDMAXTOKSIZE + 1];

    auto cmd = new std::string(Command.constData());
    //std::replace(cmd->begin(), cmd->end(), '\xA', '\x0');
    replaceAll(*cmd, std::string("\xA"), std::string());

    const char* currChar;
    int count;

    ServerCommand* svc = new ServerCommand();

    // collect frame id
    count = 0;
    currChar = cmd->c_str();
    while(*currChar != 0 && count < CSSE_COM_CMDMAXDIGITS && *currChar != ' ')
    {
        CurrTok[count++] = *currChar;
        currChar++;
    }
    CurrTok[count] = 0;

    if(*currChar != 0)
        currChar++;

    svc->ServerFrame = atoi(CurrTok); // 0 is invalid, since sv_frame is always >= 1

    if(!svc->ServerFrame)
    {
        delete cmd;
        delete svc;
        return; // skip frame input because this is malformatted
    }

    // collect numargs
    count = 0;
    while(*currChar != 0 && count < CSSE_COM_CMDMAXDIGITS && *currChar != ' ')
    {
        CurrTok[count++] = *currChar;
        currChar++;
    }
    CurrTok[count] = 0;

    if(*currChar != 0)
        currChar++;

    svc->NumArgs = (char)atoi(CurrTok);

    if(svc->NumArgs > CSSE_COM_CMDMAXARGS)
    {
        delete cmd;
        delete svc;
        return; // skip frame input because there are too many args
    }

    // create command hash
    count = 0;
    while(*currChar != 0 && *currChar != ' ' && count < CSSE_COM_CMDMAXTOKSIZE)
    {
        CurrTok[count++] = *currChar;
        currChar++;
    }

    if(count >= CSSE_COM_CMDMAXTOKSIZE)
    {
        delete cmd;
        delete svc;
        return; // skip frame input because the command token was malformatted
    }
    CurrTok[count] = 0;

    if(*currChar != 0)
        currChar++;

    svc->CommandID = fnv1a(CurrTok, count);

    int argc = 0;
    svc->Args = new std::list<std::string*>();

    // parse arguments
    while(argc > svc->NumArgs && *currChar != 0)
    {
        count = 0;
        while(count < CSSE_COM_CMDMAXTOKSIZE && *currChar != 0 && *currChar != ' ')
        {
            CurrTok[count++] = *currChar;
            currChar++;
        }

        if(count >= CSSE_COM_CMDMAXTOKSIZE)
        {
            delete cmd;
            delete svc;
            return; // skip frame input because the command token was malformatted
        }

        if(*currChar != 0)
            currChar++;

        CurrTok[count] = 0;

        svc->Args->push_back(new std::string(CurrTok));
        argc++;
    }

    // drop input buffer if the command queue gets too large
    if(CommandQueue->size() > CSSE_COM_MAXQUEUE)
        CommandQueue->clear();

    CommandQueue->push_back(svc);

    delete cmd;
}

uint MainWindow::CmdLocateMinFrame()
{
    uint32_t MinValue = 0xFFFFFFFF;

    for(auto it = CommandQueue->begin(); it != CommandQueue->end(); ++it)
    {
        ServerCommand *cmd = *it;
        if(cmd->ServerFrame <= CommandFrame)
            return CommandFrame;

        if(cmd->ServerFrame < MinValue)
            MinValue = cmd->ServerFrame;
    }

    return MinValue;
}

void MainWindow::CmdDispatchFrame()
{
    CommandFrame++;
    CommandFrame = CmdLocateMinFrame();
    std::list<ServerCommand*>* dispatched = new std::list<ServerCommand*>();

    for(auto it = CommandQueue->begin(); it != CommandQueue->end(); ++it)
    {
        ServerCommand *cmd = *it;
        if(cmd->ServerFrame > CommandFrame)
            continue;

        try
        {
            DispatchCommand(cmd);
            dispatched->push_back(cmd);
        }
        catch (...)
        {
        }
    }

    for(auto it = dispatched->begin(); it != dispatched->end(); ++it)
    {
        ServerCommand *_cmd = *it;

        if(_cmd == nullptr)
            continue;

        CommandQueue->remove(_cmd);

        for(auto it1 = _cmd->Args->begin(); it1 != _cmd->Args->end(); ++it1)
        {
            std::string* carg = *it1;

            if(carg != nullptr)
                delete carg;
        }

        if(_cmd->Args != nullptr)
            delete _cmd->Args;

        delete _cmd;
    }

    delete dispatched;
}

void MainWindow::DispatchCommand(ServerCommand* cmd)
{
    switch(cmd->CommandID)
    {
        CMD_SwitchCase(CSSE_CMD_NotifyEvent, MainWindow::NotifyEvent, cmd->NumArgs, cmd->Args)
        CMD_SwitchCase(CSSE_CMD_ForensicsParse, MainWindow::ForensicsParse, cmd->NumArgs, cmd->Args)
        default:
            return;
    }
}

// attempt to acquire a specific mutex for read/write ops.
// note that this function will never attempt to validate the need for a mutex (ex: mutex for a non-existant file)
bool MainWindow::AcquireMutex(const char* PipeName)
{
    auto ServerMutex = CSSE_MUTEX_STR(PipeName, CSSE_COM_ENGINE);
    auto ClientMutex = CSSE_MUTEX_STR(PipeName, CSSE_COM_CLIENT);

    if(QFile::exists(ServerMutex.c_str()))
    {
        // check if both the server and client mutex exist first
        // if they do, delete the client mutex and return false
        MainWindow::ReleaseMutex(PipeName);

        // the server has a mutex, but we dont, so return false. Server always supercedes client mutex.
        return false;
    }

    // then check if we have the mutex already
    // if so, return true
    if(QFile::exists(ClientMutex.c_str()))
        return true;

    // finally, attempt to create a mutex.
    // if the file exists after its created, we can return true.
    QFile Mutex(ClientMutex.c_str());

    bool result = false;
    if(Mutex.open(QIODevice::NewOnly))
    {
        Mutex.write("csse-mutex-shiversoftdev", 24);
        Mutex.close();
        result = true;
    }

    return result;
}

// attempt to release a mutex for a given pipe
void MainWindow::ReleaseMutex(const char* PipeName)
{
    auto ClientMutex = CSSE_MUTEX_STR(PipeName, CSSE_COM_CLIENT);

    QFile Mutex(ClientMutex.c_str());

    if(Mutex.exists())
        Mutex.remove();
}

// commands

void MainWindow::NotifyEvent(char, std::list<std::string*>*)
{
    CMD_ParseScores(true);
}

void MainWindow::ForensicsParse(char, std::list<std::string*>*)
{
    auto x = _SRP->GetForensicsConst();

    if(x == nullptr)
         CMD_ParseScores(true);

    x = _SRP->GetForensicsConst();
    if(x == nullptr)
        return;

    _Forensics->CopyForensicsData(x);
}

void MainWindow::ShowScoringReport()
{
    StaticMain->ClearNotifies();
    if(StaticMain->ActivePane != StaticMain->_SRP)
    {
        MagistrateAudio::PlaySong(SongID::ScoringReport);
        StaticMain->ChangeActivePane(StaticMain->ui->btn_srp, StaticMain->_SRP);
    }
}

void MainWindow::ClearNotifies()
{
    int index = 0;
    QLayoutItem* item;
    while (index < ui->NotificationLayout->count() && (item = ui->NotificationLayout->itemAt(index)))
    {
        if(item == ui->notifypusher)
        {
            index++;
            continue;
        }
        ui->NotificationLayout->takeAt(index);
        delete item->widget();
    }
}

qreal MainWindow::FontScalar()
{
    return .5 + (Preferences.FontScalar * .25);
}

void MainWindow::SetGlobalFontScalar(int scalar)
{
    Preferences.FontScalar = scalar;
    SetFonts();
    emit GlobalFontUpdate();
    TrySavePrefs();
}

void MainWindow::SetSafeFontEnabled(bool enabled)
{
    StaticMain->Preferences.UseSimpleFont = enabled;
    SetFonts();
    emit GlobalFontUpdate();
    TrySavePrefs();
}
