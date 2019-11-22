using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace encryption
{
    class PTG
    {
        // Ввод информации о полиноме из файла
        public static List<int> inputFunction(ref int size, string PATH)
        {
            // size - максимальная степень полинома, она же - размерность регистра
            // Индексы ненулевых коэффициентов массива
            List<int> indicesOfCoef = new List<int>();
            // Вспомогательная переменная
            int help = 0;
            string buf = System.String.Empty;

            using (var Sr = new StreamReader(PATH))
            {
                while ((buf = Sr.ReadLine()) != null)
                {
                    // Проверка на недопустимые символы
                    if (buf != "0" && buf != "1")
                        throw new Exception("Файл с данными о функции содержит недопустимые символы!");
                    help = int.Parse(buf);
                    // Сохраняем индексы ненулевых коэффициентов полинома
                    if (help != 0 && size != 0)
                        indicesOfCoef.Add(size - 1);
                    size++;
                }
                size--;
            }

            return indicesOfCoef;
        }

        // Функция генерации, задается стартовое значение и размер
        // сгенерированной последовательности
        public static void GenerationLFSR(string startValue, int power, List<int> indices)
        {
            // Создание регистра с начальным заданным значением
            int[] register = new int[startValue.Length];
            for (int i = 0; i < register.Length; i++)
                register[i] = startValue[i] - '0';

            // Основной цикл
            for (int k = 0; k < power; k++)
            {
                for (int i = 0; i < register.Length; i++)
                    Console.Write(register[i]);
                // Выводим значение последнего бита регистра
                Console.Write("   " + register[register.Length - 1] + "\n");
                // Вызов одной итерации LFSR
                OneIterationLFSR(ref register, indices);
            }
        }

        // Одна итерация LFSR
        public static int OneIterationLFSR(ref int[] register, List<int> indices)
        {
            int sum = 0;
            // Считаем новое значение 0-бита регистра
            for (int i = 0; i < indices.Count; i++)
                sum += register[indices[i]];
            //sum += register[register.Length - 1 - indices[i]];
            // Выполняем сдвиг вправо на один бит
            for (int i = register.Length - 1; i > 0; i--)
                register[i] = register[i - 1];
            // Устанавливаем новое значение первого бита
            register[0] = sum % 2;
            return register[register.Length - 1];
        }

        // Псевдослучайная генерация с прореживанием
        // Задается стартовое значение x0,
        // Длина сгенерированной последовательности power
        // индексы коэффициентов для элементарной и селектирующей последовательности
        public static string GenerationPT(string startValue_a, string startValue_s, List<int> indices_a, List<int> indices_s, int power, ref double Stat)
        {
            // Сгенерированная последовательность
            string result = System.String.Empty;

            // Счетчики единиц и нулей для оценивания распределения
            int zero_counter = 0, unit_counter = 0;
            // n - размер системы
            int n = power;

            // Создаем регистры для элементарной и селектирующей последовательности
            int[] register_a = new int[startValue_a.Length];
            int[] register_s = new int[startValue_s.Length];
            for (int i = 0; i < register_a.Length; i++)
                register_a[i] = startValue_a[i] - '0';

            for (int i = 0; i < register_s.Length; i++)
                register_s[i] = startValue_s[i] - '0';

            int a = register_a[register_a.Length - 1];
            int s = register_s[register_s.Length - 1];
            if (s == 1)
            {
                // Вывод последнего бита
                result += a.ToString();
                power--;
                if (a == 1)
                    unit_counter++;
                else
                    zero_counter++;
            }

            while (power > 0)
            {
                a = OneIterationLFSR(ref register_a, indices_a);
                s = OneIterationLFSR(ref register_s, indices_s);

                if (s == 1)
                {
                    // Вывод последнего бита
                    result += a.ToString();
                    power--;
                    if (a == 1)
                        unit_counter++;
                    else
                        zero_counter++;
                }
            }

            // Оцениваем вид распределения по критерию хи-квадрат
            Stat = n * 2 * (Math.Pow((float)unit_counter / n - (float)1 / 2, 2) + Math.Pow((float)zero_counter / n - (float)1 / 2, 2));
            //Console.WriteLine("S = {0}; t = {1}", S, 3.84);



            return result;
        }

        // Функция получения значения периода для заданной
        // комбинации исходных данных
        public static int PeriodOfSeries(string startValue, List<int> indices)
        {
            // По свойству, период не превышает данное число
            int limit = (int)Math.Pow(2, startValue.Length);
            // Период последовательности
            int T = 1;

            // Создаем два регистра
            int[] register_original = new int[startValue.Length];
            int[] register_side = new int[startValue.Length];
            for (int i = 0; i < register_original.Length; i++)
            {
                register_original[i] = startValue[i] - '0';
                register_side[i] = register_original[i];
            }

            for (int k = 0; k < limit; k++, T++)
            {
                OneIterationLFSR(ref register_side, indices);

                if (RegisterCompare(register_original, register_side))
                    return T;
            }

            // Ошибка
            return -1;
        }

        // Функция сравнения двух регистров, необходимо
        // для получение периода LFSR
        public static bool RegisterCompare(int[] register_original, int[] register_side)
        {
            // Переменные должны совпадать по размеру
            if (register_original.Length != register_side.Length)
                return false;

            for (int i = 0; i < register_original.Length; i++)
            {
                if (register_original[i] != register_side[i])
                    return false;
            }

            return true;
        }

        // Нахождение НОД x и y
        public static int gcd(int x, int y)
        {
            if (y == 0)
                return x;
            else
                return gcd(y, x % y);
        }

        // Проверка чисел на взаимную простоту
        public static bool MutualSimplicity(int x, int y)
        {
            return ((gcd(x, y) == 1) ? true : false);
        }

        // Нахождение периода генератора псевдослучаных числе с прореживанием
        public static int PeriodOfPTG(string startValue_a, string startValue_s, List<int> indices_a, List<int> indices_s)
        {
            // Период элементарной последовательности
            int period_a = PeriodOfSeries(startValue_a, indices_a);
            // Период селектирующей последовательности
            int period_s = PeriodOfSeries(startValue_s, indices_s);

            // Если взаимно простые числа, то можем найти период
            if (MutualSimplicity(period_a, period_s))
                return (int)((Math.Pow(2, startValue_a.Length) -1) * Math.Pow(2, startValue_s.Length - 1));
            else
                // Не являются взаимно простыми, период можно найти только вручную
                return -1;
        }
    }
}