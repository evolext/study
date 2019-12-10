using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace encryption
{
    class PollardMethod
    {
        // Константа c: НОД(c,n)=1
        public int c { get; set; }
        // Массив значений M(k)
        // так как для остальных k очень большие значения, и мы можем
        // использовать только k = {3, 4, 5, 6}, то M(k) можем найти зраанее, при создании класса
        public int[] M { get; set; }
        // список простых чисел до 200
        public int[] SimpleNumbers { get; set; }

        // конструктор класса
        public PollardMethod()
        {
            this.c = 2;
            // считаем M(k)
            this.M = new int[6];
            // M(3) = 2
            this.M[0] = 2;
            // M(4) = 2 * 3 = 6
            this.M[1] = 6;
            // M(5) = 2 * 3  =6
            this.M[2] = 6;
            // M(5) = 4 * 5 = 12
            this.M[3] = 12;
            // M(6) = 2 * 3 * 5 = 30
            this.M[4] = 30;
            // M(6) = 2 * 2 * 3 * 5 = 60
            this.M[5] = 60;

            SimpleNumbers = new int[168];
            // Ввод значений простых чисел
            using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр7(графическое)\Криптография.лр7(графическое)\SimpleNumbers.txt"))
            {
                for (int i = 0; i < SimpleNumbers.Length; i++)
                    SimpleNumbers[i] = int.Parse(Sr.ReadLine());
            }
        }

        // Основной фрагмент программы
        public void procedure(int n, ref List<int> factors, ref int iterations)
        {
            // Проверка на простоту входного числа
            if (isSimpleNumber(n))
                throw new Exception("SimpleNumber");
            // Если заданное число - составное
            else
            {
                // Вспомогательные переменные
                int i = 0;
                ulong m = 0;
                int d = 0;
                // Флаг внутреннего цикла
                bool flagOfInnerLoop = true;
                // Флаг внешнего цикла
                bool flagOfOuterLoop = true;

                // Внешний цикл - факторизация числа
                do
                {
                    // Внутренний цикл - нахождение простого делителя
                    for (i = 0; i < this.M.Length && flagOfInnerLoop; i++)
                    {
                        // Проверка на то, является ли число n степнью двойки
                        if (isPowerOf2((int)n))
                        {
                            d = 2;
                            flagOfInnerLoop = false;
                        }
                        else
                        {
                            // Находим m(i)
                            m = find_m(i, n);
                            // находим d
                            d = find_d((int)m, n);
                            // Проверяем d
                            if (d == 1 || d == n)
                                i++;
                            else
                                flagOfInnerLoop = false;
                        }
                    }

                    if (flagOfInnerLoop)
                        throw new Exception("Impossible");
                    else
                    {
                        n /= d;
                        addToList(ref factors, d);
                        if (isSimpleNumber(n))
                        {
                            flagOfOuterLoop = false;
                            addToList(ref factors, n);
                        }
                        else
                            flagOfInnerLoop = true;
                    }
                    // Считаем количество итераций цикла
                    iterations++;
                } while (flagOfOuterLoop);

            }
        }

        // ответ на вопрос: "является ли число n простым?"
        public bool isSimpleNumber(int n)
        {
            for (int i = 0; i < this.SimpleNumbers.Length; i++)
            {
                if (n == SimpleNumbers[i])
                    return true;
            }
            return false;
        }

        // Проверка на степень двойки
        public bool isPowerOf2(int n)
        {
            if (n > 1)
                return (double)((int)Math.Log(n, 2)) == Math.Log(n, 2);
            else
                return false;
        }

        // Проверка на принадлежность элемента списку, если -1 - то не принадлжеит
        // если принадлежит, то возвращаем инедкс вхождения
        public void addToList(ref List<int> L, int elem)
        {
            int iter = 0;
            // Проходим только по четным элементам, 
            // четные - числа разложения, а нечетные - количество вхождений их в разложение
            while (iter < L.Count)
            {
                if (L[iter] == elem)
                {
                    L[iter + 1]++;
                    return;
                }
                else
                    iter += 2;
            }
            // если не входит
            L.Add(elem);
            L.Add(1);
        }

        // Считаем m = c^M(i) mod n
        public ulong find_m(int i, int n)
        {
            return (ulong)Math.Pow(this.c, this.M[i]) % (ulong)n;
        }

        // Находим d = НОД(m - 1, n)
        public int find_d(int m, int n)
        {
            return gcd(m - 1, n);
        }

        // Нахождение НОД(x, y)
        public int gcd(int x, int y)
        {
            if (y == 0)
                return x;
            else
                return gcd(y, x % y);
        }
    }
}
