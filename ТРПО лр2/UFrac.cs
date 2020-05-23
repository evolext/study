using System;
using System.Collections.Generic;
using System.Text;

namespace Fractional_Calc
{
    // Класс, описывающий тип данных "Простая дробь"
    public class TFrac
    {
        // Числитель
        int Numerator;
        // Знаменатель
        int Denominator;

        // Нахождение НОД двух чисел
        int gcd(int x, int y)
        {
            while (y != 0)
                y = x % (x = y);
            return x;
        }

        // Первый конструктор, принимает на вход два целых числа
        // @a - числитель
        // @b - знаменатель
        public TFrac(int a, int b)
        {
            // Знаменатель не может быть равен нулю
            if (b == 0)
                throw new DivideByZeroException();

            // Для начала определим, нужно ли сокращать дробь,
            // найдем для этого НОД двух заданных чисел
            int gcd_result = gcd(Math.Abs(a), Math.Abs(b));

            // Сокращаем дробь, если возможно
            if (gcd_result > 1)
            {
                a /= gcd_result;
                b /= gcd_result;
            }

            // Инициализация полей класса
            Numerator = a;
            Denominator = b;
        }


        // Второй конструктор, принимает на вход строковое 
        // представление дроби в виде: 'a\b'
        // @a - числитель
        // @b - знаменатель
        public TFrac(string frac)
        {
            int a = 0, b = 1;

            // Если строка состоит только из числителя
            if (!frac.Contains("\\"))
            {
                if (!Int32.TryParse(frac, out a))
                    throw new Exception("Error: Unformatted string specified");
            }
            else
            {
                // Если строка содержит разделитель целой и дробной части
                var split_lines = frac.Split('\\');

                // Если строка с дробью не содержит знаменателя
                if (split_lines[1].Length == 0)
                {
                    if (!Int32.TryParse(split_lines[0], out a))
                        throw new Exception("Error: Unformatted string specified");
                }
                // Вычленяем значения числителя и знаменателя из строки
                else if (!Int32.TryParse(split_lines[0], out a) || !Int32.TryParse(split_lines[1], out b))
                    throw new Exception("Error: Unformatted string specified");
            }


            // Определим, можно ли сократить дробь,
            // найдем для этого НОД двух заданных чисел
            int gcd_result = gcd(Math.Abs(a), Math.Abs(b));

            // Сокращаем дробь, если возможно
            if (gcd_result > 1)
            {
                a /= gcd_result;
                b /= gcd_result;
            }

            // Инициализация полей класса
            Numerator = a;
            Denominator = b;
        }

        // Создает объект TFrac с таким же числителем и
        // знаменателем как у самой дроби
        public TFrac Copy()
        {
            return new TFrac(Numerator, Denominator);
        }

        // Операция "сложить"
        // Создает и возвращает дробь, которая
        // является результатом сложения входной и самой дроби
        public static TFrac operator +(TFrac x, TFrac y)
        {
            int new_numerator, new_denominator;
            // Проверка на переполнение
            try
            {
                checked
                {
                    new_numerator = x.Numerator * y.Denominator + y.Numerator * x.Denominator;
                    new_denominator = x.Denominator * y.Denominator;
                }
                return new TFrac(new_numerator, new_denominator);
            }
            catch (OverflowException) { throw new OverflowException(); }
            // При делении на ноль
            catch (DivideByZeroException) { throw new DivideByZeroException(); }   
        }

        // Операция "умножить"
        // Создает и возващает простую дробь, которая
        // является результатом умножения входной и исходной дроби
        public static TFrac operator *(TFrac x, TFrac y)
        {
            int new_numerator, new_denominator;
            // Проверка на переполнение
            try
            {
                checked
                {
                    new_numerator = x.Numerator * y.Numerator;
                    new_denominator = x.Denominator * y.Denominator;
                }
                return new TFrac(x.Numerator* y.Numerator, x.Denominator* y.Denominator);
            }
            catch (OverflowException) { throw new OverflowException(); }
            // При делении на ноль
            catch (DivideByZeroException) { throw new DivideByZeroException(); }
        }

