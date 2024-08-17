#include <MagistrateAudio.h>
#include <QtDebug>
#include <QUrl>

MagistrateAudio &MagistrateAudio::AudioEngine = *new MagistrateAudio;
bool MagistrateAudio::AudioRegistered = false;

MagistrateAudio::MagistrateAudio()
{
    AudioState.CurrentSong = SongID::NoSong;
    AudioState.CurrentEffect = SFXID::NoEffect;
    AudioState.CurrentPlayID = 0;

    SongMap = new QMap<SongID, SongPairInfo*>();
    SFXMap = new QMap<SFXID, QSoundEffect*>();

    //SongTimer.setSingleShot(true);
    //SongTimer.callOnTimeout(this, &MagistrateAudio::IntroComplete);
}

MagistrateAudio::~MagistrateAudio()
{

}

void MagistrateAudio::Register()
{
    if(AudioRegistered)
        return;

    AudioRegistered = true;
}

bool MagistrateAudio::IsRegistered()
{
    return AudioRegistered;
}

void MagistrateAudio::RegisterSong(SongID song, const char* SongIntro, uint32_t IntroLengthMS, const char* SongLoop)
{
    if(AudioEngine.SongMap->contains(song))
        return; // do not re-register a song

    SongPairInfo* spi = new SongPairInfo;
    spi->IntroLengthMS = IntroLengthMS;

    spi->SongLoop = new QSoundEffect;
    spi->SongLoop->setLoopCount(QSoundEffect::Infinite);
    spi->SongLoop->setSource(QUrl(SongLoop));

    spi->SongIntro = new QSoundEffect;
    spi->SongIntro->setLoopCount(0);
    spi->SongIntro->setSource(QUrl(SongIntro));
    connect(spi->SongIntro, &QSoundEffect::playingChanged, &AudioEngine, &MagistrateAudio::IntroComplete);

    AudioEngine.SongMap->insert(song, spi);
}

void MagistrateAudio::RegisterSFX(SFXID sfx, const char* sound)
{
    if(AudioEngine.SFXMap->contains(sfx))
        return; // do not re-register the sfx

    QSoundEffect* sfp =  new QSoundEffect;
    sfp->setSource(QUrl(sound));
    sfp->setLoopCount(0);

    connect(sfp, &QSoundEffect::playingChanged, &AudioEngine, &MagistrateAudio::EffectPlayingUpdated);

    AudioEngine.SFXMap->insert(sfx, sfp);
}

void MagistrateAudio::PlaySong(SongID song)
{
    if(!IsRegistered())
        return;

    if(!AudioEngine.SongMap->contains(song))
        return;

    auto SongData = AudioEngine.SongMap->find(song).value();

    if(AudioEngine.LoopPlayer != nullptr && AudioEngine.LoopPlayer->source().path() == SongData->SongLoop->source().path()) //dont swap song if not necessary
        return;

    AudioEngine.AudioState.NoLoop = true;

    if(AudioEngine.SongPlayer != nullptr)
    {
        AudioEngine.SongPlayer->stop();
    }

    if(AudioEngine.LoopPlayer != nullptr)
    {
        AudioEngine.LoopPlayer->stop();
    }

    AudioEngine.SongPlayer = SongData->SongIntro;
    AudioEngine.LoopPlayer = SongData->SongLoop;

    AudioEngine.SongPlayer->setVolume(AudioEngine.Volume);
    AudioEngine.LoopPlayer->setVolume(AudioEngine.Volume);

    AudioEngine.AudioState.CurrentSong = song;
    AudioEngine.AudioState.CurrentPlayID++; // inc play frame to invalidate other loops

    AudioEngine.AudioState.NoLoop = false;

    AudioEngine.SongPlayer->play();

    if(AudioEngine.LoopPlayer != nullptr)
        AudioEngine.LoopPlayer->setMuted(AudioEngine.EffectPlayer != nullptr && AudioEngine.EffectPlayer->isPlaying() && AudioEngine.MuteForOneshot);

    if(AudioEngine.SongPlayer != nullptr)
        AudioEngine.SongPlayer->setMuted(AudioEngine.EffectPlayer != nullptr && AudioEngine.EffectPlayer->isPlaying() && AudioEngine.MuteForOneshot);
}

void MagistrateAudio::PlaySFX(SFXID sfx, bool AutoMute)
{
    if(!IsRegistered())
        return;

    if(!AudioEngine.SFXMap->contains(sfx))
        return;

    AudioEngine.MuteForOneshot = AutoMute;

    AudioEngine.EffectPlayer = AudioEngine.SFXMap->find(sfx).value();

    AudioEngine.EffectPlayer->setVolume(AudioEngine.Volume);
    AudioEngine.EffectPlayer->play();
}

void MagistrateAudio::IntroComplete()
{
    if(AudioEngine.SongPlayer == nullptr || AudioEngine.LoopPlayer == nullptr)
        return;

    if(AudioEngine.SongPlayer->isPlaying() || AudioEngine.AudioState.NoLoop)
        return;

    AudioEngine.LoopPlayer->play();
}

void MagistrateAudio::EffectPlayingUpdated()
{
    if(AudioEngine.EffectPlayer == nullptr)
        return;

    if(sender() != AudioEngine.EffectPlayer)
        return;

    if(AudioEngine.LoopPlayer != nullptr)
        AudioEngine.LoopPlayer->setMuted(AudioEngine.EffectPlayer->isPlaying() && AudioEngine.MuteForOneshot);

    if(AudioEngine.SongPlayer != nullptr)
        AudioEngine.SongPlayer->setMuted(AudioEngine.EffectPlayer->isPlaying() && AudioEngine.MuteForOneshot);
}
