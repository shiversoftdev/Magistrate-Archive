import soundfile as sf

import os

for file in os.listdir("snd"):
    if file.endswith(".wav"):
        f = sf.SoundFile(os.path.join("snd", file))
        print('"{}" {}'.format(file, int(len(f) / f.samplerate * 1000)))
