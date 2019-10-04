using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    public interface Ivector
    {
        int N { get; set; } //size
    }

    class Vector : Ivector
    {
        public int N { set; get; }
        public double[] Elem { set; get; }
        public Vector() { }
        public Vector(int n)
        {
            N = n;
            Elem = new double[n];
        }
        // Ввод вектора
        public void Input()
        {
            for (int i = 0; i < this.N; i++)
            {
                this.Elem[i] = double.Parse(Console.ReadLine());
            }
            Console.Clear();
        }
        // ВЫвод вектора в консоль
        public void Print()
        {
            for (int i = 0; i < this.N; i++)
                Console.Write(this.Elem[i] + "\t");
        }
        // Копирование векторв в вектор
        public Vector Copy()
        {
            Vector result = new Vector(N);

            for (int i = 0; i < N; i++)
                result.Elem[i] = this.Elem[i];

            return result;
        }

        // Скалярное произведение
        public double Scalar_product(Vector other)
        {
            if (this.N != other.N)
                throw new Exception("Unequal length\n");

            double result = 0;

            for (int i = 0; i < N; i++)
                result += this.Elem[i] * other.Elem[i];


            return result;
        }

        // Умножение столбца на строку
        public Matrix Column_mult_row(Vector other)
        {
            if (this.N != other.N)
                throw new Exception("Unequal length\n");

            Matrix result = new Matrix(this.N, this.N);

            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.N; j++)
                    result.Elem[i][j] = this.Elem[i] * other.Elem[j];
            }


            return result;
        }

        // Вычитание векторов 
        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector result = new Vector(v1.N);

            for (int i = 0; i < v1.N; i++)
                result.Elem[i] = v1.Elem[i] - v2.Elem[i];


            return result;


        }
        // Вычисление нормы вектора по норме-2 
        public double Norma()
        {
            double sum = .0;

            for (int i = 0; i < this.N; i++)
                sum += Math.Pow(this.Elem[i], 2);

            return Math.Sqrt(sum);
        }
        // Вычисление нормы веткора по норме-инф
        public double NormaLinf()
        {
            double max = -1.0;
            for (int i = 0; i < this.N; i++)
                max = Math.Max(max, Math.Abs(this.Elem[i]));
            return max;
        }

    }
}