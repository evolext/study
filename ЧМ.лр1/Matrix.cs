using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    public interface Imatrix
    {
        int N { get; set; }
        int M { get; set; }
    }
    class Matrix : Imatrix
    {
        public int N { get; set; }
        public int M { get; set; }
        public double[][] Elem { set; get; }

        public Matrix() { }

        public Matrix(int n, int m)
        {
            N = n;
            M = m;
            Elem = new double[n][];
            for (int i = 0; i < n; i++)
                Elem[i] = new double[m];
        }
        // Ввод матрицы
        public void Input()
        {
            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.M; j++)
                    this.Elem[i][j] = double.Parse(Console.ReadLine());
            }
            Console.Clear();
        }
        // Создание матрицы случайныых чисел
        public void RandMatrix()
        {
            var r = new Random();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    this.Elem[i][j] = r.NextDouble() * 100;
            }
        }
        // Вывод матрицы консоль
        public void Print()
        {
            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.M; j++)
                    Console.Write(this.Elem[i][j] + "\t");
                Console.Write("\n");
            }
        }
        // Создание копии матрицы
        public Matrix Copy()
        {
            Matrix Result = new Matrix(this.N, this.M);
            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.M; j++)
                    Result.Elem[i][j] = this.Elem[i][j];
            }
            return Result;
        }
        // транспонирование матрицы
        public void Transpose()
        {
            double temp = .0;
            for (int i = 0; i < this.N; i++)
            {
                for (int j = i; j < this.M; j++)
                {
                    temp = this.Elem[i][j];
                    this.Elem[i][j] = this.Elem[j][i];
                    this.Elem[j][i] = temp;
                }
            }
        }

        // Умножение матрицы на веткор
        public static Vector operator *(Matrix T, Vector V)
        {
            if (T.M != V.N)
                throw new Exception("Unequal lengths\n");
            Vector result = new Vector(V.N);
            for (int i = 0; i < T.N; i++)
            {
                for (int j = 0; j < T.M; j++)
                    result.Elem[i] += T.Elem[i][j] * V.Elem[j];
            }
            return result;
        }

        // Произведение матрицы на матрицу
        public Matrix Matrix_mult(Matrix other)
        {
            if (this.M != other.N)
                throw new Exception("Unequal length\n");
            Matrix result = new Matrix(this.N, other.M);

            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < other.M; j++)
                {
                    for (int k = 0; k < this.M; k++)
                        result.Elem[i][j] += Elem[i][k] * other.Elem[k][j];
                }
            }

            return result;
        }

        // Перестановка двух строк матрицы местами i-ой и j-ой
        public void Row_swap(int i, int j)
        {
            double[] temp = Elem[i];
            Elem[i] = Elem[j];
            Elem[j] = temp;
        }

        // Домножение одной строки на число и прибавление к другой
        public void Row_add(int i, int j, double coef)
        {
            for (int k = 0; k < this.M; k++)
                this.Elem[j][k] += this.Elem[i][k] * coef;
        }


    

        // Приведение матрицы А к нижней треугольной,
        // т.е. получение матрицы L, на вход подается
        // уже готовая матрица U
        
    }
}