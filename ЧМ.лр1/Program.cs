using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace math_space
{
    class CONST
    {
        public static double Eps = 10e-9;
    }

    class Program
    {
        static void Main(string[] args)
        {
            int N = 500;
            Matrix A = new Matrix(N, N);
            //Vector b = new Vector(N);


            StreamReader sr = new StreamReader(@"C:\Users\evole\source\repos\ЧМ.лр1\ЧМ.лр1\Input.txt");

            //for (int i = 0; i < N; i++)
            //{
            //    for (int j = 0; j < N; j++)
            //        A.Elem[i][j] = double.Parse(sr.ReadLine());
            //}

            //for (int i = 0; i < N; i++)
            //    b.Elem[i] = double.Parse(sr.ReadLine());

            sr.Close();


            A.RandMatrix();

            Vector X_true = new Vector(N);
            for (int i = 0; i < N; i++)
                X_true.Elem[i] = 1.0;

            Vector f = A * X_true;



            // Замер времени
            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            //// do something

            //QR_decomposition.Solve_Slough(A, b);


            //sw.Stop();

            //TimeSpan ts = sw.Elapsed;
            //Console.WriteLine(ts.ToString());




            Vector X = QR_decomposition.Solve_Slough(A, f);



            Vector X2 = X - X_true;
            double Eps = X2.Norma() / X_true.Norma();
            Console.WriteLine(Eps);
            









            //Vector b = new Vector(N);
            //b.Input();

            //Vector X = QR_decomposition.Solve_Slough(A, b);
            //X.Print();

            






        }
    }
}
