using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    // Класс, реализующий процесс ортогонализации
    // Грама-Шмидта, классический и модернизированный
    class GramSchmidt_procedure
    {
        public static void Classic_method(Matrix A, Matrix Q, Matrix R)
        {
            // Вспомогательный вектор
            Vector Help = new Vector(A.N);

            for (int j = 0; j < A.N; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    for (int k = 0; k < A.N; k++)
                        R.Elem[i][j] += A.Elem[k][j] * Q.Elem[k][i];
                }

                for (int k = 0; k < Help.N; k++)
                    Help.Elem[k] = A.Elem[k][j];

                for (int i = 0; i < j; i++)
                {
                    for (int k = 0; k < Help.N; k++)
                        Help.Elem[k] -= R.Elem[i][j] * Q.Elem[k][i];
                }

                R.Elem[j][j] = Help.Norma();

                if (R.Elem[j][j] < CONST.Eps)
                    return;

                for (int k = 0; k < Help.N; k++)
                    Q.Elem[k][j] = Help.Elem[k] / R.Elem[j][j];
            }

        }

        public static void Modified_method(Matrix A, Matrix Q, Matrix R)
        {
            // Вспомогательный вектор
            Vector Help = new Vector(A.N);

            for (int j = 0; j < A.N; j++)
            {
                for (int k = 0; k < Help.N; k++)
                    Help.Elem[k] = A.Elem[k][j];

                for (int i = 0; i < j; i++)
                {
                    for (int k = 0; k < Help.N; k++)
                        R.Elem[i][j] += Help.Elem[k] * Q.Elem[k][i];

                    for (int k = 0; k < Help.N; k++)
                        Help.Elem[k] -= R.Elem[i][j] * Q.Elem[k][i];
                }

                R.Elem[j][j] = Help.Norma();

                if (R.Elem[j][j] < CONST.Eps)
                    return;

                for (int k = 0; k < Help.N; k++)
                    Q.Elem[k][j] = Help.Elem[k] / R.Elem[j][j];
            }

        }   

    }
}
