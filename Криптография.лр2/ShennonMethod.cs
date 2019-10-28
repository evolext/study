using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace encryption
{

    // Класс, реализующий метод Шеннона для кодирования без потери информации
    class ShennonCoding
    {
        // Ввод данных
        public static string InputText(List L, string PATH1, string PATH2)
        {
            string lineRead = System.String.Empty;

            using (var Sr = new StreamReader(PATH1))
            {
                    string help = System.String.Empty;


                    // Считывание строки из файла
                    lineRead = Sr.ReadLine();
                    // Проверяем файл ввода на пустоту
                    if (lineRead == null || lineRead == "")
                        throw new Exception("Файл с закодированным сообщение пуст, перепроверьте данные!");

                    // Проход по строке
                    for (int i = 0; i < lineRead.Length; i++)
                    {
                        if (lineRead[i] != ':')
                            help += lineRead[i];
                        else
                        {
                            // Сохраняем выделенные символы в двусвязный список
                            // для дальнейших процедур
                            L.AddSymbol(help);
                            help = System.String.Empty;
                        }
                    }
            }

            // Ввод вероятностей, заданных пользователем
            using (var Sr = new StreamReader(PATH2))
            {
                string help = System.String.Empty;
                string symb = System.String.Empty;


                help = Sr.ReadLine();
                // Проверяем файл с вероятностями на пустоту
                if (help == null || help == "")
                    throw new Exception("Файл с вероятностями пуст, перепроверьте данные!");

                do
                {
                    symb = help.Split(' ')[0];
                    var p = L.FindCode(symb);
                    p.Probability = double.Parse(help.Split(' ')[1]);
                    help = Sr.ReadLine();
                } while(help != null && help != "");
            }

            // Вывод считанного буфера с сообщением
            return lineRead;
        }

        // Вывод закодированного сообщения в файл вывода
        public static string OutputText(List L, string message)
        {
            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\Криптография.лр2\Криптография.лр2\Output.txt"))
            {
                var p = L.Head;
                string help = System.String.Empty;
                string outputStr = System.String.Empty;

                // Осуществляем проход по исходному сообщению,
                // соответствующие считанные символы сопоставляем с созданнами для них шифрами,
                // записываем в выходной буфер
                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] != ':')
                        help += message[i];
                    else
                    {
                        p = L.FindCode(help);
                        Sw.Write(p.Code + ":");

                        outputStr += p.Code + ":";
                        help = System.String.Empty;
                    }
                }
                return outputStr;
            }
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

        // Считаем кумулятивную вероятность
        public static void CumulativeCalc(List L)
        {
            for (var p = L.Head.Next; p != null; p = p.Next)
                p.CumulativeProb = p.Prev.Probability + p.Prev.CumulativeProb;
        }

        // Неподсредственное кодирование
        public static void Coding(List L)
        {
            double l = .0;
            int length = 0;
            string binCumulative;
            // Основной цикл
            for (var p = L.Head; p != null; p = p.Next)
            {
                // 1. Вычисление l(i)
                l = -Math.Log(p.Probability, 2);

                if (Double.IsInfinity(l))
                    throw new Exception("Файл вероятности неполный, перепроверьте данные!");

                // 2. Округление функцией "потолок"
                length = (int)Math.Ceiling(l);
                // 3. Представление кумулятивной вероятности в 2СС
                binCumulative = FloatToBin(p.CumulativeProb);
                // 4. Получение конечного шифра для исмвола
                p.Code = binCumulative.Substring(0, length);
            }
        }

        // Перевод нецелого числа в двоичную СС
        public static string FloatToBin(double x)
        {
            string result = System.String.Empty;
            int help = 0;

            for (int i = 0; i < 20; i++)
            {
                x *= 2;
                help = (int)Math.Truncate(x);
                x -= help;
                result += help.ToString();
            }
            return result;
        }
    }


    // Класс, реализующий декодирвоание с помощью двочиного
    // дерева на основе заданных файлов с закодированным сообщением 
    // и файлом с вероятностями
    class ShennonDecoding
    {
        // Ввод закодированного сообщения
        public static string InputText(List L, string PATH)
        {
            using (var Sr = new StreamReader(PATH))
            {
                string help = System.String.Empty;

                string lineRead = Sr.ReadLine();

                // Проверка файла с закодированным сообщением на пустоту
                if(lineRead == "" || lineRead == null)
                    throw new Exception("Файл с закодированным сообщением пуст, перепроверьте данные!");


                // Обработка полученного буфера, выделяем символы и формируем алфавит
                for (int i = 0; i < lineRead.Length; i++)
                {
                    if (lineRead[i] != ':')
                        help += lineRead[i];
                    else
                    {
                        // Записываем считанный символ в двусвязный список
                        L.AddCode(help);
                        help = System.String.Empty;
                    }
                }
                // Сохраняем входной буфер
                return lineRead;
            }
        }

        // Построение бинарного дерева на основе списка с алфавитом шифров
        public static void BuildTree(List L, BinaryTree Tree)
        {
            for (var p = L.Head; p != null; p = p.Next)
                Tree.Insert(p.Code);
        }

        // Прямой обход бинарного дерева(рекурсивный) с записью вероятностей
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
                    // Записываем код символа, от корня к листу
                    char[] str = help.ToCharArray();
                    Array.Reverse(str);
                    string Help = new string(str);        
                    L.AddCode(Help);
                }
                Traversal(pointer.Right, L);
            }
        }

        // Идентифицирование символов
        public static string Decoding(List L, string message, string PATH)
        {
            string outputStr = System.String.Empty;
            string help = System.String.Empty;

            using (var Sr = new StreamReader(PATH))
            {
                help = Sr.ReadLine();
                // Проверяем файл с вероятностями на пустоту
                if (help == null || help == "")
                    throw new Exception("Файл с вероятностями пуст, перепроверьте данные!");

                var p = L.Head;
                do
                {
                    if (help == null || help == "")
                        throw new Exception("Файл вероятности неполный, перепроверьте данные!");


                    p.Symbol = help.Split(' ')[0];
                    p.Probability = double.Parse(help.Split(' ')[1]);

                    help = Sr.ReadLine();
                    p = p.Next;
                } while (p != null);
            }

            // Вывод раскодированного сообщения
            help = System.String.Empty;
            using (var Sw = new StreamWriter(@"C:\Users\evole\source\repos\Криптография.лр2\Криптография.лр2\Output.txt"))
            {
                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] != ':')
                        help += message[i];
                    else
                    {
                        var p = L.FindSymbol(help);
                        Sw.Write(p.Symbol + ":");

                        outputStr += p.Symbol + ":";
                        help = System.String.Empty;
                    }
                }
            }
            return outputStr;
        }
    }
}
