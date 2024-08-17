#ifndef MNOTIFYFORM_H
#define MNOTIFYFORM_H

#include <QWidget>

namespace Ui {
class MNotifyForm;
}

class MNotifyForm : public QWidget
{
    Q_OBJECT

public:
    explicit MNotifyForm(QWidget *parent = nullptr);
    ~MNotifyForm();

    bool FadeInvoked = false;
    bool DestroyInvoked = false;

    void SetNotifyData(const char* msg, const char* ico);

public slots:
    void FadeAndDestroy();

signals:
    void RemoveMe(QWidget*);

protected:
    void mousePressEvent(QMouseEvent *event) override;

private:
    Ui::MNotifyForm *ui;
};

#endif // MNOTIFYFORM_H
