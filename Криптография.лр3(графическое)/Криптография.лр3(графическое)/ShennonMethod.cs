using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace encryption
{
    // Реализация кодирования методом Шеннона с 
    // добавление контрольного бита
    class ShennonCoding : SpecialMath
    {
        // Ввод данных
        public static string InputText(string PATH, List L)
        {
            // Вспомогательный буфер
            string lineRead = System.String.Empty;

            using (var Sr = new StreamReader(PATH))
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
        public static void ProbSave(string PATH, List L)
        {
            using (var Sw = new StreamWriter(PATH))
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

        // Вывод закодированного сообщения в файл вывода
        public static string OutputText(string PATH, string message, List L)
        {
            string output = System.String.Empty;
            using (var Sw = new StreamWriter(PATH))
            {
                var p = L.Head;
                string help = System.String.Empty;

                // Перевод сообщения
                for (int i = 0; i < message.Length; i++)
                {
                    p = L.FindCode((message[i]).ToString());
                    Sw.Write(p.Code + ":");
                    output += p.Code + ":";
                }
            }
            return output;
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

    // Реализация декодирования текста, закодированного
    // методом Шеннона сдобавлением проверочного бита в конце
    class ShennonDecoding
    {
        // Ввод закодированного сообщения
        public static string InputText(string PATH, List L)
        {
            using (var Sr = new StreamReader(PATH))
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

        // Выявление ошибочных символов по проверочному биту
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
                if (p.Probability >= 0)
                    Tree.Insert(p.Code);
            }
        }

        // Прямой обход бинарного дерева
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
        public static string Decoding(string PROB_PATH, string OUTPUT_PATH, string message, List L, ref string errors)
        {
            string result = System.String.Empty;
            string help = System.String.Empty;

            // Считываем входное сообщение
            using (var Sr = new StreamReader(PROB_PATH))
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
            using (var Sw = new StreamWriter(OUTPUT_PATH))
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
                        {
                            Sw.Write(p.Symbol);
                            result += p.Symbol;
                        }
                        else
                            // Если обнаружена ошибка
                            errors += "Ошибка в " + count + " слове;" + "  ";    
                        help = System.String.Empty;
                    }
                }

            }
            return result;
        }
    }
}