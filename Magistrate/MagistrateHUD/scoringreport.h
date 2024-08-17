#ifndef SCORINGREPORT_H
#define SCORINGREPORT_H

#include <QWidget>
#include <QLabel>
#include <ScoringTypes.h>
#include <scorelineitem.h>
#include <MagistrateAudio.h>
#include <QTimer>

namespace Ui {
class ScoringReport;
}

class ScoringReport : public QWidget
{
    Q_OBJECT

public:
    explicit ScoringReport(QWidget *parent = nullptr);
    ~ScoringReport();

    void SetScore(int ScoreValue);
    void SetChecks(int CheckCount);
    void CopyScoringData(const char* InputData, int32_t BufferSize, bool doNotify);
    ForensicsData* GetForensicsConst();

    SongID CurrentProgression = SongID::Progression1;

    void resizeEvent(QResizeEvent* event) override;
    void closeEvent(QCloseEvent *event) override;
public slots:
    void CopyScoringReport();

private:
    Ui::ScoringReport *ui;
    std::list<QLabel*> *ScoreDigits;
    std::list<QLabel*> *CheckDigits;
    std::list<ScoreLineItem*> *Messages;
    QTimer TimePlayedTick;
    uint64_t SecondsPlayed = 0;
    uint64_t MaxSecondsPlayed = 359999;
    void UpdateScalarHack();
    void TickTimer();

    void BalanceDigits(uint NumDigits, std::list<QLabel*>* DigitList);

    void ClearMessageItems();
    void AddMessageItem(ScoreEntry* entry);
    void GlobalFontUpdated();

    int SanitizeWidth(int width);

    void UpdateLayoutGeo();

    QLabel* CreateDigit(int value);

    const char* PathToIntDigit(int value);
    const char* PathToDigit(char c);

    ScoringData* Scores;

    char* ScoreBuffer;
    int CacheScore;
};

#endif // SCORINGREPORT_H
