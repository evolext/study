using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace encryption
{
    class BeaufortCode
    {
        // Создаем алфавит
        public static List<char> AlphabetInit()
        {
            List<char> L = new List<char>();
            L.Add('a');
            L.Add('b');
            L.Add('c');
            L.Add('d');
            L.Add('e');
            L.Add('f');
            L.Add('g');
            L.Add('h');
            L.Add('i');
            L.Add('j');
            L.Add('k');
            L.Add('l');
            L.Add('m');
            L.Add('n');
            L.Add('o');
            L.Add('p');
            L.Add('q');
            L.Add('r');
            L.Add('s');
            L.Add('t');
            L.Add('u');
            L.Add('v');
            L.Add('w');
            L.Add('x');
            L.Add('y');
            L.Add('z');
            L.Add('_');
            return L;
        }

        // Получить индекс символа в алфавите по значению символа
        public static int GetCodeOfSymbol(List<char> Alphabet, char key)
        {
            for (int i = 0; i < Alphabet.Count; i++)
            {
                if (Alphabet[i] == key)
                    return i;
            }
            throw new Exception("Использован символ, не входящий в алфавит, перепроверьте данные или измените алфавит!");
        }

        // Реализация функции x mod y
        public static int Mod(int x, int y)
        {
            if (x >= 0)
                return x % y;
            else
                return y + (x % y);
        }

        // Считывание данных из указанного файла
        public static string InputData(string PATH)
        {
            // Буфер для записи входного сообщения
            string message = System.String.Empty;
            using (var Sr = new StreamReader(PATH))
            {
                // Считывание из файла
                message = Sr.ReadLine();
            }
            return message;
        }


        // Расширение ключа до размера сообщения
        public static string MakeKey(string key, int size)
        {
            // Повторяем ключ до размера сообщения size
            if (key.Length < size)
            {
                string original_key = key;
                while (key.Length < size)
                    key += original_key;
            }
            // Обрезать строку с ключом, если размер не совпадает с размером сообщения
            if (key.Length > size)
                key = key.Substring(0, size);
            return key;
        }

        // Процедура кодирования сообщения
        public static string Coding(string message, string key, List<char> Alphabet)
        {
            string result = System.String.Empty;
            int help = 0;
            for (int i = 0; i < message.Length; i++)
            {
                help = GetCodeOfSymbol(Alphabet, key[i]) - GetCodeOfSymbol(Alphabet, message[i]);
                help = Mod(help, Alphabet.Count);
                result += Alphabet[help].ToString();
            }
            return result;
        }

        // Процедура декодирования сообщения
        public static string Decoding(string encoded_message, string key, List<char> Alphabet)
        {
            string result = System.String.Empty;
            int help = 0;

            for (int i = 0; i < encoded_message.Length; i++)
            {
                help = GetCodeOfSymbol(Alphabet, key[i]) - GetCodeOfSymbol(Alphabet, encoded_message[i]);
                help = Mod(help - Alphabet.Count, Alphabet.Count);
                result += Alphabet[help].ToString();
            }
            return result;
        }


        // Сохранение результата в файл
        public static void SaveResult(string message, string PATH)
        {
            using (var Sw = new StreamWriter(PATH))
            {
                Sw.WriteLine(message);
            }
        }
    }
}