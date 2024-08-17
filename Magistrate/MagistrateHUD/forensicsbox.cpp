#include "forensicsbox.h"
#include "ui_forensicsbox.h"
#include <QFontDatabase>
#include <QPushButton>
#include <QMessageBox>
#include "CSSE_CONST.h"
#include <QFile>
#include <MagistrateAudio.h>
#include <mainwindow.h>

forensicsbox::forensicsbox(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::forensicsbox)
{
    ui->setupUi(this);

    AnswerBoxes = new std::list<ForensicsAnswerBox*>();

    connect(MainWindow::StaticMain, &MainWindow::GlobalFontUpdate, this, &forensicsbox::GlobalFontUpdated);

    GlobalFontUpdated();

    connect(ui->Submit, &QPushButton::clicked, this, &forensicsbox::SubmitClicked);
}

void forensicsbox::GlobalFontUpdated()
{
    ui->QuestionBox->setFont(MainWindow::GetFont(16));
    ui->Submit->setFont(MainWindow::GetFont(16));
}

forensicsbox::~forensicsbox()
{
    delete AnswerBoxes;
    //delete ui;
}

void forensicsbox::ApplyForensicsData(ForensicsQuestion* data)
{
    ui->QuestionBox->setAcceptRichText(true);
    ui->QuestionBox->setText((std::string("<font color=\"white\">") + std::string(data->QuestionRichText) + std::string("<font/>")).c_str());
    ID = data->QuestionID;

    auto ans = data->Answers;
    for(uint i = 0; i < data->NumAnswers; i++, ans++)
    {
        ForensicsAnswerBox* AnsBox = new ForensicsAnswerBox;
        AnsBox->SetAnswerData(ans);
        AnswerBoxes->push_back(AnsBox);
        ui->AnswerLayout->addWidget(AnsBox);
    }

    // load old answers
    try
    {
        QFile fqo(CSSE_FQNAME(ID).c_str());

        if(!fqo.open(QIODevice::ReadOnly | QIODevice::Text))
            throw new std::exception;

        QByteArray filecontents = fqo.readAll();

        std::string fstr = filecontents.toStdString();

        int index = 0;
        auto it = AnswerBoxes->begin();
        while(it != AnswerBoxes->end())
        {
            auto val = fstr.find(CSSE_FQ_CONCATSTR, index);

            if(val == std::string::npos) break;

            index = val + strlen(CSSE_FQ_CONCATSTR);
            val = fstr.find(CSSE_FQ_CONCATSTR, index);

            //fstr = fstr.substr(index); //purge

            if(val != std::string::npos)
            {
                auto newstr = fstr.substr(index, val - index);
                (*it)->SetAnswerValue(new std::string(newstr));
                index += newstr.length();
                it++;
            }
            else
            {
                auto newstr = fstr.substr(index, val);
                (*it)->SetAnswerValue(new std::string(newstr));
                break;
            }
        }

        fqo.close();
    }
    catch(...)
    {

    }
}

void forensicsbox::SubmitClicked()
{
    QMessageBox *msgbox = new QMessageBox(this);
    std::string AnswersString = "";

    for(auto it = AnswerBoxes->begin(); it != AnswerBoxes->end(); it++)
    {
        auto val = *it;
        if(!val->AnswerInputMatched)
        {
            msgbox->setAttribute(Qt::WA_DeleteOnClose);
            msgbox->setFont(MainWindow::GetFont(16));
            msgbox->setWindowTitle("Answer NOT Submitted!");
            msgbox->setText("One or more of your answers are invalid or empty.");
            msgbox->setStyleSheet("background-color: rgb(29, 31, 33); color: rgb(220,220,220);");
            msgbox->addButton(tr("Accept"), QMessageBox::YesRole);
            msgbox->buttons().at(0)->setStyleSheet("border: 1px solid rgba(220,220,220,80);border-radius: 4px;padding-left: 8px;padding-right: 8px;padding-top: 4px;padding-bottom: 4px;");
            msgbox->buttons().at(0)->setFont(MainWindow::GetFont(16));
            msgbox->buttons().at(0)->setCursor(Qt::PointingHandCursor);
            msgbox->open();
            MagistrateAudio::PlaySFX(SFXID::UIWarn);
            return;
        }
        AnswersString += std::string(CSSE_FQ_CONCATSTR) + std::string(val->TextData);
    }

    try
    {
        QFile fqo(CSSE_FQNAME(ID).c_str());

        if(!fqo.open(QIODevice::ReadWrite | QIODevice::Text | QIODevice::Truncate))
            throw new std::exception;

        fqo.write(AnswersString.c_str());

        fqo.close();
    }
    catch (...)
    {
        msgbox->setAttribute(Qt::WA_DeleteOnClose);
        msgbox->setFont(MainWindow::GetFont(16));
        msgbox->setWindowTitle("Answer NOT Submitted!");
        msgbox->setText("Could not submit answer due to an error in the saving process.");
        msgbox->setStyleSheet("background-color: rgb(29, 31, 33); color: rgb(220,220,220);");
        msgbox->addButton(tr("Accept"), QMessageBox::YesRole);
        msgbox->buttons().at(0)->setStyleSheet("border: 1px solid rgba(220,220,220,80);border-radius: 4px;padding-left: 8px;padding-right: 8px;padding-top: 4px;padding-bottom: 4px;");
        msgbox->buttons().at(0)->setFont(MainWindow::GetFont(16));
        msgbox->buttons().at(0)->setCursor(Qt::PointingHandCursor);
        msgbox->open();
        MagistrateAudio::PlaySFX(SFXID::UIWarn);
        return;
    }

    msgbox->setAttribute(Qt::WA_DeleteOnClose);
    msgbox->setFont(MainWindow::GetFont(16));
    msgbox->setWindowTitle("Answer Submitted!");
    msgbox->setText("Your answers have been saved and are being checked.");
    msgbox->setStyleSheet("background-color: rgb(29, 31, 33); color: rgb(220,220,220);");
    msgbox->addButton(tr("Accept"), QMessageBox::YesRole);
    msgbox->buttons().at(0)->setStyleSheet("border: 1px solid rgba(220,220,220,80);border-radius: 4px;padding-left: 8px;padding-right: 8px;padding-top: 4px;padding-bottom: 4px;");
    msgbox->buttons().at(0)->setFont(MainWindow::GetFont(16));
    msgbox->buttons().at(0)->setCursor(Qt::PointingHandCursor);
    msgbox->open();
    MagistrateAudio::PlaySFX(SFXID::UISuccess);
}
