// main.cpp
#include <iostream>
#include <string>

extern "C" int __fastcall PROCESSING(const char* str, int N);

int main() 
{
    std::string input_buf; // ����� ��� ������, ������� ����� ���������
    int count = 0;         // ���������� ����������

    // ������� ������
    std::cout << "Enter string to process" << std::endl;
    std::getline(std::cin, input_buf);
    std::cout << "Repeat time: ";
    std::cin >> count;

    // ��������� ���������
    PROCESSING(input_buf.c_str(), count);

    // ��������� ������ ���������
    std::cout << "Modified string: " << std::endl << input_buf.c_str() << std::endl;
    return 0;
}