#include "mnotifyform.h"
#include "ui_mnotifyform.h"
#include <QGraphicsEffect>
#include <mainwindow.h>
#include <QPropertyAnimation>

MNotifyForm::MNotifyForm(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::MNotifyForm)
{
    ui->setupUi(this);
    QGraphicsDropShadowEffect *shadowEffect = new QGraphicsDropShadowEffect;
    shadowEffect->setColor(QColor(255, 255, 255, 140));
    shadowEffect->setOffset(0);
    shadowEffect->setBlurRadius(5);
    setGraphicsEffect(shadowEffect);

    QTimer::singleShot(5000, this, SLOT(FadeAndDestroy()));
}

MNotifyForm::~MNotifyForm()
{
    delete ui;
}

void MNotifyForm::SetNotifyData(const char* msg, const char* ico)
{
    ui->StarIcon->setPixmap(QPixmap(ico));
    ui->StarIcon_2->setPixmap(QPixmap(ico));
    ui->Message->setText(msg);
}

void MNotifyForm::mousePressEvent(QMouseEvent*)
{
    MainWindow::ShowScoringReport();
}

void MNotifyForm::FadeAndDestroy()
{
    if(DestroyInvoked)
        return;

    if(!FadeInvoked)
    {
        QGraphicsOpacityEffect *eff = new QGraphicsOpacityEffect(this);
        setGraphicsEffect(eff);
        QPropertyAnimation *a = new QPropertyAnimation(eff,"opacity");
        a->setDuration(1000);
        a->setStartValue(1);
        a->setEndValue(0);
        a->setEasingCurve(QEasingCurve::OutBack);
        a->start(QPropertyAnimation::DeleteWhenStopped);

        FadeInvoked = true;

        QTimer::singleShot(1000, this, SLOT(FadeAndDestroy()));
        return;
    }

    DestroyInvoked = true;

    emit RemoveMe(this);
}

