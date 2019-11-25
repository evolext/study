using System;

namespace Com_Methods
{
    //методы прямой и обратной подстановок по строкам и столбцам
    class Substitution_Method
    {
        //прямая подстановка по строкам (А - нижняя треугольная матрица)
        public static void Direct_Row_Substitution(Matrix A, Vector F, Vector RES)
        {
            //скопируем по значениям вектор F в RES
            RES.Copy(F);

            //проход по строкам
            for (int i = 0; i < F.N; i++)
            {
                if (Math.Abs(A.Elem[i][i]) < CONST.EPS) throw new Exception("Direct Row Substitution: division by 0...");

                for (int j = 0; j < i; j++)
                {
                    RES.Elem[i] -= A.Elem[i][j] * RES.Elem[j];
                }

                RES.Elem[i] /= A.Elem[i][i];
            }
        }

        //прямая подстановка по столбцам (А - нижняя треугольная матрица)
        public static void Direct_Column_Substitution(Matrix A, Vector F, Vector RES)
        {
            //скопируем вектор F в RES
            RES.Copy(F);

            //проход по столбцам
            for (int j = 0; j < F.N; j++)
            {
                if (Math.Abs(A.Elem[j][j]) < CONST.EPS) throw new Exception("Direct Column Substitution: division by 0...");

                RES.Elem[j] /= A.Elem[j][j];

                for (int i = j + 1; i < F.N; i++)
                {
                    RES.Elem[i] -= A.Elem[i][j] * RES.Elem[j];
                }
            }
        }

        //обратная подстановка по строкам (А - верхняя треугольная матрица)
        public static void Back_Row_Substitution(Matrix A, Vector F, Vector RES)
        {
            //скопируем вектор F в RES
            RES.Copy(F);

            //начинаем с последней строки, двигаясь вверх
            for (int i = F.N - 1; i >= 0; i--)
            {
                if (Math.Abs(A.Elem[i][i]) < CONST.EPS) throw new Exception("Back Row Substitution: division by 0... ");

                //двигаемся по столбцам
                for (int j = i + 1; j < F.N; j++)
                {
                    RES.Elem[i] -= A.Elem[i][j] * RES.Elem[j];
                }

                RES.Elem[i] /= A.Elem[i][i];
            }
        }

        //обратная подстановка по столбцам (А - верхняя треугольная матрица)
        public static void Back_Column_Substitution(Matrix A, Vector F, Vector RES)
        {
            //скопируем вектор F в RES
            RES.Copy(F);

            //начинаем с последнего столбца, сдвигаясь влево
            for (int j = F.N - 1; j >= 0; j--)
            {
                if (Math.Abs(A.Elem[j][j]) < CONST.EPS) throw new Exception("Back Column Substitution: division by 0...");

                RES.Elem[j] /= A.Elem[j][j];

                //двигаемся по строкам
                for (int i = j - 1; i >= 0; i--)
                {
                    RES.Elem[i] -= A.Elem[i][j] * RES.Elem[j];
                }
            }
        }
    }
}
