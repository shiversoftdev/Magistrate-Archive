#ifndef FORENSICSANSWERBOX_H
#define FORENSICSANSWERBOX_H

#include <QWidget>
#include "ScoringTypes.h"
#include <QRegularExpression>

namespace Ui {
class ForensicsAnswerBox;
}

class ForensicsAnswerBox : public QWidget
{
    Q_OBJECT

public:
    explicit ForensicsAnswerBox(QWidget *parent = nullptr);
    ~ForensicsAnswerBox();

    void SetAnswerData(ForensicsAnswer* ans);
    void SetAnswerValue(std::string *PreAnswer);

    void GlobalFontUpdated();

    bool AnswerInputMatched;
    char TextData[1025];

private:
    Ui::ForensicsAnswerBox *ui;

    void textEdited(const QString &text);
    QRegularExpression InputRegex;
};

#endif // FORENSICSANSWERBOX_H
