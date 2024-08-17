#include "scorelineitem.h"
#include "ui_scorelineitem.h"
#include <QFontDatabase>
#include <mainwindow.h>

ScoreLineItem::ScoreLineItem(QWidget *parent) :
    QWidget(parent),
    ui(new Ui::ScoreLineItem)
{
    ui->setupUi(this);

    connect(MainWindow::StaticMain, &MainWindow::GlobalFontUpdate, this, &ScoreLineItem::GlobalFontUpdated);
    GlobalFontUpdated();

    MsgEdit = NULL;
}

void ScoreLineItem::GlobalFontUpdated()
{
    ui->Description->setFont(MainWindow::GetFont(20));
    ui->Description->updateGeometry();
    updateGeometry();
    emit UpdatedSize();
}

int ScoreLineItem::GetDesiredWidth()
{
    return ui->Description->sizeHint().width() + 60;
}

const char* ScoreLineItem::GetMsgText()
{
    return MsgEdit->c_str();
}

void ScoreLineItem::replaceAll(std::string& str, const std::string& from, const std::string& to)
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

void ScoreLineItem::SetScoreParams(int ScoreValue, const char* Msg, const char* CI2)
{
    ui->Description->setText(""); //remove reference to old string

    if(MsgEdit != NULL)
        delete MsgEdit;

    MsgEdit = new std::string(Msg);

    replaceAll(*MsgEdit, std::string("{{POINTS}}"), std::to_string(ScoreValue));
    replaceAll(*MsgEdit, std::string("{{POINTCOLOR}}"), std::string(ScoreValue > 0 ? "lightgreen" : "red"));
    replaceAll(*MsgEdit, std::string("{{CI2}}"), std::string(CI2));

    ui->Description->setTextFormat(Qt::RichText);
    ui->Description->setText(MsgEdit->c_str());
}

ScoreLineItem::~ScoreLineItem()
{
    delete ui;
}
