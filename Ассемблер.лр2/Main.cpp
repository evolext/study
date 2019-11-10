// main.cpp
#include <iostream>
#include <string>

extern "C" int __fastcall PROCESSING(const char* str, int N);

int main() 
{
    std::string input_buf; // Буфер для строки, которую нужно повторять
    int count = 0;         // Количество повторений

    // Входные данные
    std::cout << "Enter string to process" << std::endl;
    std::getline(std::cin, input_buf);
    std::cout << "Repeat time: ";
    std::cin >> count;

    // Процедура обработки
    PROCESSING(input_buf.c_str(), count);

    // Результат работы программы
    std::cout << "Modified string: " << std::endl << input_buf.c_str() << std::endl;
    return 0;
}