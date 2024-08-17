#ifndef CSSE_CONST_H
#define CSSE_CONST_H

#endif // CSSE_CONST_H
#include <stdint.h>

constexpr uint32_t fnv_base_32 = 0x811c9dc5;
constexpr uint32_t fnv_prime_32 = 0x1000193;

inline constexpr uint32_t fnv1a_const(const char* const str, const uint32_t value = fnv_base_32) noexcept
{
    return (str[0] == '\0') ? value : fnv1a_const(&str[1], (value ^ uint32_t(str[0])) * fnv_prime_32);
}
inline uint32_t fnv1a(const void* key, const uint32_t len) {

    const char* data = (char*)key;
    uint32_t hash = 0x811c9dc5;
    uint32_t prime = 0x1000193;

    for(uint32_t i = 0; i < len; ++i) {
        uint8_t value = data[i];
        hash = hash ^ value;
        hash *= prime;
    }

    return hash;

}

#define CSSE_MUTEX_SUFFIX "MUTEX"
#define CSSE_MUTEX_STR(mutex, inst) (std::string(inst ".") + std::string(mutex) + std::string(CSSE_MUTEX_SUFFIX))

// Filename of the client input command buffer from the engine
#define CSSE_PCI "PCI"

// Filename of the client output command buffer to the engine
#define CSSE_PCO "PCO"

// Filename of the score information emitted by the engine
#define CSSE_PGS "PGS"

// Filename of the temporary state data for this vm instance
#define CSSE_PTS "PTS"

// Filename of the client output buffer for forensics question answers
#define CSSE_PCF "CFQ"
#define CSSE_FQNAME(FQID) (std::to_string(FQID) + std::string("." CSSE_PCF))

// player prefs
#define CSSE_PREFS "PREFS"

#define CSSE_COM_CLIENT "1"
#define CSSE_COM_ENGINE "0"

// string value to append when concatenating csse answers
#define CSSE_FQ_CONCATSTR "&answer="

// Time in ms to wait between checking the frame input buffer
#define CSSE_COM_TICK 5000

// Highest difference between the current COM_FRAME and the received input buffer's minimum command frame before
// applying a resync procedure
#define CSSE_COM_MAXFRAMEDELTA 3

// Largest acceptable size of the command queue before dropping current queue
#define CSSE_COM_MAXQUEUE 64

// Maximum size of a cmd token (args, etc)
#define CSSE_COM_CMDMAXTOKSIZE 255
#define CSSE_COM_CMDMAXDIGITS 10
#define CSSE_COM_CMDMAXARGS 8

// Command for notifying a client database update
#define CSSE_CMD_NotifyEvent fnv1a_const("CMD_NotifyEvent")
#define CSSE_CMD_ForensicsParse fnv1a_const("CMD_ForensicsParse")

#define CMD_SwitchCase(c, f, argc, argv) case c: f(argc, argv); break;


enum CSSE_ClientCommands
{
    ParseScores = 0,
    NotifyEvent = 1
};

//DEPLOY https://stackoverflow.com/questions/28732602/qt-example-executables-wont-run-missing-qt5cored-dll
//https://doc.qt.io/Qt-5/windows-deployment.html
