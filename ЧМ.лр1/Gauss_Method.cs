using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    // Класс, реализующий решение СЛАУ методом Гаусса
    class Gauss_Method: Solution_Methods
    {
        // Нахождение индекса строки с наибольшим элементом в столбце, 
        // k - номер строки начала столба, j - номер столбца
        private static int Master_elem(Matrix A, int k, int j)
        {
            double max = .0;
            int res = k;
            while (k < A.N)
            {
                if (Math.Max(max, Math.Abs(A.Elem[k][j])) != max)
                {
                    max = Math.Abs(A.Elem[k][j]);
                    res = k;
                }
                k++;
            }
            return res;
        }
        // Прямой ход - пприведение матрицы A к верхенму
        // треугольному виду и изменением вектора правой части
        private static void Direct_running(Matrix A, Vector b)
        {
            int k = 0;
            double coef = .0;
            double temp = .0;
            for (int i = 0; i < A.M - 1; i++)
            {
                if ((k = Master_elem(A, i, i)) != i) // Если бОльший элемент не в i-ой строке, то делаем перестановку
                {
                    // перестановка строк для матрицы
                    A.Row_swap(i, k);
                    // перестановка для вектора b
                    temp = b.Elem[i];
                    b.Elem[i] = b.Elem[k];
                    b.Elem[k] = temp;
                }
                // Обнуляем элементы в столбце под наибольшим элементом
                for (int j = i + 1; j < A.N; j++)
                {
                    coef = -1 * A.Elem[j][i] / A.Elem[i][i];
                    A.Row_add(i, j, coef);
                    // Изменяем вектор b
                    b.Elem[j] += b.Elem[i] * coef;
                }

            }
        }

        // Решение СЛАУ
        public static Vector Solve_Slough(Matrix A, Vector b)
        {
            // Первый шаг: приведение матрицы A
            // к верхнему треугольному виду
            Direct_running(A, b);

            Vector X = new Vector(A.N);

            // Второй шаг: исключение по строкам
            Direct_row(A, b, X);

            return X;
        }
    }
}
