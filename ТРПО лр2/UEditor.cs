using System;
using System.Collections.Generic;
using System.Text;

namespace Fractional_Calc
{
    // Класс "редактор"
    class TEditor
    {
        // Максимальная длина числителя и знаменателя в символах
        const int MAX_LENGTH = 9; 
        // Разделитель числителя и знамеенателя
        const string SEP = "\\";
        // Исходное представление строки
        const string ZERO_STR = "0";

        // Строковое представление редактируемой простой дроби
        string Str;

        // Операция №1, "читать строка"
        // возвращает значение Str
        private string ReadString()
        {
            return Str;
        }

        // Операция №2, "писать строка"
        // заносит полученное значение в Str
        private string WriteString(string value)
        {
            return Str = value;
        }

        // Констуктор класса
        public TEditor()
        {
            // Значение по умолчанию
            Str = ZERO_STR;
        }

        // Операция №3, "Дробь есть ноль"
        // Возвращает True, если Str содержит дробь 0\1
        private bool FractionIsZero()
        {
            return Str[0] == '0' || Str.Substring(0, 2) == "-0";
        }

        // Операция №4, "Добавить знак"
        // Добавляет или удаляет знак '-' из Str
        private string AddSign()
        {
            // Если есть минус, то удаляем его
            if (Str[0] == '-')
                Str = Str.Remove(0, 1);
            else
                // Если минуа не было, то добавляем его
                Str = Str.Insert(0, "-");
            return Str;
        }

        // Операция №5, "Добавить цифру"
        // Добавляет цифру в конец Str
        private string AddDigit(int digit)
        {
            // Проверка на лимит цифр в знаменателе или числителе (максимум 9 в числителе и знаменателе)
            if (!Str.Contains("\\") && Str.Length == MAX_LENGTH)
                return Str;
            else if (Str.Contains("\\"))
            {
                // Проверка длины знаменателя
                var split_lines = Str.Split('\\');
                if (split_lines[1].Length == MAX_LENGTH)
                    return Str; 
            }

            // Проверка, что переданный агумент является цифрой
            if (digit < 0 || digit > 9)
                throw new Exception("Error: invalid format");
            // Если строка была пустая
            if (Str == ZERO_STR)
                return Str = digit.ToString();
            else
                return Str += digit.ToString();
        }

        // Операция №6, "Добавить ноль"
        // добавляет ноль к Str
        private string AddZero()
        {
            // Проверка на лимит цифр в знаменателе или числителе (максимум 9 в числителе и знаменателе)
            if ((!Str.Contains("\\") && Str.Length == MAX_LENGTH) || (Str == ZERO_STR))
                return Str;
            if (Str.Contains("\\"))
            {
                // Проверка длины знаменателя
                var split_lines = Str.Split('\\');
                if (split_lines[1].Length == MAX_LENGTH)
                    return Str;
            }
            return Str += ZERO_STR;
        }

        // Операция №7, "добавить разделиитель"
        // Добавляет дробный разделитель
        private string AddSeparator()
        {
            // Елси разделитель уже есть в строка, то изменения не нужны
            if (!Str.Contains(SEP))
                Str += SEP;
            return Str;
        }

        // Операция №8, "Забой символа"
        // Удаляет крайный правый символ из Str
        private string Backspace()
        {
            // Если есть возможность удалить крайний правый символ
            if (Str.Length > 1)
                Str = Str.Remove(Str.Length - 1);
            else
                // Если строка полностью очистилась
                Str = ZERO_STR;
            // Если остался только минус
            if (Str == "-")
                Str = ZERO_STR;
            return Str;
        }

        // Операция №9, "Очистить"
        // Устаналвивает в Str строковое представлениие дроби 0\1
        private string Clear()
        {
            return Str = ZERO_STR;
        }

        // Команда "редактировать"
        // Функция получает номер команды редактирования
        // и выполняет необходимые действия
        // Операция принимает от 1 или 2 аргумента, в зависимости
        // от требуемой операции
        // @args[0] - номер команды, которую нужно выполнить
        // @args[1] - аргумент, используемый этой командой (если необходимо)
        public string DoEdit(params object[] args)
        {
            string result = System.String.Empty;
            // Проверка числа аргументов
            if (args.Length == 0 || args.Length > 2)
                throw new Exception("Error: invalid num of args");

            switch ((int)args[0])
            {
                // Операция №1, "читать строка"
                case 1:
                    result = ReadString();
                    break;
                // Операция №2, "писать строка"
                case 2:
                    result = WriteString(args[1].ToString());
                    break;
                // Операция №3, "Дробь есть ноль"
                case 3:
                    result = FractionIsZero().ToString();
                    break;
                // Операция №4, "Добавить знак"
                case 4:
                    result = AddSign();
                    break;
                // Операция №5, "Добавить цифру"
                case 5:
                    result = AddDigit((int)args[1]);
                    break;
                // Операция №6, "Добавить ноль"
                case 6:
                    result = AddZero();
                    break;
                // Операция №7, "добавить разделиитель"
                case 7:
                    result = AddSeparator();
                    break;
                // Операция №8, "Забой символа"
                case 8:
                    result = Backspace();
                    break;
                // Операция №9, "Очистить"
                case 9:
                    result = Clear();
                    break;
                default:
                    throw new Exception("Error: invalid num of operation");
            }
            return result;
        }

    }

}