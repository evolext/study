using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    class Givens_procedure
    {
        public static void Transformation(Matrix A, Matrix Q, Matrix R)
        {
            double help1 = .0;
            double help2 = .0;
            double cos = .0;
            double sin = .0;


            for (int j = 0; j < A.M - 1; j++)
            {
                for (int i = j + 1; i < A.N; i++)
                {
                    if (Math.Abs(A.Elem[i][j]) > CONST.Eps)
                    {
                        cos = A.Elem[j][j] / Math.Sqrt(Math.Pow(A.Elem[i][j], 2) + Math.Pow(A.Elem[j][j], 2));
                        sin = A.Elem[i][j] / Math.Sqrt(Math.Pow(A.Elem[i][j], 2) + Math.Pow(A.Elem[j][j], 2));

                        for (int k = j; k < A.M; k++)
                        {
                            help1 = cos * A.Elem[j][k] + sin * A.Elem[i][k];
                            help2 = cos * A.Elem[i][k] - sin * A.Elem[j][k];

                            R.Elem[j][k] = help1;
                            R.Elem[i][k] = help2;
                            A.Elem[j][k] = help1;
                            A.Elem[i][k] = help2;
                        }

                        for (int k = 0; k < Q.N; k++)
                        {
                            help1 = cos * Q.Elem[k][j] + sin * Q.Elem[k][i];
                            help2 = cos * Q.Elem[k][i] - sin * Q.Elem[k][j];

                            Q.Elem[k][j] = help1;
                            Q.Elem[k][i] = help2;
                        }

                    }
                }
            }



            

        }
    }
}
