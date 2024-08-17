#ifndef VM_CONST_H
#define VM_CONST_H

//#define IATR
//#define IMAGE_IS_WINDOWS
#define IMAGE_IS_X11


// Constant to xor the time ticks by
#define VM_TIMEXORCONST (unsigned long long)(0xFB7331FED4AC85AA)
#define VM_USES_TIME
#define VM_MAX_TIME_FORCED (unsigned long long)(21600)

// vm resource constants
#define VM_SND_TEST ":/vm/snd/iatr-pointgain.wav"
// icon for points notification when gaining points
#define VM_ICO_PointsGained ":/vm/ICON_PointsGained"
// icon for points notification when losing points
#define VM_ICO_PointsLost ":/vm/ICON_PointsLost"
// plural noun for points in this scenario
#define VM_TEXT_PointsAlias "gifts"
// plural noun for checks in this scenario
#define VM_TEXT_ChecksAlias "tasks"
// past tense verb for checks completed in this scenario. Capitalize the first letter.
#define VM_TEXT_EarnedVerb "Completed"

// vm scenario constants
// readme uri
#define VM_README_LOCATION "https://drive.google.com/file/d/13fEYdGQIx2aF-VjyU-C0gJBv5mVJa4q-/view?usp=sharing"

// progress counts for checks (music states and injects)
#define VM_PROG_1 0
#define VM_PROG_2 25
#define VM_PROG_3 999
#define VM_PROG_4 999
#define VM_PROG_5 999
#define VM_PROG_6 999
#define VM_PROG_7 999

// songs for the vm
#define VM_SONG_1_START "qrc:/vm/snd/iatr_ambience.wav"
#define VM_SONG_1_LOOP "qrc:/vm/snd/iatr_ambience.wav"
#define VM_SONG_2_START "qrc:/vm/snd/iatr_ambience.wav"
#define VM_SONG_2_LOOP "qrc:/vm/snd/iatr_ambience.wav"
#define VM_SONG_3_START "qrc:/vm/snd/sm64_ccmintro.wav"
#define VM_SONG_3_LOOP "qrc:/vm/snd/sm64_ccmloop.wav"
#define VM_SONG_4_START "qrc:/vm/snd/sm64_sslintro.wav"
#define VM_SONG_4_LOOP "qrc:/vm/snd/sm64_sslloop.wav"
#define VM_SONG_5_START "qrc:/vm/snd/sm64_hmcintro.wav"
#define VM_SONG_5_LOOP "qrc:/vm/snd/sm64_hmcloop.wav"
#define VM_SONG_6_START "qrc:/vm/snd/sm64_bfsintro.wav"
#define VM_SONG_6_LOOP "qrc:/vm/snd/sm64_bfsloop.wav"
#define VM_SONG_7_START "qrc:/vm/snd/sm64_aceintro.wav"
#define VM_SONG_7_LOOP "qrc:/vm/snd/sm64_aceloop.wav"

#define VM_SONG_SRP_START "qrc:/vm/snd/iatr_menus.wav"
#define VM_SONG_SRP_LOOP "qrc:/vm/snd/iatr_menus.wav"
#define VM_SONG_FRQ_START "qrc:/vm/snd/iatr_menus.wav"
#define VM_SONG_FRQ_LOOP "qrc:/vm/snd/iatr_menus.wav"

// song resources to use
#define VM_SONG_1
#define VM_SONG_2
//#define VM_SONG_3
//#define VM_SONG_4
//#define VM_SONG_5
//#define VM_SONG_6
//#define VM_SONG_7

// UI sounds for the vm
#define VM_UI_CLOSE "qrc:/vm/snd/iatr_select.wav"
#define VM_UI_CLICK "qrc:/vm/snd/iatr_select.wav"
#define VM_UI_WARN "qrc:/vm/snd/iatr-warning.wav"
#define VM_UI_SUCCESS "qrc:/vm/snd/iatr-uisuccess.wav"
#define VM_UI_POINTGAIN "qrc:/vm/snd/iatr-pointgain.wav"
#define VM_UI_POINTLOSS "qrc:/vm/snd/iatr-pointloss.wav"
#define VM_UI_OPEN_README "qrc:/vm/snd/iatr-readme.wav"
#define VM_UI_OPEN_SRP "qrc:/vm/snd/iatr-pointgain.wav"
#define VM_UI_OPEN_FRNSC "qrc:/vm/snd/iatr-pointgain.wav"

#endif // VM_CONST_H
