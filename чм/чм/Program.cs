using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    class CONST
    {
        public static double EPS = 1e-7;
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\evole\OneDrive\Рабочий стол\System3\";

            int N = 3;
            Matrix A = new Matrix(N, N);
            A.Input();
            //Vector b = new Vector(N);
            //b.Input();

            //Vector X = new Vector(N);


            //X = Householder_method.Solve_Slough(A, b);
            //X.Print();

            Console.WriteLine(A.Cond_Matrix());

        }
    }
}
