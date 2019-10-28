using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace encryption
{
    public partial class Form1 : Form
    {
        // Строка, содержащая путь до файла ввода
        public string readingBuf { get; set; }
        // Строка, содержащая путь до файла с вероятностями
        public string probabilitiesBuf { get; set; }
        // Строка, содержащая путь до файла с алфавитом
        public string alphabetBuf { get; set; }
        // Строка, содержащая путь до файла вывода
        public string writingBuf { get; set; }

        // Инициализация формы
        public Form1()
        {
            InitializeComponent();
            readingBuf = System.String.Empty;
        }

        //////////////////////////////////////////////////////
        //
        //          СЕКЦИЯ КОДИРОВАНИЯ
        //
        //////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            // Очищаем рабочее прострнаство
            clearWorkspace();

            string buf = System.String.Empty;
            string output = System.String.Empty;
            var SymbolsList = new List();

            // Ошибка, не выбраны файлы исходыне
            try
            {
                if (readingBuf == "" || readingBuf == null)
                    throw new Exception("Выберите файл с сообщением для кодирвоания!");
                if (probabilitiesBuf == "" || probabilitiesBuf == null)
                    throw new Exception("Выберите файл с вероятностями!");

                try
                {
                    // Считываем сообщение, которое будем кодировать
                    buf = ShennonCoding.InputText(SymbolsList, this.readingBuf, this.probabilitiesBuf);
                    // Сортируем список в порядке убывания вероятностей
                    ShennonCoding.SortList(SymbolsList);
                    // Считаем кумулятивную вероятность
                    ShennonCoding.CumulativeCalc(SymbolsList);
                    // Само кодирование сообщения
                    ShennonCoding.Coding(SymbolsList);

                    // Сохранение закодированного сообщения
                    output = ShennonCoding.OutputText(SymbolsList, buf);


                    // Вывод исходного сообщения на экран
                    textBox1.Text = buf;
                    // Вывод закодированного сообщения на экран
                    textBox2.Text = output;
                    // ВЫвод на экран старого и нового алфавита
                    CodePrint(SymbolsList);
                    // Вывод на экран вектора Крафта
                    CraftVector(SymbolsList);
                    // Считаем среднюю длину и избыточность с последующим выводом их на экран
                    AverageLengthAndRedundancy(SymbolsList);
                    // Проверка неравнства Крафта
                    CraftInequality(SymbolsList);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Ошибка");
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка");
            }


            // Очищаем буферы петуй к файлам исходным
            clearBufers();
        }

        // Вывод алфавита на экран (в таблицу)
        private void CodePrint(List L)
        {
            for (var p = L.Tail; p != null; p = p.Prev)
                dataGridView1.Rows.Add(p.Symbol, p.Code);
        }

        // Вывод вектора Крафта на экран
        private void CraftVector(List L)
        {
            textBox3.Text += "( ";
            for (var p = L.Head; p != null; p = p.Next)
                textBox3.Text += p.Code.Length + " ";
            textBox3.Text += ")";
        }

        // Вычисление средней длины слова и избыточности
        private void AverageLengthAndRedundancy(List L)
        {
            double l = .0; // Переменная для хранения значения средней длины слова
            double entropy = .0; // Энтропия

            for (var p = L.Head; p != null; p = p.Next)
            {
                l += p.Code.Length * p.Probability;
                entropy -= p.Probability * Math.Log(p.Probability, 2);
            }
            // Выводим среднюю длину слова
            textBox4.Text = l.ToString();

            // Считаем избытончость
            textBox5.Text = (l - entropy).ToString() + " бит";
        }

        // Неравенство крафта
        private void CraftInequality(List L)
        {
            double k = .0; // Переменная для хранения суммы
            for (var p = L.Head; p != null; p = p.Next)
                k += Math.Pow(2, -p.Code.Length);
            textBox6.Text = k.ToString();

            // Проверка выполнения неравнства Крафта
            if (k <= 1)
            {
                textBox7.BackColor = Color.LightGreen;
                textBox7.Text = "Выполнимо";
            }
            else
            {
                textBox7.BackColor = Color.Red;
                textBox7.Text = "Невыполнимо";
            }
        }


        //////////////////////////////////////////////////////
        //
        //             СЕКЦИЯ ДЕКОДИРОВАНИЯ
        //
        //////////////////////////////////////////////////////
        private void button2_Click(object sender, EventArgs e)
        {
            // Очищаем рабочее прострнаство
            clearWorkspace();

            var SymbolsList = new List();
            var Tree = new BinaryTree();
            List LL = new List();
            string buf = System.String.Empty;


            try
            {
                if (readingBuf == "" || readingBuf == null)
                    throw new Exception("Выберите файл с сообщением для декодирвоания!");
                if (probabilitiesBuf == "" || probabilitiesBuf == null)
                    throw new Exception("Выберите файл с вероятностями!");

                // Считываем закодированное сообщение
                buf = ShennonDecoding.InputText(SymbolsList, readingBuf);
                // Строим бинарное дерево для декдирования
                ShennonDecoding.BuildTree(SymbolsList, Tree);
                // Обходим дерево и записываем в него вероятности
                ShennonDecoding.Traversal(Tree.Root, LL);
                // Само декодирование
                string output = ShennonDecoding.Decoding(LL, buf, probabilitiesBuf);

                // Вывод закодированного сообщения на экран
                textBox8.Text = buf;
                // Вывод раскодированного текста на экран
                textBox9.Text = output;
                // Вывод алфавита старого и нового
                SymbolsPrint(LL);
                // Сообщение об успешном декодировании
                MessageBox.Show("Декодирование прошло успешно", "Сообщение");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка");
            }

            // Очистка буфров с местоположением исходных файлов
            clearBufers();
        }

        //  Вывод расшифрованного алфавита на экран
        private void SymbolsPrint(List L)
        {
            for (var p = L.Head; p != null; p = p.Next)
                dataGridView2.Rows.Add(p.Code, p.Symbol);
        }

        // Открытие файла с помощью проводника windows
        private void button3_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, записываем в буфер
                readingBuf = openFileDialog1.FileName;
            }
        }

        // Открытие файла с вероятностями с помощью проводника windows
        private void button4_Click(object sender, EventArgs e)
        {
            // Открытие файла с вероятностями
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу
                probabilitiesBuf = openFileDialog2.FileName;
            }
        }

        // Сохранения закодированного сообщения в нужный пользователю файл
        private void button5_Click(object sender, EventArgs e)
        {
            // Если файл успешно открыт
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string name = saveFileDialog1.FileName;
                using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр2\Криптография.лр2\Output.txt"))
                {
                    string readLine = Sr.ReadLine();
                    // Сохранение
                    File.WriteAllText(name, readLine);
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение");

                }               
            }
        }

        // Открытие файла с закодированным текстом с помощью проводника windows
        private void button6_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, сохраняем в буфер
                readingBuf = openFileDialog1.FileName;
            }
        }

        // Открытие файла с вероятностями с помощью проводника windows
        private void button7_Click(object sender, EventArgs e)
        {
            // Открытие файла с вероятностями
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, записываем в буфер
                probabilitiesBuf = openFileDialog2.FileName;
            }
        }

        // Сохранение раскодированного сообщения при необходимости в нужный файл
        private void button8_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string name = saveFileDialog1.FileName;
                using (var Sr = new StreamReader(@"C:\Users\evole\source\repos\Криптография.лр2\Криптография.лр2\Output.txt"))
                {
                    string readLine = Sr.ReadLine();
                    // Сохранение
                    File.WriteAllText(name, readLine);
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение");

                }
            }
        }


        ///////////////////////////////////////////////////////
        //
        //              ГЕНЕРАЦИЯ ТЕКСТА
        //
        //////////////////////////////////////////////////////
        
        // Указать файл с алфавитом, на основе кторого будет сгенирированно сообщение
        private void button9_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, записываем в буфер
                alphabetBuf = openFileDialog1.FileName;
            }
        }

        // Указать файл с вероятностями для генерации текста
        private void button10_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, записываем в буфер
                probabilitiesBuf = openFileDialog1.FileName;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                // Получаем путь к файлу, записываем в буфер
                writingBuf = openFileDialog2.FileName;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (alphabetBuf == "" || alphabetBuf == null)
                    throw new Exception("Ошибка: Вы не указали файл с алфавитом!");
                if (probabilitiesBuf == "" || probabilitiesBuf == null)
                    throw new Exception("Вы не указали файл с вероятностями!");
                if (writingBuf == "" || writingBuf == null)
                    throw new Exception("Выберите файл для сохранения сгенерированного текста!");

                uint result;
                // Проверка введенного размера сообщения
                if (String.IsNullOrEmpty(textBox10.Text) || !UInt32.TryParse(textBox10.Text, out result) || result == 0)
                    throw new Exception("Ошибка: неверно указан размер генерированного сообщения! Укажите натуральное число.");
               
                // Получаем размер сообщения
                int size = int.Parse(textBox10.Text);

                TextGenerate.Procedure(size, alphabetBuf, probabilitiesBuf, writingBuf);
                MessageBox.Show("Сообщение успешно сгенерированно", "Сообщение");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка");
            }
            clearBufers();
        }


        // Очистка рабочего пространства от предыдущих работ
        private void clearWorkspace()
        {
            // Очищаем все поля перед использвоанием
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();

            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();

            textBox7.BackColor = SystemColors.ControlDark;
        }

        // Очистка буферов, хранящих информацию о пути к файлам
        private void clearBufers()
        {
            readingBuf = System.String.Empty;
            alphabetBuf = System.String.Empty;
            writingBuf = System.String.Empty;
            probabilitiesBuf = System.String.Empty;
        }

        
    }
}