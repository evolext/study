using System;

namespace Com_Methods
{
    // абстрактный класс предобусловливателя
    abstract class Preconditioner
    {
        // реализация предобусловливателя
        abstract public void Start_Preconditioner(Vector X, Vector RES);
        // реализация транспонированного предобусловливателя
        abstract public void Start_Tr_Preconditioner(Vector X, Vector RES);
        // тип предобусловливателей
        public enum Type_Preconditioner
        {
            Diagonal_Preconditioner = 1,
            LU_Decomposition
        }
    }

    // диагональный предобусловливатель (Якоби)
    class Diagonal_Preconditioner : Preconditioner
    {
        //диагональ матрицы
        Vector Diag { get; }
        //конструктор диагонального преобусловливателя
        public Diagonal_Preconditioner(CSlR_Matrix A)
        {
            Diag = new Vector(A.N);
            for (int i = 0; i < A.N; i++)
            {
                if (Math.Abs(A.di[i]) < CONST.EPS) throw new Exception("Error in Diag_Preconditioner: CSlR_Matrix has 0 in the A.di");
                Diag.Elem[i] = A.di[i];
            }
        }
        //реализация преобусловливателя
        public override void Start_Preconditioner(Vector X, Vector RES)
        {
            for (int i = 0; i < Diag.N; i++)
            {
                RES.Elem[i] = X.Elem[i] / Diag.Elem[i];
            }
        }
        //реализация транспонированного преобусловливателя
        public override void Start_Tr_Preconditioner(Vector X, Vector RES)
        {
            for (int i = 0; i < Diag.N; i++)
            {
                RES.Elem[i] = X.Elem[i] / Diag.Elem[i];
            }
        }
    }

    //ILU-предобусловливатель (Incomplete LU-decomposition)
    class ILU_Preconditioner : Preconditioner
    {
        //матрица неполной LU-декомпозиции
        Sparse_Matrix ILU { set; get; }
        //конструктор ILU-преобусловливателя
        public ILU_Preconditioner(CSlR_Matrix A)
        {
            ILU = A.Create_ILU_Decomposition();
        }

        //реализация преобусловливателя
        public override void Start_Preconditioner(Vector X, Vector RES)
        {
            //решаем СЛАУ с нижней треугольной матрицей
            ILU.Slau_L(RES, X);
            //решаем СЛАУ с верхней треугольной матрицей
            ILU.Slau_U(RES, RES);
        }
        //реализация транспонированного преобусловливателя
        public override void Start_Tr_Preconditioner(Vector X, Vector RES)
        {
            //решаем СЛАУ с нижней треугольной матрицей
            ILU.Slau_Ut(RES, X);
            //решаем СЛАУ с верхней треугольной матрицей
            ILU.Slau_Lt(RES, RES);
        }
    }
}