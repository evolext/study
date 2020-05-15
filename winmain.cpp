#include <windows.h>
#include <string>

// ��������� ����������
char Info[256];

// ���������������� ���������
std::string FRST_MSG = "Screen height (in pixels): ";
std::string SCND_MSG;

// �������-���������� ���������
LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

// ��������, �������������� � ��������� ������
DWORD WINAPI ThreadFunc(LPVOID);


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
	wcl.lpfnWndProc = WindowProc;
	// ���������� ����������, ���������� ����� ����
	wcl.hInstance = hThisInst;
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
		400, 300,
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
		// ��� �������� ����
		case WM_CREATE:
			// ������� �������� �����, � ������ ����� ������ ��������� �������
			hThread = CreateThread(NULL, 0, &ThreadFunc, NULL, 0, &IDThread);
			// �������� ��������� ��������� ������
			WaitForSingleObject(hThread, INFINITE);
			// �������� ����������� ������
			CloseHandle(hThread);
			break;

		// �������� ����
		case WM_DESTROY:
			PostQuitMessage(0);
			break;

		// ��� ����������� ����
		case WM_PAINT:
			hDC = BeginPaint(hWnd, &ps);
			// ������� ����� ����� 
			hFont = CreateFont(
				28,				// ������ ������
				13,				// ������� ������ �������
				0,				// ���� �������
				0,				// ���� ���������� �������� �����
				FW_EXTRALIGHT,	// ������� ������: 200
				FALSE,			// ������: ���
				TRUE,			// ������������: ��
				FALSE,			// �����������: ���
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
			// ������� ���������, ������ � ������ ��������������� ���������
			TextOut(hDC, 10, 10, LPCWSTR((std::wstring(FRST_MSG.begin(), FRST_MSG.end())).c_str()), FRST_MSG.length());
			TextOut(hDC, 10, 60, LPCWSTR((std::wstring(SCND_MSG.begin(), SCND_MSG.end())).c_str()), SCND_MSG.length());
			// �������� ���������� ������
			DeleteObject(hFont);
			EndPaint(hWnd, &ps);
			break;
		default:
			return DefWindowProc(hWnd, msg, wParam, lParam);

	}
	return 0;
}

DWORD WINAPI ThreadFunc(LPVOID t)
{
	// ���������� ���� ���������� �� ������ �������
	typedef int(*firstImportFunction)(char*);
	typedef int(*secondImportFunction)(void);

	// �������� ������������ ����������
	HINSTANCE hinstLib = LoadLibrary(TEXT("MyLibr.dll"));


	// �������� ������ ������� ��� ������ ����������� ����������
	firstImportFunction screenHeight = (firstImportFunction)GetProcAddress(hinstLib, "getScreenHeight");
	// �������� ������� � ������������ ��������
	secondImportFunction hasSSE3 = (secondImportFunction)GetProcAddress(hinstLib, "hasPrescottInstruction");

	// if screenHeight != null
	// ����� ������������ �������
	int res = screenHeight(Info);
	// ��������� �������� � ����� ��������� � �����������
	FRST_MSG.append(Info);

	// if hasSSE3 != null
	// ����� ������� � ������������ ��������
	int flag = hasSSE3();
	if (flag == 1)
		SCND_MSG = "SSE3 technology support: YES";
	else if (flag == 0)
		SCND_MSG = "SSE3 technology support: NO";
	else
		SCND_MSG = "CPUID instruction not supported by processor";

	// �������� ������������ ����������
	FreeLibrary(hinstLib);
	return 0;
}