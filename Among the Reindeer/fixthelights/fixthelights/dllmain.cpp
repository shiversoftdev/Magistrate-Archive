#include "framework.h"
#include <stdio.h>
#include "xorstr.h"
#include <conio.h>

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

extern "C" void __declspec(dllexport) CALLBACK
SECRETPASSWORD(HWND hwnd, HINSTANCE hinst, LPSTR lpszCmdLine, int nCmdShow)
{
    AllocConsole();
    SetConsoleTitle("Welcome to the club!");
    freopen("CONOUT$", "w", stdout);
    setvbuf(stdout, NULL, _IONBF, 0);
    printf(xorstr("\n\n\nInstructions to fix lighting:\n\n\n"));
    printf(xorstr("\t1. Create a file at C:\\Windows\\flashlight.txt \n\n"));
    printf(xorstr("\t2. In the file, there should be a single line of text containing this password:\n\n"));
    printf(xorstr("\t\tShiegra123!\n\n\n"));
    printf(xorstr("Press any key to continue\n"));
    getch();
}