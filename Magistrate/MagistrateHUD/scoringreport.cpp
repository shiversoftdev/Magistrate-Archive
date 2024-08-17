#include "scoringreport.h"
#include "ui_scoringreport.h"
#include <QGraphicsBlurEffect>
#include <QtDebug>
#include <QFontDatabase>
#include <QString>
#include <VM_CONST.h>
#include <mainwindow.h>
#include <QClipboard>
#include <QGuiApplication>
#include <QPushButton>
#include <QMessageBox>
#include <QCloseEvent>
#include "VM_CONST.h"
#include "CSSE_CONST.h"

ScoringReport::ScoringReport(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::ScoringReport)
{
    ui->setupUi(this);

    setWindowFlags(Qt::Tool);

    ScoreDigits = new std::list<QLabel*>();
    CheckDigits = new std::list<QLabel*>();
    Messages = new std::list<ScoreLineItem*>();

    //setAttribute(Qt::WA_NoSystemBackground, true);
    //setAttribute(Qt::WA_TranslucentBackground, true);
    //setAttribute(Qt::WA_PaintOnScreen); // not needed in Qt 5.2 and up

    SetScore(0);
    SetChecks(0);

    Scores = nullptr;
    ScoreBuffer = NULL;

    ui->scrollArea->setWidgetResizable(true);

    connect(MainWindow::StaticMain, &MainWindow::GlobalFontUpdate, this, &ScoringReport::GlobalFontUpdated);

    GlobalFontUpdated();

    connect(ui->copysrp, &QPushButton::clicked, this, &ScoringReport::CopyScoringReport);

    QTimer::singleShot(0, this, &ScoringReport::UpdateScalarHack);
    connect(&TimePlayedTick, &QTimer::timeout, this, QOverload<>::of(&ScoringReport::TickTimer));
}

void ScoringReport::closeEvent(QCloseEvent *event)
{
    if(MainWindow::StaticMain->CloseAllowed)
        event->accept();
    else
        event->ignore();
}

void ScoringReport::UpdateScalarHack()
{
    auto maxheight = size().height() * .5;

    qreal calcheight = 0.;

    if(Messages->size())
    {
        calcheight = Messages->size() * ((*Messages->begin())->sizeHint().height() + 5);
    }
    else
    {
        calcheight = 400.;
    }

    auto height = std::min(maxheight, calcheight);

    ui->scrollArea->setMinimumHeight(height);

    ScoringReport::UpdateLayoutGeo();
}

void ScoringReport::resizeEvent(QResizeEvent* event)
{
   QWidget::resizeEvent(event);
   ScoringReport::UpdateScalarHack();
}

void ScoringReport::GlobalFontUpdated()
{
    ui->Footer_Cred_Rainbow->setFont(MainWindow::GetFont(15));
    ui->Footer_Cred_Shiv->setFont(MainWindow::GetFont(15));
    ui->Footer_Cred_Kali->setFont(MainWindow::GetFont(15));
    ui->Footer_Cred_Matt->setFont(MainWindow::GetFont(15));
    ui->TimeLabel->setFont(MainWindow::GetFont(16));
    ui->SI2Label->setFont(MainWindow::GetFont(16));
    ui->copysrp->setFont(MainWindow::GetFont(15));
    updateGeometry();
    ui->CheckListLayout->updateGeometry();
    ScoringReport::UpdateScalarHack();
}

ScoringReport::~ScoringReport()
{
    if(Scores != NULL)
        delete Scores;

    delete ScoreDigits;
    delete CheckDigits;
    delete Messages;
    delete ui;
}

