#include <windows.h>
#include <string>

// Результат выполнения
char Info[256];

// Диагностическиие сообщения
std::string FRST_MSG = "Screen height (in pixels): ";
std::string SCND_MSG;

// Функция-обработчик сообщений
LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

// Функциия, выыполняющаяся в отдельном потоке
DWORD WINAPI ThreadFunc(LPVOID);


int WINAPI WinMain(HINSTANCE hThisInst, HINSTANCE hPrevInst, LPSTR str, int nWinMode)
{
	// Имя класса окна
	LPCWSTR szClassName = L"Sample Window Class";
	// Класс окна
	WNDCLASS wcl = { };
	// Дескриптор окна
	HWND hWnd;
	// Структуры сообщений
	MSG msg = { };

	// *** Инициализация полей класса окна ***
	// Назначениие функции-обработчика сообщений
	wcl.lpfnWndProc = WindowProc;
	// Дескриптор приложения, создающего класс окна
	wcl.hInstance = hThisInst;
	// Имя класса окна
	wcl.lpszClassName = szClassName;
	// Стиль окна
	wcl.style = CS_HREDRAW | CS_VREDRAW;
	// Пиктограмма окна
	wcl.hIcon = LoadIcon(NULL, IDI_APPLICATION);
	// Крусор окна
	wcl.hCursor = LoadCursor(NULL, IDC_ARROW);
	// Меню: отсутствует
	wcl.lpszMenuName = NULL;
	wcl.cbClsExtra = 0;
	wcl.cbWndExtra = 0;
	// Цвет заднего фона окна
	wcl.hbrBackground = (HBRUSH)(COLOR_GRADIENTINACTIVECAPTION);

	// Регистрация класса окна
	RegisterClass(&wcl);

	// *** Создание и отображение окна ***

	hWnd = CreateWindowEx(
		0,
		szClassName,                                   // Имя класса окна
		L"Результат выполнения индивидуальной работы", // Заголовок окна
		WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX | WS_MAXIMIZEBOX, // Стили окна, делаем окно фиксированного размера
		// Позиция окна
		400, 300,
		// Размер окна
		640, 360,
		NULL,       // Родительского окна: нет  
		NULL,       // Меню: отсутствует
		hThisInst,  // Дескриптор приложения
		NULL        // Дополнительные параметры
	);

	if (hWnd == NULL)
	{
		return -1;
	}

	// Отображение и обновление окна
	ShowWindow(hWnd, nWinMode);
	UpdateWindow(hWnd);

	// Цикл обработки сообщений
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

	// Идентификатор потока
	DWORD IDThread;
	HANDLE hThread;

	switch (msg)
	{
		// При создании окна
		case WM_CREATE:
			// Создаем дочерний поток, в рамках этого потока выполняем функцию
			hThread = CreateThread(NULL, 0, &ThreadFunc, NULL, 0, &IDThread);
			// Ожидание окончания дочернего потока
			WaitForSingleObject(hThread, INFINITE);
			// Удлаение дескриптора потока
			CloseHandle(hThread);
			break;

		// Закрытие окна
		case WM_DESTROY:
			PostQuitMessage(0);
			break;

		// При перерисовке окна
		case WM_PAINT:
			hDC = BeginPaint(hWnd, &ps);
			// Создаем новый шрифт 
			hFont = CreateFont(
				28,				// Высота шрифта
				13,				// Средняя ширина символа
				0,				// Угол наклона
				0,				// Угол ориентации базисной линии
				FW_EXTRALIGHT,	// Толщина шрифта: 200
				FALSE,			// Курсив: нет
				TRUE,			// Подчеркнутый: да
				FALSE,			// Зачеркнутый: нет
				DEFAULT_CHARSET,
				OUT_OUTLINE_PRECIS,
				CLIP_DEFAULT_PRECIS,
				ANTIALIASED_QUALITY,
				VARIABLE_PITCH,
				TEXT("Verdana") // Имя шрифта         
			);
			// Установка шрифта
			SelectObject(hDC, hFont);
			// Установка заднего фона выводимого текста
			SetBkColor(hDC, GetSysColor(COLOR_GRADIENTACTIVECAPTION));
			// Выводим результат, первое и второе диагностические сообщения
			TextOut(hDC, 10, 10, LPCWSTR((std::wstring(FRST_MSG.begin(), FRST_MSG.end())).c_str()), FRST_MSG.length());
			TextOut(hDC, 10, 60, LPCWSTR((std::wstring(SCND_MSG.begin(), SCND_MSG.end())).c_str()), SCND_MSG.length());
			// Удаление созданного шрифта
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
	// Определяем типы указателей на нужные функции
	typedef int(*firstImportFunction)(char*);
	typedef int(*secondImportFunction)(void);

	// Загрузка динамической библиотеки
	HINSTANCE hinstLib = LoadLibrary(TEXT("MyLibr.dll"));


	// Загрузка нужной функции при помощи дескриптора библиотеки
	firstImportFunction screenHeight = (firstImportFunction)GetProcAddress(hinstLib, "getScreenHeight");
	// Загрузка функции с ассемблерной вставкой
	secondImportFunction hasSSE3 = (secondImportFunction)GetProcAddress(hinstLib, "hasPrescottInstruction");

	// if screenHeight != null
	// Вызов библиотечной функции
	int res = screenHeight(Info);
	// Сохраняем значение в текст сообщения с результатом
	FRST_MSG.append(Info);

	// if hasSSE3 != null
	// Вызов функции с ассемблерной вставкой
	int flag = hasSSE3();
	if (flag == 1)
		SCND_MSG = "SSE3 technology support: YES";
	else if (flag == 0)
		SCND_MSG = "SSE3 technology support: NO";
	else
		SCND_MSG = "CPUID instruction not supported by processor";

	// Закрытие динамической библиотеки
	FreeLibrary(hinstLib);
	return 0;
}