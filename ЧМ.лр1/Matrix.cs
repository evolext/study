using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    public interface imatrix
    {
        int N { get; set; }
        int M { get; set; }
    }
    class matrix : imatrix
    {
        public int N { get; set; }
        public int M { get; set; }
        public double[][] Elem { set; get; }

        public matrix() { }

        public matrix(int n, int m)
        {
            N = n;
            M = m;
            Elem = new double[n][];
            for (int i = 0; i < n; i++)
                Elem[i] = new double[m];
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

        // Умножение матрицы на веткор
        public static vector operator *(matrix T, vector V)
        {
            if (T.M != V.N)
                throw new Exception("Unequal lengths\n");
            vector result = new vector(V.N);
            for (int i = 0; i < T.N; i++)
            {
                for (int j = 0; j < T.M; j++)
                    result.Elem[i] += T.Elem[i][j] * V.Elem[j];
            }
            return result;
        }

        // Произведение матрицы на матрицу
        public matrix Matrix_mult(matrix other)
        {
            if (this.M != other.N)
                throw new Exception("Unequal length\n");
            matrix result = new matrix(this.N, other.M);

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

        // Нахождение индекса строки с наибольшим элементом в столбце, k - вершина столбца, j - номер столбца
        public int Master_elem(int k, int j)
        {
            double max = .0;
            int res = k;

            while (k < this.N)
            {
                if (Math.Max(max, Math.Abs(this.Elem[k][j])) != max)
                {
                    max = Math.Abs(this.Elem[k][j]);
                    res = k;
                }
                k++;
            }

            return res;
        }

        // Приведение матрицы A к верхней треугольной U,
        // на вход также подается вектор b, потому что он тоже изменяется 
        // приперестановке строк и вычитании строк  
        public matrix To_upper_triangle()
        {
            // Инициализация матрицы U
            matrix result = new matrix(this.N, this.M);
            for (int i = 0; i < result.N; i++)
            {
                for (int j = 0; j < result.M; j++)
                    result.Elem[i][j] = this.Elem[i][j];
            }

            // Основное тело
            int k = 0;
            double coef = .0;
            for (int i = 0; i < result.M - 1; i++)
            {
                if ((k = result.Master_elem(i, i)) != i) // Если бОльший элемент не в i-ой строке, то делаем перестановку
                {
                    // перестановка строк для матрицы
                    result.Row_swap(i, k);
                }
                // Обнуляем элементы в столбце под наибольшим элементом
                for (int j = i + 1; j < result.N; j++)
                {
                    coef = -1 * result.Elem[j][i] / result.Elem[i][i];
                    result.Row_add(i, j, coef);
                }

            }

            return result;
        }

        // Приведение матрицы А к нижней треугольной,
        // т.е. получение матрицы L, на вход подается
        // уже готовая матрица U
        public matrix To_lower_triangle(matrix U)
        {
            matrix result = new matrix(this.N, this.M);
            double sum = .0;
            int k = 0;

            for (int j = 0; j < this.M; j++)
            {
                int i = j;
                result.Elem[i++][j] = 1;
                for (; i < N; i++)
                {
                    for (sum = .0, k = 0; k <= j - 1; k++)
                        sum += result.Elem[i][k] * U.Elem[k][j];
                    result.Elem[i][j] = 1.0 / U.Elem[j][j] * (this.Elem[i][j] - sum);
                }
            }

            return result;

        }

        // Прямой ход по строкам, применяется к матрице U
        // на вход подаётся вектор b (измененный)
        // т.е., сначала приводим матрицу А к верхней треугольной,
        // потом вызываем эту функцию для получения решения СЛАУ
        public vector Direct_row(vector b)
        {
            vector X = new vector(this.N);
            double sum = .0;
            int k = 0;

            for (int i = this.N - 1; i >= 0; i--)
            {
                for (sum = .0, k = i + 1; k <= this.M - 1; k++)
                    sum += this.Elem[i][k] * X.Elem[k];
                X.Elem[i] = (b.Elem[i] - sum) / this.Elem[i][i];
            }
            return X;
        }


        public vector Indirect_row(vector b)
        {
            vector X = new vector(this.N);
            double sum = .0;
            int k = 0;

            for (int i = 0; i < N; i++)
            {
                for (sum = .0, k = 0; k < i; k++)
                    sum += this.Elem[i][k] * X.Elem[k];
                X.Elem[i] = (b.Elem[i] - sum) / this.Elem[i][i];
            }

            return X;
        }



    }
}