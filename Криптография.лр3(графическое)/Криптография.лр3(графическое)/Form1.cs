using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Криптография.лр3
{
    public partial class Form1 : Form
    {
        // Путь до входного файла
        public string inputBuf { get; set; }
        // Путь до файла вывода
        public string outputBuf { get; set; }
        // Путь до файла с вероятностями (для метода Шеннона)
        public string probabBuf { get; set; }


        public Form1()
        {
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////
        //
        //          СЕКЦИЯ КОДИРОВАНИЯ МЕТОДОМ ШЕННОНА
        //
        ////////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл для ввода
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                inputBuf = openFileDialog1.FileName;
            }
        }

        // Сохраняем путь до файла вывода 
        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                outputBuf = saveFileDialog1.FileName;
            }
        }

        // Сохраняем путь до файла вывода для вероятностей
        private void button13_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                probabBuf = saveFileDialog2.FileName;
            }
        }

        // Основной код
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (inputBuf == "" || inputBuf == null)
                    throw new Exception("Укажите файл с собщением для кодирования!");
                if (outputBuf == "" || outputBuf == null)
                    throw new Exception("Укажите файл для сохранения закодированного сообщения!");
                if (probabBuf == "" || probabBuf == null)
                    throw new Exception("Укажите файл для сохранения вероятностей!");

                // Структура для хранения символов алфавита и их кодов
                var L = new encryption.List();
                // Считывание заданного сообщения
                string input_buf = encryption.ShennonCoding.InputText(inputBuf, L);
                // Расчет вероятностей по равномерному закону
                encryption.ShennonCoding.ProbCalc(L);
                // Сортировка списка с символами в порядке убывания веротяностей
                encryption.ShennonCoding.SortList(L);
                // Подсчет кумулятивной вероятности
                encryption.ShennonCoding.CumulativeCalc(L);
                // Само кодирвоание симовлов
                encryption.ShennonCoding.Coding(L);
                // Вывод закодированное сообщения
                string output_buf = encryption.ShennonCoding.OutputText(outputBuf, input_buf, L);
                encryption.ShennonCoding.ProbSave(probabBuf, L);
                // Подсчет характеристик полученного кода + вывод в форму
                Specifications(L);
                // Вывд данных в форму
                textBox1.Text = input_buf;
                textBox2.Text = output_buf;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка");
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
        private void Specifications(encryption.List L)
        {
            var i = 1;
            var j = 0;
            int help;
            int d_min = L.Head.Code.Length;
            // 1. Находим все расстояния Хэмминга
            for (var p = L.Head; p != null; p = p.Next, i++)
            {
                j = i + 1;
                for (var q = p.Next; q != null; q = q.Next, j++)
                {
                    help = encryption.ShennonCoding.String_xor(p.Code, q.Code);
                    // Сразу запоминаем минимальное d
                    if (help < d_min)
                        d_min = help;
                    // Вывод в таблицу
                    dataGridView1.Rows.Add(i, j, help);
                }
                Console.WriteLine();
            }
            // 2. d0 уже найденна к концу циклов
            textBox3.Text = d_min.ToString();
            // 3. Кратность обнаружения
            int q_detection = d_min - 1;
            textBox4.Text = q_detection.ToString();
            // 4. Кратность исправления
            int q_correction = 0;
            if ((d_min & 1) == 0) // четное
                q_correction = d_min / 2 - 1;
            else
                q_correction = (d_min - 1) / 2;
            textBox5.Text = q_correction.ToString();
            // 5. граница Хэмминга, k и r
            int help1 = 0;
            for (i = 0; i <= q_correction; i++)
                help1 += encryption.ShennonCoding.Cominations(L.Head.Code.Length, i);
            double help2 = Math.Log(help1, 2);
            int r = (int)Math.Ceiling(help2);
            int k = L.Head.Code.Length - r;

            textBox6.Text = r.ToString() + " >= " + help2.ToString();

            if (r >= help2)
            {
                textBox7.BackColor = Color.LightGreen;
                textBox7.Text = "Выполнимо";
            }
            else
            {
                textBox7.BackColor = Color.Red;
                textBox7.Text = "Невыполнимо";
            }

            textBox8.Text = k.ToString();
            textBox9.Text = r.ToString();

            // 6. Граница Плоткина
            double help3 = (r + k) * Math.Pow(2, k - 1) / (Math.Pow(2, k) - 1);
            textBox10.Text = d_min.ToString() + " <= " + help3.ToString();

            if (d_min <= help3)
            {
                textBox11.BackColor = Color.LightGreen;
                textBox11.Text = "Выполнимо";
            }
            else
            {
                textBox11.BackColor = Color.Red;
                textBox11.Text = "Невыполнимо";
            }

            // 7. Граница Варшамова-Гильберта
            double help4 = 0;
            for (i = 0; i <= d_min - 2; i++)
                help4 += encryption.ShennonCoding.Cominations(r + k, i);
            textBox12.Text = (Math.Pow(2, r)).ToString() + " >= " + help4.ToString();

            if (Math.Pow(2, r) >= help4)
            {
                textBox13.BackColor = Color.LightGreen;
                textBox13.Text = "Выполнимо";
            }
            else
            {
                textBox13.BackColor = Color.Red;
                textBox13.Text = "Невыполнимо";
            }
        }

        ///////////////////////////////////////////////////////
        //
        //       СЕКЦИЯ ДЕКОДИРОВАНИЯ МЕТОДОМ ШЕННОНА
        //
        ///////////////////////////////////////////////////////
        
        // Получаем путь до файла ввода
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, сохраняем в буфер
                inputBuf = openFileDialog1.FileName;
            }
        }

        // Путь до файла вывода
        private void button5_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                outputBuf = saveFileDialog1.FileName;
            }
        }

        // Путь до файла с вероятностями
        private void button14_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                probabBuf = openFileDialog2.FileName;
            }
        }

        // Основной код
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (inputBuf == "" || inputBuf == null)
                    throw new Exception("Укажите файл с закодированным сообщением!");
                if (outputBuf == "" || outputBuf == null)
                    throw new Exception("Укажите файл для сохранения результата декодирования!");
                if (probabBuf == "" || probabBuf == null)
                    throw new Exception("Укажите файл с вероятностями!");

                var L = new encryption.List();
                var list = new encryption.List();
                var Tree = new encryption.BinaryTree();
                string error_pos = System.String.Empty;
                // Ввод закодированного сообщения из файла
                string input_buf = encryption.ShennonDecoding.InputText(inputBuf, L);
                // Идентифицирование ошибочных кодов
                encryption.ShennonDecoding.Error_detection(L);
                // Построение дерева для декодирования
                encryption.ShennonDecoding.BuildTree(L, Tree);
                // Прямой обход дерева с подсчетом вероятностей
                encryption.ShennonDecoding.Traversal(Tree.Root, list);
                // Декодирование сообщения
                string output_buf = encryption.ShennonDecoding.Decoding(probabBuf, outputBuf, input_buf, list, ref error_pos);

                // Вывод полученных результатов в форму
                textBox14.Text = input_buf;
                textBox15.Text = output_buf;
                if (error_pos == "" || error_pos == null)
                {
                    textBox16.BackColor = Color.LightGreen;
                    textBox16.Text = "Ошибок не обнаружено";
                }
                else
                    textBox16.Text = error_pos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка");
            }
        }


        ////////////////////////////////////////////////////
        //
        //       КОДИРОВАНИЕ КОДОМ ХЭММИНГА
        //
        ////////////////////////////////////////////////////

        // Путь до файла ввода
        private void button9_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, сохраняем в буфер
                inputBuf = openFileDialog1.FileName;
            }
        }

        // Путь до файла вывода
        private void button8_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                outputBuf = saveFileDialog1.FileName;
            }
        }

        // Основной код
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (inputBuf == "" || inputBuf == null)
                    throw new Exception("Укажите файл с сообщением для кодирования!");
                if (outputBuf == "" || outputBuf == null)
                    throw new Exception("Укажите файл для сохранения результата!");

                // Создаем список с буквами и кодами
                List<hamming_code.Node> Alphabet = hamming_code.HammingCoding.InputAlphabet(4);
                // Ввод матрицы G
                var G = new hamming_code.Matrix(4, 7);
                G.Input();
                // Находим преобразованные коды
                hamming_code.HammingCoding.GetCodes(Alphabet, G);
                // Ввод текста
                string input = hamming_code.HammingCoding.InputText(inputBuf);
                // Кодируем сообщение
                string output = hamming_code.HammingCoding.Coding(outputBuf, input, Alphabet);

                // Вывод полученных результатов в форму
                textBox29.Text = input;
                textBox28.Text = output;
                // Нахождение характеристик кода
                Characteristics(Alphabet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка");
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
        private void Characteristics(List<hamming_code.Node> Alphabet)
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
                    // Сразу записываем минимальное 
                    if (help < d_min)
                        d_min = help;
                    dataGridView2.Rows.Add(i + 1, j + 1, help);
                }
            }
            // 2. d0 уже найденна к концу циклов
            textBox27.Text = d_min.ToString();
            // 3. Кратность обнаружения
            int q_detection = d_min - 1;
            textBox26.Text = q_detection.ToString();
            // 4. Кратность исправления
            int q_correction = 0;
            if ((d_min & 1) == 0) // четное
                q_correction = d_min / 2 - 1;
            else                  // нечетное
                q_correction = (d_min - 1) / 2;
            textBox25.Text = q_correction.ToString();
            // 5. граница Хэмминга, k и r
            int help1 = 0;
            for (int i = 0; i <= q_correction; i++)
                help1 += encryption.SpecialMath.Cominations(code_length, i);
            double help2 = Math.Log(help1, 2);
            int r = (int)Math.Ceiling(help2);
            // k определяем через n и r
            int k = code_length - r;

            textBox24.Text = r.ToString() + " >= " + help2.ToString();
            if (r >= help2)
            {
                textBox23.BackColor = Color.LightGreen;
                textBox23.Text = "Выполнимо";
            }
            else
            {
                textBox23.BackColor = Color.Red;
                textBox23.Text = "Невыполнимо";
            }

            textBox22.Text = k.ToString();
            textBox21.Text = r.ToString();

            // 6. Граница Плоткина
            double help3 = (r + k) * Math.Pow(2, k - 1) / (Math.Pow(2, k) - 1);
            textBox20.Text = d_min.ToString() + " <= " + help3.ToString();

            if (d_min <= help3)
            {
                textBox19.BackColor = Color.LightGreen;
                textBox19.Text = "Выполнимо";
            }
            else
            {
                textBox19.BackColor = Color.Red;
                textBox19.Text = "Невыполнимо";
            }

            // 7. Граница Варшамова-Гильберта
            double help4 = 0;
            for (int i = 0; i <= d_min - 2; i++)
                help4 += encryption.SpecialMath.Cominations(r + k, i);

            textBox18.Text = (Math.Pow(2, r)).ToString() + " >= " + help4.ToString();

            if (Math.Pow(2, r) <= help4)
            {
                textBox17.BackColor = Color.LightGreen;
                textBox17.Text = "Выполнимо";
            }
            else
            {
                textBox17.BackColor = Color.Red;
                textBox17.Text = "Невыполнимо";
            }
        }

        ////////////////////////////////////////////////////////
        //
        //            ДЕКОДИРОВАНИЕ КОДА ХЭММИНГА
        //
        ////////////////////////////////////////////////////////
        private void button12_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, сохраняем в буфер
                inputBuf = openFileDialog1.FileName;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                outputBuf = saveFileDialog1.FileName;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (inputBuf == "" || inputBuf == null)
                    throw new Exception("Укажите файл с закодированным сообщением!");
                if (outputBuf == "" || outputBuf == null)
                    throw new Exception("Укажите файл для сохранения результата декодирования!");

                string errors = System.String.Empty;
                // Ввод матрицы G
                var G = new hamming_code.Matrix(4, 7);
                G.Input();
                // Получение матрицы H
                var H = hamming_code.HammingDecoding.GetMatrixH(G);
                // Считываем текст
                string input_buf = hamming_code.HammingDecoding.InputText(inputBuf);
                List<hamming_code.Node> Alphabet = hamming_code.HammingCoding.InputAlphabet(4);
                hamming_code.HammingCoding.GetCodes(Alphabet, G);
                // Вывод поулченного сообщения
                string output = hamming_code.HammingDecoding.DecodingProcedure(outputBuf, input_buf, Alphabet, H, ref errors);

                // Вывод результатов в форму
                textBox31.Text = input_buf;
                textBox30.Text = output;
                if (errors == null || errors == "")
                {
                    textBox32.BackColor = Color.LightGreen;
                    textBox32.Text = "Ошибок не обнаружено";
                }
                else
                    textBox32.Text = errors;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка");
            }
        }
    }
}