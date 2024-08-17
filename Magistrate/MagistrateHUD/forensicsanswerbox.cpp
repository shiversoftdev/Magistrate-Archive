#include "forensicsanswerbox.h"
#include "ui_forensicsanswerbox.h"
#include <QFontDatabase>
#include <mainwindow.h>

ForensicsAnswerBox::ForensicsAnswerBox(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::ForensicsAnswerBox)
{
    ui->setupUi(this);

    AnswerInputMatched = false;

    connect(MainWindow::StaticMain, &MainWindow::GlobalFontUpdate, this, &ForensicsAnswerBox::GlobalFontUpdated);
    GlobalFontUpdated();

    connect(ui->TextValue, &QLineEdit::textChanged, this, &ForensicsAnswerBox::textEdited);
}

void ForensicsAnswerBox::GlobalFontUpdated()
{
    ui->NameLabel->setFont(MainWindow::GetFont(16));
    ui->TextValue->setFont(MainWindow::GetFont(16));
}

ForensicsAnswerBox::~ForensicsAnswerBox()
{
    delete ui;
}

void ForensicsAnswerBox::SetAnswerData(ForensicsAnswer* ans)
{
    if(ans == nullptr)
        return;

    ui->NameLabel->setText(ans->NameText);
    InputRegex.setPattern(ans->FormatString);
}

void ForensicsAnswerBox::textEdited(const QString &text)
{
    if(text.length() == 0 || text.length() > 1024)
    {
        AnswerInputMatched = false;
        ui->TextValue->setStyleSheet("color: rgb(220,220,220); background-color: rgba(0,0,0,140); border: 1px solid rgba(255,255,255,40);");
        return;
    }

    strcpy(TextData, text.toStdString().c_str());

    QRegularExpressionMatch match = InputRegex.match(text);
    AnswerInputMatched = match.hasMatch();

    if(AnswerInputMatched)
    {
        ui->TextValue->setStyleSheet("color: rgb(220,220,220); background-color: rgba(0,0,0,140); border: 1px solid rgba(0,255,0,120);");
    }
    else
    {
        ui->TextValue->setStyleSheet("color: rgb(220,220,220); background-color: rgba(0,0,0,140); border: 1px solid rgba(255,0,0,120);");
    }
}

void ForensicsAnswerBox::SetAnswerValue(std::string *PreAnswer)
{
    ui->TextValue->setText(QString(PreAnswer->c_str()));
}