        // Операция "Вычесть"
        // Создает и возвращает простую дробь, которая
        // получается в результате вычетания входной дроби из исходной
        public static TFrac operator -(TFrac x, TFrac y)
        {
            int new_numerator, new_denominator;
            // Проверка на переполнение
            try
            {
                checked
                {
                    new_numerator = x.Numerator * y.Denominator - y.Numerator * x.Denominator;
                    new_denominator = x.Denominator * y.Denominator;
                }
                return new TFrac(new_numerator, new_denominator);
            }
            catch (OverflowException) { throw new OverflowException(); }
            // При делении на ноль
            catch (DivideByZeroException) { throw new DivideByZeroException(); }
        }

        // Операция "Делить"
        // Создает и возващает постую дробь, которая
        // является результатом деления исходной дроби на входную, 
        // при условии, если числитель входной не равен нулю
        public static TFrac operator /(TFrac x, TFrac y)
        {
            // При попытке поделить на ноль
            if (y.Numerator == 0)
                throw new DivideByZeroException("Числитель второй дроби равен нулю");
            int new_numerator, new_denominator;
            // Проверка на переполнение
            try
            {
                checked
                {
                    new_numerator = x.Numerator * y.Denominator;
                    new_denominator = x.Denominator * y.Numerator;
                }
                return new TFrac(new_numerator, new_denominator);
            }
            catch (OverflowException) { throw new OverflowException(); }
            // При делении на ноль
            catch (DivideByZeroException) { throw new DivideByZeroException(); }
        }

        // Операция возведения в квадрат исходной дроби
        public TFrac Square()
        {
            int new_numerator, new_denominator;
            // Проверка на переполнение
            try
            {
                checked
                {
                    new_numerator = Numerator * Numerator;
                    new_denominator = Denominator * Denominator;
                }
                return new TFrac(new_numerator, new_denominator);
            }
            catch (OverflowException) { throw new OverflowException(); }
            // При делении на ноль
            catch (DivideByZeroException) { throw new DivideByZeroException(); }

        }

        // Операция "Обратное"
        // Создает и возвращает дробь, обратную исходной
        public TFrac Reverse()
        {
            if (Numerator < 0)
                return new TFrac(-Denominator, Math.Abs(Numerator));
            else
                return new TFrac(Denominator, Numerator);

        }

        // Операция "Минус"
        // Создает и возващает простую дробь, котоая
        // является результатом вычитания исходной дроби из дроби 0\1
        public static TFrac operator -(TFrac a)
        {
            return new TFrac(0, 1) - a;
        }

        // Операция "равно"
        // Сравнивает исходную дробь и входную
        public bool IsEqual(TFrac a)
        {
            return a.Numerator == Numerator && a.Denominator == Denominator;
        }

        // Операция "Больше"
        // Сравнивает значения исходной и входной дробей,
        // взвращает true, если исходная больше входной
        public bool IsLarge(TFrac a)
        {
            return Numerator * a.Denominator > a.Numerator * Denominator;
        }

        // Операция "Взять числитель число"
        // Возвращает значение числителя дроби в цифровом формате
        public int GetNumeratorNumber()
        {
            return Numerator;
        }

        // Операция "Взять знаменатель число"
        // Возвращает значение знаменателя дроби в цифровом формате
        public int GetDenominatorNumber()
        {
            return Denominator;
        }

        // Операция "Взять числитель строка"
        // Возвращает значение числителя дроби в строковом формате
        public string GetNumeratorString()
        {
            return Numerator.ToString();
        }

        // Операция "Взять знаменатель строка"
        // Возвращает значение знаменателя дроби в строковом формате
        public string GetDenominatorString()
        {
            return Denominator.ToString();
        }

        // Операция "Взять дробь строка"
        // Возвращает дробь в строковом представлении
        public string GetFractionString()
        {
            return GetNumeratorString() + '\\' + GetDenominatorString();
        }
    }

}