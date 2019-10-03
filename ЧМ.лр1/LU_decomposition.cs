using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    // Класс, реализующие метод LU-разложения матрицы
    // + решение СЛАУ при помощи этого разложения
    class LU_decomposition: Solution_Methods
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
        // Приведение матрицы к верхнему треугольному виду
        // т.е. получение матрицы U
        public static Matrix To_upper_triangle(Matrix A)
        {
            // Инициализация матрицы U
            Matrix U = A.Copy();

            // Основное тело
            int k = 0;
            double coef = .0;
            for (int i = 0; i < U.M - 1; i++)
            {
                if ((k = Master_elem(U, i, i)) != i) // Если бОльший элемент не в i-ой строке, то делаем перестановку
                {
                    // перестановка строк для матрицы
                    U.Row_swap(i, k);
                }
                // Обнуляем элементы в столбце под наибольшим элементом
                for (int j = i + 1; j < U.N; j++)
                {
                    coef = -1 * U.Elem[j][i] / U.Elem[i][i];
                    U.Row_add(i, j, coef);
                }

            }
            return U;
        }

        // Получение нижней треугольной матрицы, т.е.
        // матрицы L, при помощи готовой U
        public static Matrix To_lower_triangle(Matrix A, Matrix U)
        {
            Matrix L = new Matrix(A.N, A.M);
            double sum = .0;
            int k = 0;

            for (int j = 0; j < A.M; j++)
            {
                int i = j;
                L.Elem[i++][j] = 1;
                for (; i < A.N; i++)
                {
                    for (sum = .0, k = 0; k <= j - 1; k++)
                        sum += L.Elem[i][k] * U.Elem[k][j];
                    L.Elem[i][j] = 1.0 / U.Elem[j][j] * (A.Elem[i][j] - sum);
                }
            }
            return L;
        }

        // Решение СЛАУ
        public static Vector Solve_Slough(Matrix A, Vector b)
        {
            // Первый шаг: получение верхней труегольной матрицы U
            Matrix U = To_upper_triangle(A);
            // Второй шаг: получаем нижнюю треуголььную матрицу L
            Matrix L = To_lower_triangle(A, U);


            // Третий шаг: решение системы L*y = b
            // Методом обратного исключения по строкам
            Vector y = new Vector(A.N);
            Indirect_row(L, b, y);

            // Четвертый шаг: решение системы U*x = y
            // Методом прямого исключения по строкам
            Vector X = new Vector(A.N);
            Direct_row(U, y, X);
            return X;
        }
    }
}
