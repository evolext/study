using System;

namespace Com_Methods
{
    /// <summary>
    /// преобразования Гивенса
    /// </summary>
    class Givens_Transformation
    {
        /// <summary>
        /// реализация процедуры ортогонализации по методу вращений Гивенса
        /// </summary>
        /// <param name="A - исходная матрица"></param>
        /// <param name="Q - ортогональная матрица преобразований"></param>
        /// <param name="R - результат"></param>
        public static void Givens_Orthogonalization(Matrix A, Matrix Q, Matrix R)
        {
            double help1, help2;

            //косинус, синус
            double c = 0, s = 0;

            //алгоритм вращения Гивенса: для каждого столбца
            for (int j = 0; j < A.N - 1; j++)
            {
                //просматриваем строки в столбце
                for (int i = j + 1; i < A.M; i++)
                {
                    //если очередной элемент под диагональю не нулевой, то требуется поворот вектора
                    if (Math.Abs(A.Elem[i][j]) > CONST.EPS)
                    {
                        help1 = Math.Sqrt(Math.Pow(A.Elem[i][j], 2) + Math.Pow(A.Elem[j][j], 2));
                        c = A.Elem[j][j] / help1;
                        s = A.Elem[i][j] / help1;

                        //A_new = H * A (минус у матрицы вращения внизу)
                        for (int k = j; k < A.N; k++)
                        {
                            help1 = c * A.Elem[j][k] + s * A.Elem[i][k];
                            help2 = c * A.Elem[i][k] - s * A.Elem[j][k];
                            R.Elem[j][k] = help1;
                            R.Elem[i][k] = help2;
                            A.Elem[j][k] = help1;
                            A.Elem[i][k] = help2;
                        }
                        //перемножаем строки матрицы Q на трансп.матрицу преобразования
                        for (int k = 0; k < Q.M; k++)
                        {
                            help1 = c * Q.Elem[k][j] + s * Q.Elem[k][i];
                            help2 = c * Q.Elem[k][i] - s * Q.Elem[k][j];
                            Q.Elem[k][j] = help1;
                            Q.Elem[k][i] = help2;
                        }
                    }
                }
            }
        }
    }
}
