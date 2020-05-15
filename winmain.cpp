#include <windows.h>
#include <string>

// Результат выполнения
std::string Info;

// Диагностическое сообщение
std::string FRST_MSG = "Screen height (in pixels): ";

// Функция-обработчик сообщений
LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

// Функциия, выыполняющаяся в отдельном потоке
DWORD WINAPI getScreenHeight(LPVOID);

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
    wcl.lpfnWndProc   = WindowProc;
    // Дескриптор приложения, создающего класс окна
    wcl.hInstance     = hThisInst;
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
        100, 50,
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
        case WM_CREATE:

            // Создаем дочерний поток, в рамках этого потока выполняем функцию
            hThread = CreateThread(NULL, 0, &getScreenHeight, NULL, 0, &IDThread);
            // Ожидание окончания дочернего потока
            WaitForSingleObject(hThread, INFINITE);
            // Удлаение дескриптора потока
            CloseHandle(hThread);
            // Сохраняем значение в текст сообщения с результатом
            FRST_MSG.append(Info);
            break;

        case WM_DESTROY:
            PostQuitMessage(0);
            break;

        case WM_PAINT:
            hDC = BeginPaint(hWnd, &ps);
            // Создаем новый шрифт 
            hFont = CreateFont(28, // Высота шрифта
                               13, // Средняя ширина символа
                                0, // Угол наклона
                                0, // Угол ориентации базисной линии
                                FW_EXTRALIGHT, // Толщина шрифта: 200
                                FALSE,         // Курсив: нет
                                TRUE,          // Подчеркнутый: да
                                FALSE,         // Зачеркнутый: нет
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
            // Тут выводим результат
            TextOut(hDC, 10, 10, LPCWSTR((std::wstring(FRST_MSG.begin(), FRST_MSG.end())).c_str()), FRST_MSG.length());
            // Удаление созданного шрифта
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
    // Получаем значениие высоты экрана
    Info = std::to_string(GetSystemMetrics(SM_CYSCREEN));
    return 0;
}