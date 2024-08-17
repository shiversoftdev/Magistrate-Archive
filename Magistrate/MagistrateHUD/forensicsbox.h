#ifndef FORENSICSBOX_H
#define FORENSICSBOX_H

#include <QWidget>
#include "ScoringTypes.h"
#include "forensicsanswerbox.h"

namespace Ui {
class forensicsbox;
}

class forensicsbox : public QWidget
{
    Q_OBJECT

public:
    explicit forensicsbox(QWidget *parent = nullptr);
    ~forensicsbox();

    void ApplyForensicsData(ForensicsQuestion*);
private:
    Ui::forensicsbox *ui;

    void SubmitClicked();

    void GlobalFontUpdated();

    uint32_t ID;

    std::list<ForensicsAnswerBox*>* AnswerBoxes;
};

#endif // FORENSICSBOX_H
