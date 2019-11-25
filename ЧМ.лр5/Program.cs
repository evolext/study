using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Com_Methods
{
    class CONST
    {
        static public double EPS = 1e-7;
    }
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch sw = new Stopwatch();
            
            var A = new CSlR_Matrix(@"C:\Users\evole\OneDrive\Рабочий стол\Systems1\SPD\");
            var F = new Vector(A.N);
            var X_true = new Vector(A.N);
            for (int i = 0; i < A.N; i++)
            {
                X_true.Elem[i] = 1.0;
            }
            A.Mult_MV(X_true, F);
            var Solver = new Conjugate_Gradient_Method(30000, 1e-10);

            sw.Start();
            var X = Solver.Start_Solver(A, F, Preconditioner.Type_Preconditioner.LU_Decomposition);
            sw.Stop();
            Console.WriteLine((sw.ElapsedMilliseconds / 1000.0).ToString());
        }
    }
}
