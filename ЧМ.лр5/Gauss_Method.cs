using System;

namespace Com_Methods
{
    //метод Гаусса с ведущим элементом
    class Gauss_Method
    {
        //поик ведущего элемента в j-ом столбце матрицы
        public static int Find_Main_Element(Matrix A, int j)
        {
            int Index = j;
            for (int i = j + 1; i < A.M; i++)
                if (Math.Abs(A.Elem[i][j]) > Math.Abs(A.Elem[Index][j])) Index = i;
            if (Math.Abs(A.Elem[Index][j]) < CONST.EPS) throw new Exception("Gauss_Method: degenerate matrix...");
            return Index;
        }

        //прямой ход: построение верхней треугольной матрицы и вектора правой части (изменятся)
        public void Direct_Way (Matrix A, Vector F)
        { 
            //вспомогательная переменная
            double help;

            //прямой ход: реализация
            for (int i = 0; i < A.M - 1; i++)
            {
                //находим ведущий элемент в i-том столбце
                int I = Find_Main_Element(A, i);
                
                //если это не диагональ
                if (I != i)
                {
                    //переставляем строки I и i в СЛАУ местами
                    var Help = A.Elem[I];
                    A.Elem[I] = A.Elem[i];
                    A.Elem[i] = Help;

                    help = F.Elem[i];
                    F.Elem[i] = F.Elem[I];
                    F.Elem[I] = help;
                }
                
                //для оставшихся строк выполним умножение слева на матрицу преобразований
                for (int j = i + 1; j < A.M; j++)
                {
                    help = A.Elem[j][i] / A.Elem[i][i];
                    A.Elem[j][i] = 0; //чтобы нивелировать ошибки вычислений (машинный ноль!)
                    //вычитаем элементы строки i из строк от i + 1 до A.M 
                    for (int k = i + 1; k < A.N; k++)
                    {
                        A.Elem[j][k] -= help * A.Elem[i][k];
                    }
                    F.Elem[j] -= help * F.Elem[i];
                }
            }
        }

        //прямой ход: построение верхней треугольной матрицы
        public static void Direct_Way(Matrix A)
        {
            //вспомогательная переменная
            double help;

            //прямой ход: реализация
            for (int i = 0; i < A.M - 1; i++)
            {
                //находим ведущий элемент в i-том столбце
                int I = Find_Main_Element(A, i);

                //если это не диагональ
                if (I != i)
                {
                    //переставляем строки I и i в СЛАУ местами
                    var Help = A.Elem[I];
                    A.Elem[I] = A.Elem[i];
                    A.Elem[i] = Help;
                }

                //для оставшихся строк выполним умножение слева на матрицу преобразований
                for (int j = i + 1; j < A.M; j++)
                {
                    help = A.Elem[j][i] / A.Elem[i][i];
                    A.Elem[j][i] = 0; //чтобы нивелировать ошибки вычислений (машинный ноль!)
                    //вычитаем элементы строки i из строк от i + 1 до A.M 
                    for (int k = i + 1; k < A.N; k++)
                    {
                        A.Elem[j][k] -= help * A.Elem[i][k];
                    }
                }
            }
        }

        //метод Гаусса с ведущим элементом: реализация решателя
        public Vector Start_Solver (Matrix A, Vector F)
        {
            //прямой ход метода Гаусса
            Direct_Way(A, F);

            var RES = new Vector(F.N);

            //обратное ислючение в методе Гаусса через верхнюю треугольную матрицу
            Substitution_Method.Back_Row_Substitution(A, F, RES);

            return RES;
        }
    }
}
