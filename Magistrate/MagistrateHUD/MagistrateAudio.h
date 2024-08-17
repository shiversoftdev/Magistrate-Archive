#ifndef MAGISTRATEAUDIO_H
#define MAGISTRATEAUDIO_H

#include <QMap>
#include <QtMultimedia/QSound>
#include <QSoundEffect>
#include <QTimer>

enum class SongID : unsigned char
{
    NoSong,
    ScoringReport,
    Forensics,
    Readme,
    Progression1,
    Progression2,
    Progression3,
    Progression4,
    Progression5,
    Progression6,
    Progression7
};

enum class SFXID : unsigned char
{
    NoEffect,
    PointGain,
    PointLoss,
    ScoringReportOpen,
    ForensicsOpen,
    ReadmeOpen,
    UIClick,
    UIWarn,
    UISuccess,
    UIClosePane
};


struct AudioStateInfo
{
    SongID CurrentSong;
    SFXID CurrentEffect;

    int CurrentPlayID;

    bool NoLoop;
};

struct SongPairInfo
{
    QSoundEffect *SongIntro;
    uint32_t IntroLengthMS;
    QSoundEffect *SongLoop;
};

class MagistrateAudio : public QObject
{

public:

    static void PlaySong(SongID song);
    static void RegisterSong(SongID song, const char* SongIntro, uint32_t IntroLengthMS, const char* SongLoop);
    static void PlaySFX(SFXID sfx, bool AutoMute = true);
    static void RegisterSFX(SFXID sfx, const char* sound);
    static void Register();
    static void SetVolume(float vol);
    static bool IsRegistered();

    void IntroComplete();
    void EffectPlayingUpdated();

private:
    static MagistrateAudio &AudioEngine;
    static bool AudioRegistered;


    float Volume = .25f;
    AudioStateInfo AudioState;
    QMap<SongID, SongPairInfo*>* SongMap;
    QMap<SFXID, QSoundEffect*>* SFXMap;

    bool MuteForOneshot = true;

    QTimer SongTimer;

    QSoundEffect* SongPlayer = nullptr;
    QSoundEffect* LoopPlayer = nullptr;
    QSoundEffect* EffectPlayer = nullptr;

    explicit MagistrateAudio();
    ~MagistrateAudio();
};

#endif // MAGISTRATEAUDIO_H
