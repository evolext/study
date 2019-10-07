using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    // Реализация метода верхней релаксации
    class SOR_method
    {
        // На вход подаётся матрица А, вектор правой части
        // Нужная точность и параметр w
        // при w = 1 получаем метод Гаусса-Зейделя
        public static void Procedure(Matrix A, Vector b, Vector X, double w, double Eps)
        {
            Vector help = new Vector(b.N); // Вспомогательный
            Vector X_prev = new Vector(b.N); // Вектор предыдщуей итерации
            double accur = .0; // точность

            // Возьмем в качестве вектора нулевой итерации нулевой вектор
            for (int i = 0; i < b.N; i++)
                X_prev.Elem[i] = .0;

            do
            {
                for (int i = 0; i < A.N; i++)
                {
                    X.Elem[i] = b.Elem[i];

                    for (int j = 0; j < i; j++)
                        X.Elem[i] -= A.Elem[i][j] * X.Elem[j];

                    for (int j = i + 1; j < b.N; j++)
                        X.Elem[i] -= A.Elem[i][j] * X_prev.Elem[j];

                    X.Elem[i] *= w / A.Elem[i][i];
                    X.Elem[i] += (1 - w) * X_prev.Elem[i];
                }

                help = X - X_prev;
                accur = help.NormaLinf();
                X_prev = X.Copy();
            } while (accur > Eps);

        }
    }
}
