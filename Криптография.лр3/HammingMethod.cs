using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace hamming_code
{
    // Класс, реализующий элемент списка с кодами символов
    class Node
    {
        // Символ исходный
        public string Symbol { get; set; }
        // Его шифр
        public int[] Code { get; set; }
        public Node Prev { get; set; }
        public Node Next { get; set; }

        // Конструктор
        public Node(string symb, int[] code)
        {
            this.Symbol = symb;
            this.Code = code;
            this.Prev = null;
            this.Next = null;
        }
    }

    // Класс матрицы
    class Matrix
    {
        // Количество строк матрицы
        public int N { get; set; }
        // Количество столбцов матрицы
        public int M { get; set; }
        // Элементы матрицы
        public int[][] Elem { set; get; }

        // Конструктор
        public Matrix(int n, int m)
        {
            N = n;
            M = m;
            Elem = new int[n][];
            for (int i = 0; i < n; i++)
                Elem[i] = new int[m];
        }

        // Ввод элементов через файл
        public void Input()
        {
            using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Matrix.txt"))
            {
                for (int i = 0; i < this.N; i++)
                {
                    for (int j = 0; j < this.M; j++)
                        this.Elem[i][j] = int.Parse(Sr.ReadLine());
                }
            }
        }
    }


    class HammingCoding: encryption.SpecialMath
    {
        // Инициализация алфавита
        // path - путь до файла с алфавитом
        // size - длина одного слова в символах
        public static List<Node> InputAlphabet(string PATH, int size)
        {
            List<Node> L = new List<Node>();
            string readline = System.String.Empty;
            string readSymbol = System.String.Empty;
            int[] readCode = new int[size];


            using (var Sr = new StreamReader(PATH))
            {
                int i;
                while ((readline = Sr.ReadLine()) != null)
                {
                    i = 0;
                    // Считываем шифр заданного размера
                    for (var count = 0; count < size; i++)
                        readCode[count++] = readline[i++] - '0';
                    // Считываем исходный символ
                    readSymbol = readline[i].ToString();
                    // Добавляем в список
                    L.Add(new Node(readSymbol, ArrCopy(readCode, size)));

                    // Очищаем буфер
                    readSymbol = System.String.Empty;
                    
                }
            }
            return L;
        }

        // Преобразование кодов символов при помощи матрицы G
        public static void GetCodes(List<Node> L, Matrix G)
        {
            int[] help = new int[G.M];
            int elem = 0; // Вспомгательная переменная

            // Для каждого элемента списка
            for (int i = 0; i < L.Count; i++)
            {
                // Первые N символов оставляем без изменения
                for (int j = 0; j < G.N; j++)
                    help[j] = L[i].Code[j];
                // 
                for (int j = G.N; j < G.M; j++)
                {
                    for (int k = 0; k < G.N; k++)
                        elem += L[i].Code[k] * G.Elem[k][j];
                    help[j] = elem % 2;
                    elem = 0;
                }

                // Переписываем код на преобразованный
                L[i].Code = ArrCopy(help, G.M);
            }
        }

        // Само кодирвоание
        public static void Coding(List<Node> Alphabet)
        {
            string input_buf = System.String.Empty; // Входной текст
            string output_buf = System.String.Empty; // Закодированное сообщение
            string help = System.String.Empty;

            using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Input.txt"))
            {
                input_buf = Sr.ReadLine();
            }

            // Анализируем входное сообщение
            for (int i = 0; i < input_buf.Length; i++)
            {
                if ((help = FindCodeOfSymbol(Alphabet, input_buf[i].ToString())) != System.String.Empty)
                    output_buf += help + ":";
            }

            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Output.txt"))
            {
                Sw.WriteLine(output_buf);
            }
        }

        // Поиск кода заданного символа
        public static string FindCodeOfSymbol(List<Node> L, string symbol)
        {
            string result = System.String.Empty;

            for (int i = 0; i < L.Count; i++)
            {
                if (L[i].Symbol == symbol)
                {
                    for (int k = 0; k < L[i].Code.Length; k++)
                        result += L[i].Code[k].ToString();
                    return result;
                }
            }
            return System.String.Empty;
        }

        // Создание копии массива
        public static int[] ArrCopy(int[] arr, int arr_size)
        {
            int[] new_arr = new int[arr_size];
            for (int i = 0; i < arr_size; i++)
                new_arr[i] = arr[i];
            return new_arr;
        }


        // Посчитать характеристики кода
        // Характеристики: 
        //                  1. все расстояния Хэмминга
        //                  2. расстояние d0
        //                  3. Кратность обнаружения
        //                  4. Кратность исправления
        //                  5. граница Хэмминга и из неё значения r, k
        //                  6. Проверка границы Плоткина
        //                  7. Проверка границы Варшамова-Гильберта
        public static void Specifications(List<Node> Alphabet)
        {
            // 1. Находим все расстояния Хэмминга
            int help;
            int d_min = Alphabet[0].Code.Length;
            int code_length = Alphabet[0].Code.Length;

            for (int i = 0; i < Alphabet.Count; i++)
            {
                for (int j = i + 1; j < Alphabet.Count; j++)
                {
                    help = 0;
                    // Ищем кодовое расстояние
                    for (int p = 0; p < Alphabet[i].Code.Length; p++)
                    {
                        if (Alphabet[i].Code[p] != Alphabet[j].Code[p])
                            help++;
                    }
                    if (help < d_min)
                        d_min = help;
                    Console.WriteLine("d = {0} для {1} и {2} кода", help, i + 1, j + 1);
                }
                Console.WriteLine();
            }

            // 2. d0 уже найденна к концу циклов
            Console.WriteLine("d0 = {0}", d_min);

            // 3. Кратность обнаружения
            int q_detection = d_min - 1;
            Console.WriteLine("qобн = {0}", q_detection);

            // 4. Кратность исправления
            int q_correction = 0;
            if ((d_min & 1) == 0) // четное
                q_correction = d_min / 2 - 1;
            else
                q_correction = (d_min - 1) / 2;
            Console.WriteLine("qисп = {0}", q_correction);


            // 5. граница Хэмминга, k и r
            int help1 = 0;
            for (int i = 0; i <= q_correction; i++)
                help1 += Cominations(code_length, i);
            double help2 = Math.Log(help1, 2);
            Console.WriteLine("Граница Хэминга r >= {0}", help2);
            int r = (int)Math.Ceiling(help2);
            int k = code_length - r;
            Console.WriteLine("r = {0}, k = {1}", r, k);

            // 6. Граница Плоткина
            double help3 = (r + k) * Math.Pow(2, k - 1) / (Math.Pow(2, k) - 1);
            Console.WriteLine("Граница Плоткина d0 <= {0}", help3);

            // 7. Граница Варшамова-Гильберта
            double help4 = 0;
            for (int i = 0; i <= d_min - 2; i++)
                help4 += Cominations(r + k, i);
            Console.WriteLine("{0} >= {1}", Math.Pow(2, r), help4);
        }
    }

    class HammingDecoding
    {
        // Получить матрицу H, имея матрицу G
        public static Matrix GetMatrixH(Matrix G)
        {
            // Определяем размер результирующей матрицы
            Matrix H = new Matrix(G.M - G.N, G.M);

            // Обнуляем матрицу перед заполнением
            for (int i = 0; i < H.N; i++)
            {
                for (int j = 0; j < H.M; j++)
                    H.Elem[i][j] = 0;
            }

            // Заполняем сначала R
            for (int i = 0; i < G.N; i++)
            {
                for (int j = G.N; j < G.M; j++)
                    H.Elem[j - G.N][i] = G.Elem[i][j];
            }
            // Заполняем единичную матрицу
            for (int i = 0; i < H.N; i++)
                H.Elem[i][H.M - H.N + i] = 1;
            return H;
        }

        // Ввод закодированного сообщения
        public static string InputText()
        {
            using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Input.txt"))
            {
                return Sr.ReadLine();
            }
        }


        // Сама процедура декодирования
        public static void DecodingProcedure(List<Node> Alphabet, string message, Matrix H)
        {
            string help1 = System.String.Empty;
            int[] help2 = new int[H.M - H.N];
            string symbol = System.String.Empty;
            string output_buf = System.String.Empty;


            int count = 0;
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] != ':')
                    help1 += message[i];
                else
                {
                    count++;
                    // Обрабатываем код на ошибки и получаем исходный шифр
                    help2 = FindingAndfixingErrors(help1, H, count);

                    if ((symbol = FindSymbolOfCode(Alphabet, help2)) != null)
                        output_buf += symbol;
                    else
                        Console.WriteLine("Ошибка в {0} слове", count);
                    help1 = System.String.Empty;
                }
            }

            // Вывод ответа в файл
            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Output.txt"))
            {
                Sw.WriteLine(output_buf);
            }

        }

        // Функция нахождения и возможной обработки ошибок
        public static int[] FindingAndfixingErrors(string code, Matrix H, int count)
        {
            // Переводим считанное слово в вид массива
            int[] arr = new int[H.M];
            int[] result = new int[H.M - H.N];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = code[i] - '0';

            // Вектор-результат умножения
            int[] res = new int[H.N];


            // Умножаем кодовое слово на транспонированную матрицу H
            int sum; // Вспомогательная переменая
            for (int i = 0; i < H.N; i++)
            {
                sum = 0;
                for (int j = 0; j < H.M; j++)
                    sum += H.Elem[i][j] * arr[j];
                // Сумма по модулю 2
                res[i] = sum % 2;
            }

            int index; // Индекс, для замены в коде

            // Если ничего не надо исправлять
            if (res[0] == 0 && res[1] == 0 && res[2] == 0)
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = arr[i];
                return result;
            }

            // Если надо исправлять ошибку
            // Определяем синдром, чтобы исправить ошибку
            if (res[0] == 0) // 001, 010, 011
            {
                if (res[1] == 0)
                    index = 6;
                else
                {
                    if (res[2] == 0)
                        index = 5;
                    else
                        index = 3;
                }
            }
            else             // 100, 101, 110, 111
            {
                if (res[1] == 0) // 100, 101
                {
                    if (res[2] == 0)
                        index = 4;
                    else
                        index = 1;
                }
                else            // 110, 111
                {
                    if (res[2] == 0)
                        index = 2;
                    else
                        index = 0;
                }     
            }            


            // Заменяем ошибочный бит
            if (arr[index] == 0)
                arr[index] = 1;
            else
                arr[index] = 0;

            
            // Проверяем на двухкратную ошибку
            for (int i = 0; i < H.N; i++)
            {
                sum = 0;
                for (int j = 0; j < H.M; j++)
                    sum += H.Elem[i][j] * arr[j];
                // Сумма по модулю 2
                res[i] = sum % 2;
            }

            Console.WriteLine("Исправлена ошибка в {0} слове в {1} позиции", count, index + 1);

            // получаем исходный код символа
            for (int i = 0; i < result.Length; i++)
                result[i] = arr[i];
            return result;
        }

        // Найти в списке значение символа по коду
        public static string FindSymbolOfCode(List<Node> L, int[] code)
        {
            bool flag;

            for (int i = 0; i < L.Count; i++)
            {
                flag = true;
                for (int k = 0; k < code.Length; k++)
                {
                    if (code[k] != L[i].Code[k])
                        flag = false;
                }

                if (flag)
                    return L[i].Symbol;

            }

            return null;
        }

    }
}