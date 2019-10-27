using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            // Кодирование
            //var L = new List();
            //string input_buf = ShennonCoding.InputText(L);
            //ShennonCoding.ProbCalc(L);
            //ShennonCoding.SortList(L);
            //ShennonCoding.CumulativeCalc(L);
            //ShennonCoding.Coding(L);
            //ShennonCoding.AlphabetPrint(L); // Вывод алфавита
            //ShennonCoding.OutputText(L, input_buf);
            //ShennonCoding.ProbSave(L);

            //ShennonCoding.Сode_specifications(L);

            // Декодирование
            //var L = new List();
            //var Tree = new BinaryTree();

            //string input_buf = ShennonDecoding.InputText(L);
            //ShennonDecoding.Error_detection(L);
            //ShennonDecoding.BuildTree(L, Tree);

            //var list = new List();
            //ShennonDecoding.Traversal(Tree.Root, list);
            //ShennonDecoding.Decoding(list, input_buf);



            // Кодирование кода Хэмминга
            string path = @"C:\Users\evole\source\repos\Криптография.лр3(консольное)\Криптография.лр3(консольное)\Alphabet.txt";
            int N = 4;

            // Создаем список с буквами и кодами
            List<hamming_code.Node> Alphabet = hamming_code.HammingCoding.InputAlphabet(path, N);

            // Ввод матрицы G
            var G = new hamming_code.Matrix(4, 7);
            G.Input();

            // Находим преобразованные коды
            hamming_code.HammingCoding.GetCodes(Alphabet, G);

            // Кодируем сообщение

            hamming_code.HammingCoding.Coding(Alphabet);

            // Характеристики кода
            hamming_code.HammingCoding.Specifications(Alphabet);

            // Декодирование кода Хэмминга

            // Ввод матрицы G
            //var G = new hamming_code.Matrix(4, 7);
            //G.Input();
            //// Получение матрицы H
            //var H = hamming_code.HammingDecoding.GetMatrixH(G);

            //// Считываем текст
            //string input_buf = hamming_code.HammingDecoding.InputText();


            //List<hamming_code.Node> Alphabet = hamming_code.HammingCoding.InputAlphabet(path, 4);
            //hamming_code.HammingCoding.GetCodes(Alphabet, G);

            //hamming_code.HammingDecoding.DecodingProcedure(Alphabet, input_buf, H);
        }
    }
}
