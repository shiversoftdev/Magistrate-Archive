#include "forensicsinterface.h"
#include "ui_forensicsinterface.h"
#include <QFontDatabase>
#include <QGraphicsEffect>
#include "forensicsbox.h"
#include <QTabBar>
#include <MagistrateAudio.h>
#include <mainwindow.h>
#include <QGuiApplication>
#include <QScreen>
#include <QTimer>
#include <QCloseEvent>

void ForensicsInterface::UpdateScalarHack()
{
    auto width = size().width() * .5;
    auto height = size().height() * .5;
    ui->ForensicsTabs->setMinimumWidth(width);
    ui->ForensicsTabs->setMinimumHeight(height);
}

ForensicsInterface::ForensicsInterface(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::ForensicsInterface)
{
    ui->setupUi(this);
    //setWindowFlags(Qt::Tool);

    QGraphicsDropShadowEffect *shadowEffect = new QGraphicsDropShadowEffect;
    shadowEffect->setColor(QColor(255, 255, 255, 60));
    shadowEffect->setOffset(0);
    shadowEffect->setBlurRadius(5);
    ui->ForensicsTabs->setGraphicsEffect(shadowEffect);

    connect(MainWindow::StaticMain, &MainWindow::GlobalFontUpdate, this, &ForensicsInterface::GlobalFontUpdated);

    GlobalFontUpdated();

    Forensics = nullptr;

    connect(ui->popout, &QPushButton::clicked, this, &ForensicsInterface::TogglePopout);

    QTimer::singleShot(0, this, &ForensicsInterface::UpdateScalarHack);
}

void ForensicsInterface::closeEvent(QCloseEvent *event)
{
    if(MainWindow::StaticMain->CloseAllowed)
        event->accept();
    else
        event->ignore();
}


void ForensicsInterface::resizeEvent(QResizeEvent* event)
{
   QWidget::resizeEvent(event);
   ForensicsInterface::UpdateScalarHack();
}

void ForensicsInterface::GlobalFontUpdated()
{
    ui->ForensicsTabs->setFont(MainWindow::GetFont(16));
    ui->popout->setFont(MainWindow::GetFont(15));

    ForensicsInterface::UpdateScalarHack();
}

void ForensicsInterface::TogglePopout()
{
    IsPoppedOut = !IsPoppedOut;

    MagistrateAudio::PlaySFX(SFXID::UIClick, false);
    if(IsPoppedOut)
    {
        setWindowFlags(Qt::CustomizeWindowHint | Qt::WindowTitleHint);

        window()->resize(QGuiApplication::primaryScreen()->size().width() * .4,QGuiApplication::primaryScreen()->size().height() * .4);
        showNormal();
    }
    else
    {
        setWindowFlags(Qt::FramelessWindowHint | Qt::SubWindow);
        showFullScreen();
    }
    ForensicsInterface::UpdateScalarHack();
}

ForensicsInterface::~ForensicsInterface()
{
    if(Forensics != nullptr)
        delete Forensics;

    delete ui;
}

void ForensicsInterface::CopyForensicsData(ForensicsData* Data)
{
    if(Data == nullptr)
        return;

    if(Forensics != nullptr)
        delete Forensics;

    Forensics = (ForensicsData*)malloc(Data->BufferSize);
    memcpy(Forensics, Data, Data->BufferSize);

    // relocate pointers
    {
        Forensics->Questions = (ForensicsQuestion*)((char*)Forensics + sizeof(ForensicsData));

        ForensicsQuestion *fqe = Forensics->Questions;

        for(uint i = 0; i < Forensics->NumQuestions; i++)
        {
            // fixup answer pointers
            fqe->Answers = (ForensicsAnswer*)((char*)fqe + sizeof(ForensicsQuestion));

            fqe = (ForensicsQuestion*)((char*)fqe + (fqe->NumAnswers * sizeof(ForensicsAnswer)) + sizeof(ForensicsQuestion));
        }
    }


    //TODO clean up old tab memory allocations
    ui->ForensicsTabs->clear();

    auto entry = Data->Questions;
    for(uint i = 0; i < Data->NumQuestions; i++)
    {
        forensicsbox* fbox = new forensicsbox;
        ui->ForensicsTabs->addTab(fbox, QString((std::string("Question ") + std::to_string(entry->QuestionID + 1)).c_str()));
        fbox->ApplyForensicsData(entry);
        entry = (ForensicsQuestion*)((char*)entry + (entry->NumAnswers * sizeof(ForensicsAnswer)) + sizeof(ForensicsQuestion));
    }
    ui->ForensicsTabs->tabBar()->setCursor(Qt::PointingHandCursor);
    connect(ui->ForensicsTabs->tabBar(), &QTabBar::tabBarClicked, this, &ForensicsInterface::TabClicked);
    LoadedForensics = true;
}

void ForensicsInterface::TabClicked()
{
    MagistrateAudio::PlaySFX(SFXID::UIClick, false);
}
