#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QTimer>
#include <scoringreport.h>
#include <forensicsinterface.h>
#include <QPushButton>
#include <QProcess>
#include <QFontDatabase>
#include <guioptions.h>
#include "VM_CONST.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

    static void ShowScoringReport();
    static void DoNotify(const char* MessageText, const char* NotifyIcon);
    static void UpdateProgressSong();
    static void SetFonts();
    static qreal ScaleFont(int font);

    static bool isRunning(const QString &process);
    static MainWindow *StaticMain;

    static QFont GetFont(int Size);

    void SetGlobalFontScalar(int scalar);
    void SetSafeFontEnabled(bool enabled);

    ClientPrefs Preferences;
    bool CloseAllowed = false;

protected:
    virtual bool event(QEvent*) override;

public:
    void OutOfTime();

signals:
    void GlobalFontUpdate();

public slots:
    void btn_DevExit_Clicked();
    void btn_srp_Clicked();
    void btn_forensics_Clicked();
    void btn_readme_Clicked();
    void btn_settings_Clicked();
    void FormOpened();

private:
    Ui::MainWindow *ui;
    ScoringReport *_SRP;
    GUIOptions* _Options;
    ForensicsInterface *_Forensics;
    QTimer *CmdDispatchTimer;
    QWidget* ActivePane;

    static QFont Mono15, Mono16, Mono20;

    qreal FontScalar();


    //int _font_id = QFontDatabase::addApplicationFont(":/vm/MarioFont");
    QString _font_family = QString("Arial");//QFontDatabase::applicationFontFamilies(_font_id).at(0);

    uint64_t CommandFrame;
    QTimer *ResizeHack = new QTimer;
#ifdef IATR
    QTimer *LightsTimer = new QTimer;
    QTimer *FlashTimer = new QTimer;
    uint64_t NumKills = 0;
    void Lights();
    void KillExplorer();
#endif
    QTimer TimeOutTick;
#ifdef IMAGE_IS_WINDOWS
    void KillProcessByName(const char *);
#endif
    void CMD_ParseScores(bool doNotify);

    void CmdDispatch_Tick();

    bool AcquireMutex(const char* PipeName);
    void ReleaseMutex(const char* PipeName);

    void TryLoadPrefs();
    void TrySavePrefs();
    void OutOfTimeLock();
    void CmdParseCommand(QByteArray &Command);
    void CmdDispatchFrame();
    void ResizeTick();
    uint CmdLocateMinFrame();
    void DispatchCommand(ServerCommand*);
    void ChangeActivePane(QPushButton*, QWidget*);

    void closeEvent(QCloseEvent *event) override;

    void replaceAll(std::string& str, const std::string& from, const std::string& to);

    std::list<ServerCommand*>* CommandQueue;
    std::list<QPushButton*>* InstanceButtons;

    void ClearNotifies();
    void RemoveNotify(QWidget*);

    // commands

    void NotifyEvent(char argc, std::list<std::string*>*);
    void ForensicsParse(char argc, std::list<std::string*>*);
};
#endif // MAINWINDOW_H
