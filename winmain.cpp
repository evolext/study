#include <windows.h>
#include <string>

// ��������� ����������
std::string Info;

// ��������������� ���������
std::string FRST_MSG = "Screen height (in pixels): ";

// �������-���������� ���������
LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

// ��������, �������������� � ��������� ������
DWORD WINAPI getScreenHeight(LPVOID);

int WINAPI WinMain(HINSTANCE hThisInst, HINSTANCE hPrevInst, LPSTR str, int nWinMode)
{
    // ��� ������ ����
    LPCWSTR szClassName = L"Sample Window Class";
    // ����� ����
    WNDCLASS wcl = { };
    // ���������� ����
    HWND hWnd;
    // ��������� ���������
    MSG msg = { };

    // *** ������������� ����� ������ ���� ***
    // ����������� �������-����������� ���������
    wcl.lpfnWndProc   = WindowProc;
    // ���������� ����������, ���������� ����� ����
    wcl.hInstance     = hThisInst;
    // ��� ������ ����
    wcl.lpszClassName = szClassName;
    // ����� ����
    wcl.style = CS_HREDRAW | CS_VREDRAW;
    // ����������� ����
    wcl.hIcon = LoadIcon(NULL, IDI_APPLICATION);
    // ������ ����
    wcl.hCursor = LoadCursor(NULL, IDC_ARROW);
    // ����: �����������
    wcl.lpszMenuName = NULL;
    wcl.cbClsExtra = 0;
    wcl.cbWndExtra = 0;
    // ���� ������� ���� ����
    wcl.hbrBackground = (HBRUSH)(COLOR_GRADIENTINACTIVECAPTION);

    // ����������� ������ ����
    RegisterClass(&wcl);

    // *** �������� � ����������� ���� ***

    hWnd = CreateWindowEx(
        0,                               
        szClassName,                                   // ��� ������ ����
        L"��������� ���������� �������������� ������", // ��������� ����
        WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX | WS_MAXIMIZEBOX, // ����� ����, ������ ���� �������������� �������
        // ������� ����
        100, 50,
        // ������ ����
        640, 360,
        NULL,       // ������������� ����: ���  
        NULL,       // ����: �����������
        hThisInst,  // ���������� ����������
        NULL        // �������������� ���������
     );

    if (hWnd == NULL)
    {
        return -1;
    }

    // ����������� � ���������� ����
    ShowWindow(hWnd, nWinMode);
    UpdateWindow(hWnd);

    // ���� ��������� ���������
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return msg.wParam;
}

LRESULT CALLBACK WindowProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    PAINTSTRUCT ps;
    HDC hDC;
    HFONT hFont, hOldFont;

    // ������������� ������
    DWORD IDThread;
    HANDLE hThread;

    switch (msg)
    {
        case WM_CREATE:

            // ������� �������� �����, � ������ ����� ������ ��������� �������
            hThread = CreateThread(NULL, 0, &getScreenHeight, NULL, 0, &IDThread);
            // �������� ��������� ��������� ������
            WaitForSingleObject(hThread, INFINITE);
            // �������� ����������� ������
            CloseHandle(hThread);
            // ��������� �������� � ����� ��������� � �����������
            FRST_MSG.append(Info);
            break;

        case WM_DESTROY:
            PostQuitMessage(0);
            break;

        case WM_PAINT:
            hDC = BeginPaint(hWnd, &ps);
            // ������� ����� ����� 
            hFont = CreateFont(28, // ������ ������
                               13, // ������� ������ �������
                                0, // ���� �������
                                0, // ���� ���������� �������� �����
                                FW_EXTRALIGHT, // ������� ������: 200
                                FALSE,         // ������: ���
                                TRUE,          // ������������: ��
                                FALSE,         // �����������: ���
                                DEFAULT_CHARSET, 
                                OUT_OUTLINE_PRECIS,
                                CLIP_DEFAULT_PRECIS,
                                ANTIALIASED_QUALITY,
                                VARIABLE_PITCH,
                                TEXT("Verdana") // ��� ������         
                );
            // ��������� ������
            SelectObject(hDC, hFont);
            // ��������� ������� ���� ���������� ������
            SetBkColor(hDC, GetSysColor(COLOR_GRADIENTACTIVECAPTION));
            // ��� ������� ���������
            TextOut(hDC, 10, 10, LPCWSTR((std::wstring(FRST_MSG.begin(), FRST_MSG.end())).c_str()), FRST_MSG.length());
            // �������� ���������� ������
            DeleteObject(hFont);
            EndPaint(hWnd, &ps);
            break;
        default:
            return DefWindowProc(hWnd, msg, wParam, lParam);

    }
    return 0;
}

DWORD WINAPI getScreenHeight(LPVOID t)
{
    // �������� ��������� ������ ������
    Info = std::to_string(GetSystemMetrics(SM_CYSCREEN));
    return 0;
}