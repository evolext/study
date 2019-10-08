using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    // Класс, релизующий методы обхода СЛАУ
    // Прямое и обратное исключение по строкам
    class Solution_Methods
    {
        // Прямое исключение по строкам
        protected static void Direct_row(Matrix A, Vector b, Vector Result)
        {
            double sum = .0;
            int k = 0;

            for (int i = A.N - 1; i >= 0; i--)
            {
                for (sum = .0, k = i + 1; k <= A.M - 1; k++)
                    sum += A.Elem[i][k] * Result.Elem[k];
                Result.Elem[i] = (b.Elem[i] - sum) / A.Elem[i][i];
            }
        }

        // Обратное исключение по строкам
        protected static void Indirect_row(Matrix A, Vector b, Vector Result)
        {
            double sum = .0;
            int k = 0;
            for (int i = 0; i < b.N; i++)
            {
                for (sum = .0, k = 0; k < i; k++)
                    sum += A.Elem[i][k] * Result.Elem[k];
                Result.Elem[i] = (b.Elem[i] - sum) / A.Elem[i][i];
            }
        }
    }
}