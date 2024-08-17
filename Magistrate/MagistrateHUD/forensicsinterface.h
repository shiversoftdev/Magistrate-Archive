#ifndef FORENSICSINTERFACE_H
#define FORENSICSINTERFACE_H

#include <QWidget>
#include "ScoringTypes.h"

namespace Ui {
class ForensicsInterface;
}

class ForensicsInterface : public QWidget
{
    Q_OBJECT

public:
    explicit ForensicsInterface(QWidget *parent = nullptr);
    ~ForensicsInterface();

    void CopyForensicsData(ForensicsData*);

    void resizeEvent(QResizeEvent* event) override;

    bool LoadedForensics = false;
    bool IsPoppedOut = false;

    void closeEvent(QCloseEvent *event) override;

private:
    Ui::ForensicsInterface *ui;

    void UpdateScalarHack();
    void TabClicked();

    void TogglePopout();

    void GlobalFontUpdated();

    ForensicsData* Forensics;
};

#endif // FORENSICSINTERFACE_H