void ScoringReport::CopyScoringReport()
{
    std::string FinalText = std::string("<!DOCTYPE html><html><body style=\"background-color: #1b1b21\"><ul style=\"color:#FFFFFF\">");

    for(auto it = Messages->begin(); it != Messages->end(); it++)
    {
        auto msg = *it;
        FinalText += std::string("<li>") + std::string(msg->GetMsgText()) + std::string("</li>\r\n");
    }

    FinalText += std::string("<li>" VM_TEXT_EarnedVerb " ") + std::to_string(Scores->NumChecksPassed) + std::string(" out of ") + std::to_string(Scores->MaxChecksPassed) + std::string(" " VM_TEXT_ChecksAlias) + std::string(" (") + std::string(Scores->SI2) + std::string(")</li>");
    FinalText += std::string("<li>Total " VM_TEXT_PointsAlias ": ") + std::to_string(CacheScore) + std::string("</li>\r\n");
    FinalText += std::string("</ul></body></html>");
    QGuiApplication::clipboard()->setText(FinalText.c_str());

    QMessageBox *msgbox = new QMessageBox(this);

    msgbox->setAttribute(Qt::WA_DeleteOnClose);
    msgbox->setFont(MainWindow::GetFont(15));
    msgbox->setWindowTitle("Report Copied!");
    msgbox->setText("Your scoring report has been copied to your clipboard!");
    msgbox->setStyleSheet("background-color: rgb(29, 31, 33); color: rgb(220,220,220);");
    msgbox->addButton(tr("Accept"), QMessageBox::YesRole);
    msgbox->buttons().at(0)->setStyleSheet("border: 1px solid rgba(220,220,220,80);border-radius: 4px;padding-left: 8px;padding-right: 8px;padding-top: 4px;padding-bottom: 4px;");
    msgbox->buttons().at(0)->setFont(MainWindow::GetFont(15));
    msgbox->buttons().at(0)->setCursor(Qt::PointingHandCursor);
    msgbox->open();
    MagistrateAudio::PlaySFX(SFXID::UISuccess);
}

void ScoringReport::SetScore(int Score)
{
    CacheScore = Score;

    std::string scoreString = std::to_string(Score);
    uint numDigits = scoreString.length();
    BalanceDigits(numDigits, ScoreDigits);

    auto li = ScoreDigits->begin();
    for(uint i = 0; i < numDigits; i++)
    {
        (*li)->setPixmap(QPixmap(PathToDigit(scoreString.at(i))));
        ui->pointLayout->removeWidget(*li);
        ui->pointLayout->addWidget(*li);
        (*li)->show();
        std::advance(li, 1);
    }
    ui->pointLayout->update();
}

void ScoringReport::SetChecks(int CheckCount)
{
    std::string scoreString = std::to_string(CheckCount);
    uint numDigits = scoreString.length();
    BalanceDigits(numDigits, CheckDigits);

    auto li = CheckDigits->begin();
    for(uint i = 0; i < numDigits; i++)
    {
        (*li)->setPixmap(QPixmap(PathToDigit(scoreString.at(i))));
        ui->checksLayout->removeWidget(*li);
        ui->checksLayout->addWidget(*li);
        (*li)->show();
        std::advance(li, 1);
    }
    ui->checksLayout->update();
}

