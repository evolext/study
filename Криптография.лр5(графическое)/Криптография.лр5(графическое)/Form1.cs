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
        // Путь до файла с данными о первой функции
        public string pathFileFunctionFirst { get; set; }
        // Путь до файла с данными о второй функции
        public string pathFileFunctionSecond { get; set; }
        // Путь до файла с результатом работы программы
        public string pathFileResult { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        // Указать путь до файла с данными о первой функции
        private void button1_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл для ввода
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Сохраняем до него путь
                pathFileFunctionFirst = openFileDialog1.FileName;
            }
        }

        // Указать путь до файла с данными о второй функции
        private void button2_Click(object sender, EventArgs e)
        {
            // Если удалось открыть файл для ввода
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                // Сохраняем до него путь
                pathFileFunctionSecond = openFileDialog2.FileName;
            }
        }

        // Указать путь до файла сохранения результата
        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Сохраняем путь до файла вывода результата
                pathFileResult = saveFileDialog1.FileName;
            }
        }

        // Основной фрагмент
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка на заполненность всех необходимых данных
                if (pathFileFunctionFirst == "" || pathFileFunctionFirst == null)
                    throw new Exception("Укажите файл с данными о функции для первого LFSR-генератора!");
                if (pathFileFunctionSecond == "" || pathFileFunctionSecond == null)
                    throw new Exception("Укажите файл с данными о функции для второго LFSR-генератора!");
                if (textBox1.Text == "" || textBox1.Text == null)
                    throw new Exception("Укажите начальный элемент для первого LFSR-генератора!");
                if (textBox2.Text == "" || textBox2.Text == null)
                    throw new Exception("Укажите начальный элемент для второго LFSR-генератора!");
                if (textBox3.Text == "" || textBox3.Text == null)
                    throw new Exception("Укажите размер генерируемой последовательности!");
                

                // Две функции
                int size_a = 0; // Размерность первой функции
                int size_s = 0; // Размерность второй функции
                List<int> indices_a = PTG.inputFunction(ref size_a, pathFileFunctionFirst);
                List<int> indices_s = PTG.inputFunction(ref size_s, pathFileFunctionSecond);

                // Стратовое значение x0 для а и x0 для s
                string startValue_a = textBox1.Text;
                string startValue_s = textBox2.Text;

                // Проверка на соответствие размеров функции и начального значения x0
                if (size_a != startValue_a.Length)
                    throw new Exception("Порядок первой функции и размер первого начального элемента не совпадают!");
                if (size_s != startValue_s.Length)
                    throw new Exception("Порядок второй функции и размер второго начального элемента не совпадают!");

                // Проверка на содержание в стартовых значениях недопустимых символов
                for (int i = 0; i < startValue_a.Length; i++)
                {
                    if (startValue_a[i] != '0' && startValue_a[i] != '1')
                        throw new Exception("Первый начальный элемент содержит недопустимые символы!");
                }
                for (int i = 0; i < startValue_s.Length; i++)
                {
                    if (startValue_s[i] != '0' && startValue_s[i] != '1')
                        throw new Exception("Второй начальный элемент содержит недопустимые символы!");
                }

                // Проверка на нулевые стартовые значения x0
                bool flag = false;
                for (int i = 0; i < startValue_a.Length && !flag; i++)
                {
                    if (startValue_a[i] == '1')
                        flag = true;
                }
                if (!flag)
                    throw new Exception("Первый начальный элемент содержит одни нули!");
                flag = false;
                for (int i = 0; i < startValue_s.Length && !flag; i++)
                {
                    if (startValue_s[i] == '1')
                        flag = true;
                }
                if (!flag)
                    throw new Exception("Второй начальный элемент содержит одни нули!");

                // Длина сгенерированной последовательности
                int sizeOfSeries = int.Parse(textBox3.Text);
                // Получение результата
                // S - значение статистики для полученной последовательности
                double Stat = 0;
                string result = PTG.GenerationPT(startValue_a, startValue_s, indices_a, indices_s, sizeOfSeries, ref Stat);
                // Вывод реузльтата
                richTextBox1.Text = result;
                using (var Sw = new StreamWriter(pathFileResult))
                {
                    Sw.WriteLine(result);
                }

                // Получение и вывод в форму периода полученной последовательности
                // (если его возможно определеить аналитически)
                textBox4.Text = (PTG.PeriodOfPTG(startValue_a, startValue_s, indices_a, indices_s)).ToString();
                // Значение статистики
                textBox5.Text = Stat.ToString();
                // Вывод значения критической статистики для 2-1 степени свободы и a = 0,05
                textBox6.Text = "3,84";
                // Принятие или отвержение гипотезы о виде распределения
                if (Stat > 3.84)
                {
                    textBox7.BackColor = Color.Red;
                    textBox7.Text = "Отвергается";
                }
                else
                {
                    textBox7.BackColor = Color.LightGreen;
                    textBox7.Text = "Подтверждается";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}