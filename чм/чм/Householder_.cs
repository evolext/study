﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    // Класс, реализующий отражения Хаусхолдера
    class Householder_method: Solution_Methods
    {
        private static void Transformation(Matrix A, Matrix Q, Matrix R)
        {
            // Вектор отражения
            Vector w = new Vector(A.N);

            double s = .0;
            double beta = 0;
            double m = .0;

            // Основное тело
            for (int i = 0; i < A.M - 1; i++)
            {
                // Находим первый параметр
                for (int k = i; k < A.N; k++)
                    s += Math.Pow(A.Elem[k][i], 2);

                if (Math.Abs(s - Math.Pow(A.Elem[i][i], 2)) > CONST.EPS)
                {
                    // Выбор знака второго параметра
                    if (A.Elem[i][i] < 0)
                        beta = Math.Sqrt(s);
                    else
                        beta = -Math.Sqrt(s);

                    m = 1.0 / (beta * (beta - A.Elem[i][i]));

                    // Формирование вектора w
                    for (int k = 0; k < A.N; k++)
                    {
                        w.Elem[k] = 0;
                        if (k >= i)
                            w.Elem[k] = A.Elem[k][i];
                    }

                    w.Elem[i] -= beta;

                    for (int k = i; k < A.M; k++)
                    {
                        s = 0;
                        for (int n = i; n < A.N; n++)
                            s += A.Elem[n][k] * w.Elem[n];
                        s *= m;
                        for (int n = i; n < A.N; n++)
                            A.Elem[n][k] -= s * w.Elem[n];
                    }

                    for (int k = 0; k < A.N; k++)
                    {
                        s = .0;
                        for (int n = 0; n < A.N; n++)
                            s += Q.Elem[k][n] * w.Elem[n];
                        s *= m;
                        for (int n = i; n < A.N; n++)
                            Q.Elem[k][n] -= s * w.Elem[n];
                    }
                }
            }
            R.Elem = A.Elem;
        }

        public static Vector Solve_Slough(Matrix A, Vector b)
        {
            // Инициализация матриц
            Matrix R = new Matrix(A.N, A.N);
            for (int i = 0; i < A.M; i++)
            {
                for (int j = 0; j < A.N; j++)
                    R.Elem[i][j] = .0;
            }
            Matrix Q = new Matrix(A.N, A.N);
            for (int i = 0; i < A.N; i++)
                Q.Elem[i][i] = 1.0;

            Transformation(A, Q, R);


            Q.Transpose();
            Vector y = Q * b;
            Vector X = new Vector(A.N);
            Direct_row(A, y, X);
            return X;
        }

    }
}