#include <iostream>
#include <windows.h>

// Спецификатор volatile нужен, чтобы несколько потоков могли обращаться к этой переменной
volatile int n = 0; 

// Функция потока
DWORD WINAPI Add(LPVOID iNum)
{
	std::cout << "Thread is started" << std::endl;
	n += *static_cast<int*>(iNum);
	std::cout << "Thread is finished" << std::endl;
	return 123;
}

int main()
{
	int inc = 10;
	HANDLE hThread; // Дескриптор потока
	DWORD IDThread; // Идентификатор потока
	std::cout << "Old n: " << n << std::endl;

	// Запуск потока
	hThread = CreateThread(NULL, 0, Add, &inc, 0, &IDThread);

	// Ожидание завершения потока
	WaitForSingleObject(hThread, INFINITE);

	std::cout << "New n: " << n << std::endl;


	// Получить код заврешенеия потока
	DWORD code;
	GetExitCodeThread(hThread, &code);
	std::cout << "Thread was finished with code: " << code << std::endl;

	return 0;
}