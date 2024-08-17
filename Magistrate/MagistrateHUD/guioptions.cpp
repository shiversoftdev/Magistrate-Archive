#include "guioptions.h"
#include "ui_guioptions.h"
#include <mainwindow.h>
#include <QPushButton>
#include <MagistrateAudio.h>
#include <CSSE_CONST.h>
#include <QCloseEvent>

GUIOptions::GUIOptions(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::GUIOptions)
{
    ui->setupUi(this);
    setWindowFlags(Qt::Tool);

    GlobalFontUpdated();
    connect(MainWindow::StaticMain, &MainWindow::GlobalFontUpdate, this, &GUIOptions::GlobalFontUpdated);
    connect(ui->FontChanger, &QPushButton::clicked, this, &GUIOptions::ToggleGlobalFont);

    ui->FontChanger->setChecked(MainWindow::StaticMain->Preferences.UseSimpleFont);
    ui->FontChanger->setText(MainWindow::StaticMain->Preferences.UseSimpleFont ? "Enabled" : "Disabled");

    ui->horizontalSlider->setValue(MainWindow::StaticMain->Preferences.FontScalar);

    connect(ui->horizontalSlider, &QSlider::valueChanged, this, &GUIOptions::SetGlobalFontSize);
}

void GUIOptions::closeEvent(QCloseEvent *event)
{
    if(MainWindow::StaticMain->CloseAllowed)
        event->accept();
    else
        event->ignore();
}

void GUIOptions::SetGlobalFontSize(int value)
{
    MainWindow::StaticMain->SetGlobalFontScalar(value);
}

void GUIOptions::GlobalFontUpdated()
{
    ui->FontLabel->setFont(MainWindow::GetFont(15));
    ui->FontChanger->setFont(MainWindow::GetFont(15));
    ui->FontScaleLabel->setFont(MainWindow::GetFont(15));
}

void GUIOptions::ToggleGlobalFont()
{
    bool result = !MainWindow::StaticMain->Preferences.UseSimpleFont;

    MainWindow::StaticMain->SetSafeFontEnabled(result);

    ui->FontChanger->setChecked(result);
    ui->FontChanger->setText(MainWindow::StaticMain->Preferences.UseSimpleFont ? "Enabled" : "Disabled");

    MagistrateAudio::PlaySFX(SFXID::UIClick, false);
}

GUIOptions::~GUIOptions()
{
    delete ui;
}
