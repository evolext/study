using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Com_Methods
{
    /// <summary>
    /// класс семейтсва методов QR-декомпозиции
    /// </summary>
    class QR_Decomposition
    {
        //верхняя треугольная матрица
        public Matrix R { set; get; }
        //ортогональная матрица
        public Matrix Q { set; get; }
        //перечисление методов декомпозиции
        public enum QR_Algorithm
        {
            Classic_Gram_Schmidt = 1,
            Modified_Gram_Schmidt,
            Householder,
            Givens
        }

        /// <summary>
        /// реализация QR-декомпозиции
        /// </summary>
        /// <param name="A - исходная матрица"></param>
        /// <param name="Method - метод QR-декомпозиции"></param>
        public QR_Decomposition(Matrix A, QR_Algorithm Method)
        {
            R = new Matrix(A.M, A.N);
            Q = new Matrix(A.M, A.M);
            //начальная инициализация матрицы ортогональных преобразований
            for (int i = 0; i < A.M; i++) Q.Elem[i][i] = 1.0;

            switch (Method)
            {
                case QR_Algorithm.Classic_Gram_Schmidt:
                    {
                        Gram_Schmidt_Procedure.Classic_Gram_Schmidt_Procedure(A, Q, R);
                        break;
                    }
                case QR_Algorithm.Modified_Gram_Schmidt:
                    {
                        Gram_Schmidt_Procedure.Modified_Gram_Schmidt_Procedure(A, Q, R);
                        break;
                    }
                case QR_Algorithm.Givens:
                    {
                        Givens_Transformation.Givens_Orthogonalization(A, Q, R);
                        break;
                    }
                case QR_Algorithm.Householder:
                    {
                        Householder_Transformation.Householder_Orthogonalization(A, Q, R);
                        break;
                    }
            }
        }

        public Vector Start_Solver(Vector F)
        {
            var RES = Q.Multiplication_Trans_Matrix_Vector (F);
            Substitution_Method.Back_Row_Substitution(R, RES, RES);
            return RES;
        }
    }
}
