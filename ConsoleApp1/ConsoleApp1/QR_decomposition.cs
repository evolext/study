using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    // Класс, реализующий QR-разложение 3-мя способами:
    // 1. Процедура Грама-Шмидта (классический алгоритм)
    // 2. Процедура Грама-Шмидта (модифицированный вариант)
    // 3. Вращение Гивенса
    // 4. Вращение хаусхолдера
    // + соответствующее решение СЛАУ
    class QR_decomposition : Solution_Methods
    {
        // Функция для разложения матрицы выбранным способом, без решения СЛАУ
        public static void Decomposition(Matrix A, Matrix Q, Matrix R)
        {
            Console.WriteLine("Выберите один из способов разложения: ");
            Console.WriteLine("1: Процедура Грама-Шмидта (классический алгоритм)" + "\n2: Процедура Грама - Шмидта(модифицированный алгоритм)" + "\n3: Вращение Гивенса" + "\n4: Вращение Хаусхолдера");
            Console.Write("Выбор: ");
            int Choice = int.Parse(Console.ReadLine());
            // Инициализация матриц
            for (int i = 0; i < A.M; i++)
            {
                for (int j = 0; j < A.N; j++)
                    R.Elem[i][j] = .0;
            }
            for (int i = 0; i < A.N; i++)
                Q.Elem[i][i] = 1.0;

            switch(Choice)
            {
                case 1:
                    GramSchmidt_procedure.Classic_method(A, Q, R);
                    break;
                case 2:
                    GramSchmidt_procedure.Modified_method(A, Q, R);
                    break;
                case 3:
                    Givens_procedure.Transformation(A, Q, R);
                    break;
                case 4:
                    Householder_method.Transformation(A, Q, R);
                    break;
                default:
                    Console.Write("Ошибка: Неверный символ:");
                    break;
            }

        }

        // Разложение выбранным способ + последующее решение СЛАУ
        public static Vector Solve_Slough(Matrix A, Vector b)
        {
            Console.WriteLine("Выберите один из способов решения: ");
            Console.WriteLine("1: Процедура Грама-Шмидта (классический алгоритм)" + "\n2: Процедура Грама - Шмидта(модифицированный алгоритм)" + "\n3: Вращение Гивенса" + "\n4: Вращение Хаусхолдера");
            Console.Write("Выбор: ");
            int Choice = int.Parse(Console.ReadLine());
            // Инициализация матриц
            Matrix R = new Matrix(A.N, A.N);
            for (int i = 0; i < A.M; i++)
            {
                for (int j = 0; j < A.N; j++)
                    R.Elem[i][j] = .0;
            }
            Matrix Q = new Matrix(A.N, A.N);
            for (int i = 0; i < A.N; i++)
                Q.Elem[i][i] = 1.0;
            // Первый шаг: Разложение выбранным способом
            switch (Choice)
            {
                case 1:
                    GramSchmidt_procedure.Classic_method(A, Q, R);
                    break;
                case 2:
                    GramSchmidt_procedure.Modified_method(A, Q, R);
                    break;
                case 3:
                    Givens_procedure.Transformation(A, Q, R);
                    break;
                case 4:
                    Householder_method.Transformation(A, Q, R);
                    break;
                default:
                    Console.Write("Ошибка: Неверный символ:");
                    break;
            }
            // Второй шаг: Q*y=f => y=Q^(-1)*f=Q^(T)*f
            Q.Transpose();
            Vector y = Q * b;
            // Третий шаг: решение системы Rx=y
            Vector X = new Vector(A.N);
            Direct_row(A, y, X);
            return X;
        }
    }
}
