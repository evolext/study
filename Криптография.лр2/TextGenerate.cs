using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace encryption
{
    class TextGenerate
    {
        // Генерирование текста из заданного алфавита и размерности сообщения
        public static void Procedure(int text_size, string PATH1, string PATH2, string PATH3)
        {
            var List = new List();
            string help = System.String.Empty;
            string readLine = System.String.Empty;


            // Считываем алфавит
            using (var Sr = new StreamReader(PATH1))
            {
                readLine = Sr.ReadLine();

                if (readLine == null || readLine == "")
                    throw new Exception("Файл с алфавитом пуст, перепроверьте данные!");

                for (int i = 0; i < readLine.Length; i++)
                {
                    if (readLine[i] != ':')
                        help += readLine[i];
                    else
                    {
                        // Сохраняем выделенные символы в двусвязный список
                        // для дальнейших процедур
                        List.AddSymbol(help);
                        help = System.String.Empty;
                    }
                }

            }

            help = System.String.Empty;

            // Считываем вероятности
            using (var Sr = new StreamReader(PATH2))
            {
                string symb = System.String.Empty;

                while ((help = Sr.ReadLine()) != null)
                { 
                    symb = help.Split(' ')[0];
                    var p = List.FindCode(symb);
                    p.Probability = double.Parse(help.Split(' ')[1]);    
                } 
            }

            int k = 0;
            // Генерируем алфавит
            using (var Sw = new StreamWriter(PATH3))
            {
                for (var p = List.Head; p != null; p = p.Next)
                {
                    // Определеяем количество генерируемых символов
                    k = (int)(text_size * p.Probability);
                    // Записываем слова
                    for (int i = 0; i < k; i++)
                        Sw.Write(p.Symbol + ":");
                }
            }
        }
    }
}
