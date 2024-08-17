#pragma once

#define SIZEOF_SCOREMSG 256
#define SIZEOF_CI2 16
#define SIZEOF_ANSNAMETEXT 128
#define SIZEOF_ANSFORMATSTR 128
#define SIZEOF_QRICHTEXT 4096

#include <QWidget>

enum ForensicsAnswerType : uint32_t
{
    BoolValue = 0,
    IntValue = 1,
    FloatValue = 2,
    SingleLineFormatted = 3
};

struct ForensicsAnswer
{
    ForensicsAnswerType AnswerType;
    uint32_t Pad;
    uint64_t Pad2;
    char NameText[SIZEOF_ANSNAMETEXT];
    char FormatString[SIZEOF_ANSFORMATSTR];
};

struct ForensicsQuestion
{
    uint32_t NumAnswers;
    ForensicsAnswer* Answers;
    uint32_t QuestionID;
    uint32_t Pad;
    char QuestionRichText[SIZEOF_QRICHTEXT];
    //answers here
};

struct ForensicsData
{
    uint32_t NumQuestions;
    ForensicsQuestion* Questions;
    uint32_t BufferSize;
    uint32_t Pad;
    //questions here
};


struct ScoreEntry
{
    uint32_t ID; //Vulnerability ID
    int32_t ScoreValue; //Score Value
    const char* Description; //Check passed, etc.
    const char* CI2; //check integrity identifier

    const char ScoreMessage[SIZEOF_SCOREMSG];
    const char CI2Str[SIZEOF_CI2];
};

struct ScoringData
{
    uint64_t Magic; //Scoring Magic
    uint64_t TimeStartUnix; //Unix image start time

    uint32_t NumChecksPassed; //Number of checks passed
    uint32_t NumChecksFailed; //Number of checks failed
    uint32_t MaxChecksPassed; //Number of total positive scoring items
    uint32_t Pad;//Extra padding

    uint32_t SI2Off;
    uint32_t ScoreEntryTableOff;
    uint32_t ForensicsDataOff;
    uint32_t Pad2;//Extra padding

    const char* SI2; //Scoring integrity identifier
    ScoreEntry* ScoreEntryTable;
    ForensicsData* Forensics;
    uint32_t Pad3;//Extra padding

    //scoreentrytable
    //ForensicsData
    //SI2
};

struct ServerCommand
{
    uint ServerFrame; //Execute this command if the client frame is greater than equal to this frame.
    uint CommandID; //Command ID (internal use)
    char NumArgs; //Number of arguments for this command
    std::list<std::string*>* Args; //Arguments
};

struct ClientPrefs
{
    bool UseSimpleFont;
    uint32_t FontScalar;
    uint32_t MusicVolume;
    uint32_t SFXVolume;
};

struct PTS_s
{
    uint64_t TimePlayedCrypt;
    uint64_t MaxTimeCrypt;
};
