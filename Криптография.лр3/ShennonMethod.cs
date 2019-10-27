using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace encryption
{
    class ShennonCoding: SpecialMath
    {
        // Ввод данных
        public static string InputText(List L)
        {
            string lineRead = System.String.Empty;
            using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Input.txt"))
            {
                lineRead = Sr.ReadLine();
                string help = System.String.Empty;

                // Проход по строке
                for (int i = 0; i < lineRead.Length; i++)
                {
                    if (lineRead[i] != '\n')
                        L.AddSymbol((lineRead[i]).ToString());
                }
            }
            return lineRead;
        }

        // Считаем вероятность поялвения для каждого символа по равномеррному закону
        public static void ProbCalc(List L)
        {
            for (var p = L.Head; p != null; p = p.Next)
                p.Probability = (double)p.Count / L.Size;
        }

        // Сортировка списка по убыванию вероятностей
        public static void SortList(List L)
        {
            double HelpProbability;
            int HelpConunt;
            string HelpSymbol;
            for (var p = L.Head.Next; p != null; p = p.Next)
            {
                for (var q = L.Head; q != p; q = q.Next)
                {
                    if (q.Probability < p.Probability)
                    {
                        HelpProbability = q.Probability;
                        HelpSymbol = q.Symbol;
                        HelpConunt = q.Count;

                        q.Probability = p.Probability;
                        q.Symbol = p.Symbol;
                        q.Count = p.Count;

                        p.Probability = HelpProbability;
                        p.Symbol = HelpSymbol;
                        p.Count = HelpConunt;
                    }
                }
            }
        }

        // Сохранение посчитанных вероятностей
        public static void ProbSave(List L)
        {
            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Prob.txt"))
            {
                for (var p = L.Head; p != null; p = p.Next)
                    Sw.WriteLine(p.Symbol + " " + p.Probability);
            }
        }
       
        // Считаем кумулятивную вероятность
        public static void CumulativeCalc(List L)
        {
            for (var p = L.Head.Next; p != null; p = p.Next)
                p.CumulativeProb = p.Prev.Probability + p.Prev.CumulativeProb;
        }

        // Нахождение кода методом Шеннона с добавлением в конце проверочного бита
        public static void Coding(List L)
        {
            double l = .0;
            int length = 0;
            string binCumulative;

            int checkBit; // переменная для проверочного бита
            int max_length = 0; // длина наибольшего кодовго слова алфавита, необходима для "выравнивания"

            // Основной цикл
            for (var p = L.Head; p != null; p = p.Next)
            {
                checkBit = 0;

                // 1. Вычисление l(i)
                l = -Math.Log(p.Probability, 2);
                // 2. Округление функцией "потолок"
                length = (int)Math.Ceiling(l);
                // 3. Представление кумулятивной вероятности в 2СС
                binCumulative = FloatToBin(p.CumulativeProb);
                // 4. несредственный код
                binCumulative = binCumulative.Substring(0, length);

                // Улучшение - добавление проверочного бита в конце
                for (var i = 0; i < binCumulative.Length; i++)
                    checkBit += binCumulative[i] - '0';
                binCumulative += (checkBit % 2).ToString();
                p.Code = binCumulative;

                if (max_length < binCumulative.Length)
                    max_length = binCumulative.Length;
            }
            // "Выравнивание" всех кодовых слов до общей длины
            for (var p = L.Head; p != null; p = p.Next)
                p.Code = new string('0', max_length - p.Code.Length) + p.Code;
        }

        // Вывод алфавита в консоль
        public static void AlphabetPrint(List L)
        {
            // Вывод нового алфавита
            Console.WriteLine("Новый алфавит");
            for (var p = L.Head; p != null; p = p.Next)
                Console.WriteLine(p.Symbol + "\t" + p.Code);
        }

        // Вывод закодированного сообщения в файл вывода
        public static void OutputText(List L, string message)
        {
            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Output.txt"))
            {
                var p = L.Head;
                string help = System.String.Empty;

                // Перевод сообщения
                for (int i = 0; i < message.Length; i++)
                {
                    p = L.FindCode((message[i]).ToString());
                    Sw.Write(p.Code + ":");
                }
            }
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
        public static void СodeSpecifications(List L)
        {
            // 1. Находим все расстояния Хэмминга
            var i = 1;
            var j = 0;
            int help;
            int d_min = L.Head.Code.Length;

            for (var p = L.Head; p != null; p = p.Next, i++)
            {
                j = i + 1;
                for (var q = p.Next; q != null; q = q.Next, j++)
                {
                    help = String_xor(p.Code, q.Code);
                    if (help < d_min)
                        d_min = help;
                    Console.WriteLine("d = {0} для {1} и {2} кода", help, i, j);
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
            for (i = 0; i <= q_correction; i++)
                help1 += Cominations(L.Head.Code.Length, i);
            double help2 = Math.Log(help1, 2);
            Console.WriteLine("Граница Хэминга r >= {0}", help2);
            int r = (int)Math.Ceiling(help2);
            int k = L.Head.Code.Length - r;
            Console.WriteLine("r = {0}, k = {1}", r, k);

            // 6. Граница Плоткина
            double help3 = (r + k) * Math.Pow(2, k - 1) / (Math.Pow(2, k) - 1);
            Console.WriteLine("Граница Плоткина d0 <= {0}", help3);

            // 7. Граница Варшамова-Гильберта
            double help4 = 0;
            for (i = 0; i <= d_min - 2; i++)
                help4 += Cominations(r + k, i);
            Console.WriteLine("{0} >= {1}", Math.Pow(2, r), help4);
        }


        // Сложение по модулю два двух строк
        public static int String_xor(string str1, string str2)
        {
            int help = 0;
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != str2[i])
                    help++;
            }
            return help;
        }


        
    }


    class ShennonDecoding
    {
        // Ввод закодированного сообщения
        public static string InputText(List L)
        {
            using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Input.txt"))
            {
                string help = System.String.Empty;

                string lineRead = Sr.ReadLine();
                // Обработка полученного буфера
                for (int i = 0; i < lineRead.Length; i++)
                {
                    if (lineRead[i] != ':')
                        help += lineRead[i];
                    else
                    {
                        L.AddCode(help);
                        help = System.String.Empty;
                    }
                }
                return lineRead;
            }
        }

        // Выявление ошибочных символов
        public static void Error_detection(List L)
        {
            int help = 0;
            for (var p = L.Head; p != null; p = p.Next)
            {
                help = 0;
                for (int i = 0; i < p.Code.Length - 1; i++)
                    help += p.Code[i] - '0';
                help %= 2;
                if (help != (p.Code[p.Code.Length - 1] - '0'))
                    p.Probability = -1;
            }
        }

        // Построеине дерева для декодирования
        public static void BuildTree(List L, BinaryTree Tree)
        {
            for (var p = L.Head; p != null; p = p.Next)
            {
                if(p.Probability >= 0)
                    Tree.Insert(p.Code);
            }    
        }

        // Обход прямой
        public static void Traversal(Elem pointer, List L)
        {
            if (pointer != null)
            {
                Traversal(pointer.Left, L);
                if (pointer.Left == null && pointer.Right == null)
                {
                    string help = System.String.Empty;
                    int k;

                    var p = pointer;
                    while (p.Parent != null)
                    {
                        k = p.Key;
                        help += k.ToString();

                        p = p.Parent;
                    }

                    char[] str = help.ToCharArray();
                    Array.Reverse(str);
                    string Help = new string(str);

                    L.AddCode(Help);

                }
                Traversal(pointer.Right, L);
            }
        }

        // Идентифицирование символов
        public static void Decoding(List L, string message)
        {
            string help = System.String.Empty;

            using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Prob.txt"))
            {
                for (var p = L.Head; p != null; p = p.Next)
                {
                    help = Sr.ReadLine();

                    p.Symbol = help.Split(' ')[0];
                    p.Probability = double.Parse(help.Split(' ')[1]);
                }
            }

            // Вывод раскодированного сообщения
            help = System.String.Empty;
            int count = 0;
            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Output.txt"))
            {
                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] != ':')
                        help += message[i];
                    else
                    {
                        count++;
                        var p = L.FindSymbol(help);
                        if (p != null)
                            Sw.Write(p.Symbol);
                        else
                            Console.WriteLine("Ошибка в " + count + " позиции");
                        help = System.String.Empty;
                    }
                }

            }

        }

        //  Вывод расшифрованного алфавита
        public static void AlphabetPrint(List L)
        {
            Console.WriteLine("Расшифрованный алфавит");
            for (var p = L.Head; p != null; p = p.Next)
                Console.WriteLine(p.Code + '\t' + p.Symbol);
        }
    }
}
