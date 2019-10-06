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
            int N = 3;
            Matrix A = new Matrix(N, N);
            A.Input();

            Vector X = new Vector(N);
            Vector b = new Vector(N);
            b.Input();

            Jacobi_method.Procedure(A, b, X, 1e-3);

            X.Print();


        }
    }
}
