#ifndef SCORELINEITEM_H
#define SCORELINEITEM_H

#include <QWidget>

namespace Ui {
class ScoreLineItem;
}

class ScoreLineItem : public QWidget
{
    Q_OBJECT

public:
    explicit ScoreLineItem(QWidget *parent = nullptr);
    ~ScoreLineItem();

    void SetScoreParams(int ScoreValue, const char* Msg, const char* CI2);
    int GetDesiredWidth();

    const char* GetMsgText();


signals:
    void UpdatedSize();

private:
    Ui::ScoreLineItem *ui;

    std::string* MsgEdit;

    void GlobalFontUpdated();

    void replaceAll(std::string& str, const std::string& from, const std::string& to);
};

#endif // SCORELINEITEM_H
