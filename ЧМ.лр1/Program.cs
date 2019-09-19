using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    class Program
    {
        static void Main(string[] args)
        {

            int N = 500;

            //matrix A = new matrix(N, N);
            //Console.WriteLine("Введите матрицу:");

            //for (int i = 0; i < N; i++)
            //{
            //    for (int j = 0; j < N; j++)
            //        A.Elem[i][j] = double.Parse(Console.ReadLine());
            //}

            //Console.WriteLine("Введите вектор:");
            //vector b = new vector(N);
            //for (int i = 0; i < N; i++)
            //    b.Elem[i] = double.Parse(Console.ReadLine());


            //matrix U = A.To_upper_triangle();
            //matrix L = A.To_lower_triangle(U);

            //vector y = L.Indirect_row(b);

            //vector x = U.Direct_row(y);

            //x.Print();

            vector X_true = new vector(N);
            for (int i = 0; i < N; i++)
                X_true.Elem[i] = 1.0;


            matrix A = new matrix(N, N);
            for (int i = 0; i < N; i++)
            {
                A.Elem[i][i] = 1.0e-3;
                for (int j = i + 1; j < N; j++)
                {
                    A.Elem[i][j] = 1.0 * (i + j + 1);
                    A.Elem[j][i] = -1.0 * (i + j + 1);
                }

            }

            vector f = A * X_true;

            matrix U = A.To_upper_triangle();
            matrix L = A.To_lower_triangle(U);

            vector y = L.Indirect_row(f);
            vector X = U.Direct_row(y);


            vector X2 = X_true - X;
            double Epsilon = X2.Norma() / X.Norma();

            Console.WriteLine(Epsilon);




            





            //var r = new Random();
            //for (int i = 0; i < N; i++)
            //{
            //    for (int j = 0; j < N; j++)
            //        A.Elem[i][j] = r.NextDouble() * 100;
            //    A.Elem[i][i] = 0.01;
            //}


           



        }
    }
}