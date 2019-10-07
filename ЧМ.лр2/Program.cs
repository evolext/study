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
            string path = @"C:\Users\evole\OneDrive\Рабочий стол\System3\";

            int N = 2;
            Matrix A = new Matrix(N, N);
            Vector b = new Vector(N);
            Vector X = new Vector(N);

            A.Input();
            b.Input();

            SOR_method.Procedure(A, b, X, 0.5, 10e-3);

            X.Print();


        }
    }
}
