using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace encryption
{
    public partial class Form1 : Form
    {
        // Путь до входного файла
        public string inputBuf { get; set; }
        // Путь до файла вывода
        public string outputBuf { get; set; }

        public Form1()
        {
            InitializeComponent();
        }
        
        //////////////////////////////////////////////////////
        //
        //                   ШИФРОВАНИЕ
        //
        //////////////////////////////////////////////////////
        private void button1_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл для ввода
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Сохраняем до него путь
                inputBuf = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Сохраняем путь до файла вывода ответа
                outputBuf = saveFileDialog1.FileName;
            }
        }

        // Основной фрагмент
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на заполненность необходимых данных
                if (inputBuf == "" || inputBuf == null)
                    throw new Exception("Укажите файл с сообщением для шифрования!");
                if (outputBuf == "" || outputBuf == null)
                    throw new Exception("Укажите файл для сохранения зашифрованного сообщения!");
                if (textBox2.Text == "")
                    throw new Exception("Укажите ключ для шифрования!");


                // Инициализация структуры с алфавитом
                List<char> Alphabet = BeaufortCode.AlphabetInit();
                // Ввод исходного сообщения
                string message = BeaufortCode.InputData(inputBuf);
                // Изменение ключа до нужной длины
                string key = textBox2.Text;
                key = BeaufortCode.MakeKey(key, message.Length);
                // Непосредственно шифрование
                string result = BeaufortCode.Coding(message, key, Alphabet);
                // Сохранение результата в указанный файл
                BeaufortCode.SaveResult(result, outputBuf);
                // Вывод информации в формы
                textBox1.Text = message;
                textBox3.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //////////////////////////////////////////////////////
        //
        //                  ДЕШИФРОВАНИЕ
        //
        //////////////////////////////////////////////////////
        private void button4_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл для ввода
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Сохраним путь до файла ввода
                inputBuf = openFileDialog1.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Сохраним путь до файлла вывода
                outputBuf = saveFileDialog1.FileName;
            }
        }

        // Основной фрагмент
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на заполненность необходимых данных
                if (inputBuf == "" || inputBuf == null)
                    throw new Exception("Укажите файл с сообщением для шифрования!");
                if (outputBuf == "" || outputBuf == null)
                    throw new Exception("Укажите файл для сохранения зашифрованного сообщения!");
                if (textBox5.Text == "")
                    throw new Exception("Укажите ключ для шифрования!");

                // Инициализация структуры с алфавитом
                List<char> Alphabet = BeaufortCode.AlphabetInit();
                // Ввод зашифрованного сообщения
                string encoding_message = BeaufortCode.InputData(inputBuf);
                // Изменение ключа до нужной длины
                string key = textBox5.Text;
                key = BeaufortCode.MakeKey(key, encoding_message.Length);
                string result = BeaufortCode.Decoding(encoding_message, key, Alphabet);
                // Сохранение ррезультата в указанный файл
                BeaufortCode.SaveResult(result, outputBuf);
                // Вывод информации в формы
                textBox4.Text = encoding_message;
                textBox6.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}