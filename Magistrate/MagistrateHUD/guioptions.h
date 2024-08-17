#ifndef GUIOPTIONS_H
#define GUIOPTIONS_H

#include <QWidget>
#include <ScoringTypes.h>

namespace Ui {
class GUIOptions;
}

class GUIOptions : public QWidget
{
    Q_OBJECT

public:
    explicit GUIOptions(QWidget *parent = nullptr);
    ~GUIOptions();

    void GlobalFontUpdated();
    void ToggleGlobalFont();

    void SetGlobalFontSize(int value);

    void closeEvent(QCloseEvent *event) override;

private:
    Ui::GUIOptions *ui;
};

#endif // GUIOPTIONS_H
