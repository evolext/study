using System;

namespace Com_Methods
{
    /// <summary>
    /// преобразования Хаусхолдера
    /// </summary>
    class Householder_Transformation
    {
        /// <summary>
        /// реализация процедуры ортогонализации по методу отражений Хаусхолдера
        /// </summary>
        /// <param name="A - исходная матрица"></param>
        /// <param name="Q - ортогональная матрица преобразований"></param>
        /// <param name="R - результат"></param>
        public static void Householder_Orthogonalization(Matrix A, Matrix Q, Matrix R)
        {
            //инициализация вектора отражения
            Vector w = new Vector(A.M);

            //вспомогательные переменные
            double s, beta, mu;

            //алгоритм отражений Хаусхолдера
            for (int i = 0; i < A.N - 1; i++)
            {
                //находим квадрат нормы столбца для обнуления
                s = 0;
                for (int I = i; I < A.M; I++) s += Math.Pow(A.Elem[I][i], 2);

                //если есть ненулевые элементы под диагональю, тогда 
                //норма вектора для обнуления не совпадает с квадратом диагонального элемента
                if (Math.Abs(s - A.Elem[i][i] * A.Elem[i][i]) > CONST.EPS)
                {
                    //выбор знака слагаемого beta = sign(-x1)
                    if (A.Elem[i][i] < 0) beta = Math.Sqrt(s);
                    else beta = -Math.Sqrt(s);

                    //вычисляем множитель в м.Хаусхолдера mu = 2 / ||w||^2
                    mu = 1.0 / beta / (beta - A.Elem[i][i]);

                    //формируем вектор w
                    for (int I = 0; I < A.M; I++) { w.Elem[I] = 0; if (I >= i) w.Elem[I] = A.Elem[I][i]; }

                    //изменяем диагональный элемент
                    w.Elem[i] -= beta;

                    //вычисляем новые компоненты матрицы A = Hm * H(m-1) ... * A
                    for (int m = i; m < A.N; m++)
                    {
                        //произведение S = At * w
                        s = 0;
                        for (int n = i; n < A.M; n++) { s += A.Elem[n][m] * w.Elem[n]; }
                        s *= mu;
                        //A = A - 2 * w * (At * w)^t / ||w||^2
                        for (int n = i; n < A.M; n++) { A.Elem[n][m] -= s * w.Elem[n]; }
                    }

                    //вычисляем новые компоненты матрицы Q = Q * H1 * H2 * ...
                    for (int m = 0; m < A.M; m++)
                    {
                        //произведение Q * w
                        s = 0;
                        for (int n = 0; n < A.M; n++) { s += Q.Elem[m][n] * w.Elem[n]; }
                        s *= mu;
                        //Q = Q - w * (Q * w)^t
                        for (int n = i; n < A.M; n++) { Q.Elem[m][n] -= s * w.Elem[n]; }
                    }
                }
            }
            //R = Hm * ... H1 * A
            R.Elem = A.Elem;
        }
    }
}
