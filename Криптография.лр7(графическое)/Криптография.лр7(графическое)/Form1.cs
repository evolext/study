using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace encryption
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Главный участок кода
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox1.Text == null)
                    throw new Exception("Укажите число для разложения!");

                // Число для разложения
                int Num = int.Parse(textBox1.Text);
                // Число итераций разложения
                int Iter = 0;
                // Для измерения времени разложения
                var sw = new Stopwatch();

                // Инициализация класса для метода
                var PM = new PollardMethod();
                // Структура для хранения множителей и их вхождений для разложения
                var factors = new List<int>();

                // Начало отчета времени выполнения программы
                sw.Start();
                PM.procedure(Num, ref factors, ref Iter);
                // Конец подсчета времени выполнения
                sw.Stop();

                // Выводим результаты работы программы в форму
                textBox2.BackColor = Color.LightGreen;
                textBox2.Text = "Составное";
                // Выводим число итераций разложения
                textBox4.Text = Iter.ToString();
                // Время разложения
                textBox5.Text = sw.Elapsed.TotalMilliseconds.ToString() + " мс";


                if (factors.Count > 0)
                {
                    // Вывод множителей в форму
                    for (int i = 0; i < factors.Count; i += 2)
                    {
                        for (int j = 0; j < factors[i + 1]; j++)
                            textBox3.Text += factors[i].ToString() + " * ";
                    }
                    textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 3, 3);

                    int k = 1;
                    for (int i = 0; i < factors.Count; i += 2, k++)
                        dataGridView1.Rows.Add(k, factors[i], factors[i + 1]);
                }
            }
            catch(Exception ex)
            {
                // Обрабатываем код ошибки
                if (ex.Message == "SimpleNumber")
                {
                    textBox2.Text = "Простое";
                    MessageBox.Show("Указанное число не является составным!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(ex.Message == "Impossible")
                {
                    MessageBox.Show("Не хватает вычислительной мощности для разложения данного числа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }
    }
}