void ScoringReport::BalanceDigits(uint NumDigits, std::list<QLabel*>* DigitList)
{
    while(DigitList->size() != NumDigits)
    {
        if(DigitList->size() > NumDigits)
        {
            auto removed = *DigitList->begin();
            DigitList->pop_front();
            removed->parentWidget()->layout()->removeWidget(removed);
            delete removed;
        }
        else
        {
            auto newDigit = ScoringReport::CreateDigit(0);
            DigitList->push_front(newDigit);
        }
    }
}

 QLabel* ScoringReport::CreateDigit(int value)
 {
    QLabel *label =  new QLabel();
    label->setText("");
    label->setMaximumWidth(96);
    label->setMaximumHeight(96);
    label->setSizePolicy(QSizePolicy::Policy::Expanding, QSizePolicy::Policy::Expanding);
    label->setScaledContents(true);
    label->setAlignment(Qt::AlignHCenter | Qt::AlignVCenter);
    label->setStyleSheet("color:rgb(255,255,255);");

    if(value > 9)
        value = 9;

    if(value < 0)
        value = 0;

    label->setPixmap(QPixmap(ScoringReport::PathToIntDigit(value)));

    return label;
 }

 const char* ScoringReport::PathToIntDigit(int value)
 {
    return PathToDigit((char)('0' + value));
 }

 const char* ScoringReport::PathToDigit(char c) //memory leak? allocated but never deallocated...
 {
    if(c == '-') c = 'M';

    std::string Path = std::string(":/vm/img/vm_") + std::string(&c, 1) + std::string(".png");

    //moves the string from the stack to the heap
    char * cstr = new char [Path.length() + 1];
#ifdef IMAGE_IS_WINDOWS
    std::strcpy(cstr, Path.c_str());
#else
    strcpy(cstr, Path.c_str());
#endif
    return cstr;
 }

 //remember to de-allocate the input buffer after this call. We will copy all the necessary data to the heap.
 void ScoringReport::CopyScoringData(const char* InputBuffer, int32_t BufferSize, bool doNotify)
 {
    if(std::string(InputBuffer) != std::string("SESCORE"))
        return; //dont parse an input buffer with bad magic.

    ScoreEntry *entry;
    int MaxCount;
    int PointDelta = 0;

    if(doNotify && Scores != nullptr)
    {
        auto IBuf = (ScoringData*)InputBuffer;
        auto TEntry = (ScoreEntry*)((char*)IBuf + IBuf->ScoreEntryTableOff);
        auto TMax = (IBuf->NumChecksFailed + IBuf->NumChecksPassed);
        //O(2n^2) gang

        for(uint i = 0; i < TMax; i++, TEntry++)
        {
            entry = Scores->ScoreEntryTable;
            MaxCount = Scores->NumChecksFailed + Scores->NumChecksPassed;
            bool _continue = false;
            int j = 0;
            for(; j < MaxCount && entry; j++, entry++)
            {
                if(entry->ID == TEntry->ID)
                {
                    _continue = true;
                    break;
                }
            }

            if(_continue || j < MaxCount)
                continue;

            if(TEntry->ScoreValue == 0)
                continue;

            PointDelta += TEntry->ScoreValue;

            // new entry
            if(TEntry->ScoreValue > 0)
                MainWindow::DoNotify((std::string("You gained <font color='lightgreen'>") + std::to_string(TEntry->ScoreValue) + std::string("</font> " VM_TEXT_PointsAlias "!")).c_str(), VM_ICO_PointsGained);
            else
                MainWindow::DoNotify((std::string("You lost <font color='red'>") + std::to_string(-1 * TEntry->ScoreValue) + std::string("</font> " VM_TEXT_PointsAlias "!")).c_str(), VM_ICO_PointsLost);
        }

        TEntry = (ScoreEntry*)((char*)Scores + Scores->ScoreEntryTableOff);
        TMax = (Scores->NumChecksFailed + Scores->NumChecksPassed);

        for(uint i = 0; i < TMax; i++, TEntry++)
        {
            entry = (ScoreEntry*)((char*)IBuf + IBuf->ScoreEntryTableOff);
            MaxCount = IBuf->NumChecksFailed + IBuf->NumChecksPassed;
            bool _continue = false;
            int j = 0;
            for(; j < MaxCount && entry; j++, entry++)
            {
                if(entry->ID == TEntry->ID)
                {
                    _continue = true;
                    break;
                }
            }

            if(_continue || j < MaxCount)
                continue;

            if(TEntry->ScoreValue == 0)
                continue;

            PointDelta += -1 * TEntry->ScoreValue;

            // new entry
            if(TEntry->ScoreValue < 0)
                MainWindow::DoNotify((std::string("You gained <font color='lightgreen'>") + std::to_string(-1 * TEntry->ScoreValue) + std::string("</font> " VM_TEXT_PointsAlias "!")).c_str(), VM_ICO_PointsGained);
            else
                MainWindow::DoNotify((std::string("You lost <font color='red'>") + std::to_string(TEntry->ScoreValue) + std::string("</font> " VM_TEXT_PointsAlias "!")).c_str(), VM_ICO_PointsLost);
        }

        if(PointDelta > 0)
        {
            MagistrateAudio::PlaySFX(SFXID::PointGain);
        }
        else if(PointDelta < 0)
        {
            MagistrateAudio::PlaySFX(SFXID::PointLoss);
        }
    }

    if(ScoreBuffer != NULL)
        delete ScoreBuffer;

    ScoreBuffer = (char*)malloc(BufferSize);
    memcpy(ScoreBuffer, InputBuffer, BufferSize);

    Scores = (ScoringData*)ScoreBuffer;
    Scores->ScoreEntryTable = (ScoreEntry*)((char*)Scores + Scores->ScoreEntryTableOff);
    Scores->Forensics = (ForensicsData*)((char*)Scores + Scores->ForensicsDataOff);
    Scores->Forensics->Questions = (ForensicsQuestion*)((char*)Scores->Forensics + sizeof(ForensicsData));
    Scores->SI2 = (char*)((char*)Scores + Scores->SI2Off);

    SetChecks(Scores->NumChecksPassed);

    int score = 0;
    entry = Scores->ScoreEntryTable;
    MaxCount = Scores->NumChecksFailed + Scores->NumChecksPassed;

    ClearMessageItems();
    ui->scrollArea->setMinimumSize(400,ui->scrollArea->minimumSize().height());

    for(int i = 0; i < MaxCount; i++)
    {
        // fixup the score pointers
        entry->CI2 = (const char *)&entry->CI2Str;
        entry->Description = (const char*)&entry->ScoreMessage;

        // update the scorevalue as long as our CI2 is not null
        if(*entry->CI2)
        {
            score += entry->ScoreValue;
            AddMessageItem(entry);
        }

        // next entry in the array
        entry++;
    }

    ForensicsQuestion *fqe = Scores->Forensics->Questions;

    for(uint i = 0; i < Scores->Forensics->NumQuestions; i++)
    {
        // fixup answer pointers
        fqe->Answers = (ForensicsAnswer*)((char*)fqe + sizeof(ForensicsQuestion));

        fqe = (ForensicsQuestion*)((char*)fqe + (fqe->NumAnswers * sizeof(ForensicsAnswer)) + sizeof(ForensicsQuestion));
    }

    std::string FinalText = std::string(VM_TEXT_EarnedVerb " ") + std::to_string(Scores->NumChecksPassed) + std::string(" out of ") + std::to_string(Scores->MaxChecksPassed) + std::string(" " VM_TEXT_ChecksAlias) + std::string(" (") + std::string(Scores->SI2) + std::string(")");

    ui->SI2Label->setText(QString(FinalText.c_str()));

    SetScore(score);

    //ui->ChecksCompleteLabel->setText((std::string("Acquired ") + std::to_string(Scores->NumChecksPassed) + std::string(" out of ") + std::to_string(Scores->MaxChecksPassed) + std::string(" stars")).c_str());

    //todo sound track update
    if(Scores->NumChecksPassed >= VM_PROG_7)
        CurrentProgression = SongID::Progression7;
    else if(Scores->NumChecksPassed >= VM_PROG_6)
        CurrentProgression = SongID::Progression6;
    else if(Scores->NumChecksPassed >= VM_PROG_5)
        CurrentProgression = SongID::Progression5;
    else if(Scores->NumChecksPassed >= VM_PROG_4)
        CurrentProgression = SongID::Progression4;
    else if(Scores->NumChecksPassed >= VM_PROG_3)
        CurrentProgression = SongID::Progression3;
    else if(Scores->NumChecksPassed >= VM_PROG_2)
        CurrentProgression = SongID::Progression2;
    else
        CurrentProgression = SongID::Progression1;
    MainWindow::UpdateProgressSong();

    ScoringReport::UpdateScalarHack();

#ifdef VM_USES_TIME
    QFile PTS(CSSE_PTS);
    if(PTS.open(QIODevice::ReadOnly | QIODevice::Text))
    {
        auto data = PTS.readAll();
        const PTS_s* pts = (PTS_s*)data.data();
        MaxSecondsPlayed = ~pts->MaxTimeCrypt;
        SecondsPlayed = pts->TimePlayedCrypt ^ VM_TIMEXORCONST;
        PTS.close();
    }
    if(!TimePlayedTick.isActive()) TimePlayedTick.start(1000);
#endif
 }

 void ScoringReport::TickTimer()
 {
    char TimePlayedBuff[256];
    SecondsPlayed++;
    // Validation to fix stupid bs from the engine or antivirus
    if(MaxSecondsPlayed > VM_MAX_TIME_FORCED) MaxSecondsPlayed = VM_MAX_TIME_FORCED;
    if(SecondsPlayed > MaxSecondsPlayed) SecondsPlayed = MaxSecondsPlayed;
    uint64_t work_copy = qMax((uint64_t)0, MaxSecondsPlayed - qMin(SecondsPlayed, MaxSecondsPlayed));
    if(!work_copy)
    {
        ui->TimeLabel->setText("Out of time!");
        MainWindow::StaticMain->OutOfTime();
    }
    else
    {
        uint64_t hours = work_copy / 3600;
        work_copy -= hours * 3600;
        uint64_t minutes = work_copy / 60;
        work_copy -= minutes * 60;
        sprintf(TimePlayedBuff, "Time remaining: %02d:%02d:%02d", (int)hours, (int)minutes, (int)work_copy);
        ui->TimeLabel->setText(TimePlayedBuff);
    }
 }

 int ScoringReport::SanitizeWidth(int width)
 {
    auto result = qMin((int)(size().width() * .8), width);

    return result;
 }

 void ScoringReport::AddMessageItem(ScoreEntry* entry)
 {
    if(entry == NULL)
        return;

    ScoreLineItem* li = new ScoreLineItem(this);

    li->SetScoreParams(entry->ScoreValue, entry->Description, entry->CI2);

    Messages->push_back(li);

    ui->scrollArea->setMinimumSize(qMax(SanitizeWidth(li->GetDesiredWidth()), ui->scrollArea->minimumSize().width()),ui->scrollArea->minimumSize().height());

    //ui->CheckListLayout->removeItem(ui->CheckPushBottom);
    ui->CheckListLayout->layout()->addWidget(li);
    //ui->CheckListLayout->addItem(ui->CheckPushBottom);

    ui->checksLayout->update();

    /*
    if(ui->CheckListLayout->sizeHint().height() > ui->scrollArea->size().height())
    {
        ui->CheckPushBottom->changeSize(10,20,QSizePolicy::Minimum,QSizePolicy::Minimum);
        ui->CheckPushTop->changeSize(10,20,QSizePolicy::Minimum,QSizePolicy::Minimum);
    }
    else
    {
        ui->CheckPushBottom->changeSize(10,20,QSizePolicy::Minimum,QSizePolicy::MinimumExpanding);
        ui->CheckPushTop->changeSize(10,20,QSizePolicy::Minimum,QSizePolicy::MinimumExpanding);
    }
    */

    connect(li, &ScoreLineItem::UpdatedSize, this, &ScoringReport::UpdateLayoutGeo);
 }

 void ScoringReport::UpdateLayoutGeo()
 {
    ui->scrollArea->setMinimumSize(400,ui->scrollArea->minimumSize().height());
    for(auto it = Messages->begin(); it != Messages->end(); it++)
    {
        auto li = *it;
        ui->scrollArea->setMinimumSize(qMax(SanitizeWidth(li->GetDesiredWidth()), ui->scrollArea->minimumSize().width()),ui->scrollArea->minimumSize().height());
    }
 }

 void ScoringReport::ClearMessageItems()
 {
    while(Messages->size() > 0)
    {
        auto removed = *Messages->begin();
        Messages->pop_front();
        removed->parentWidget()->layout()->removeWidget(removed);
        delete removed;
    }
 }

 ForensicsData* ScoringReport::GetForensicsConst()
 {
     if(Scores == nullptr)
         return nullptr;

    return Scores->Forensics;
 }
