using System;
using System.IO;

namespace Com_Methods
{
    public interface Sparse_Matrix
    {
        //размер матрицы
        int N { set; get; }
        //умножение матрицы на вектор y = A * x
        void Mult_MV(Vector X, Vector Y);
        //умножение транспонированной матрицы на вектор y = At * x
        void Mult_MtV(Vector X, Vector Y);
        //решение СЛАУ L * x = F с нижним треугольником матрицы
        void Slau_L(Vector X, Vector F);
        //решение СЛАУ Lt * x = F с нижним транспонированным треугольником матрицы
        void Slau_Lt (Vector X, Vector F);
        //решение СЛАУ U * x = F с верхним треугольником матрицы
        void Slau_U(Vector X, Vector F);
        //решение СЛАУ Ut * x = F с верхним транспонированным треугольником матрицы
        void Slau_Ut(Vector X, Vector F);
    }

    //матрица в разреженном строчно-столбцовом формате CSlR
    class CSlR_Matrix : Sparse_Matrix
    {
        //размер матрицы
        public int N { set; get; }
        //диагональ матрицы
        public double[] di {set; get;}
        //нижний треугольник
        public double[] altr { set; get; }
        //верхний треугольник
        public double[] autr { set; get; }
        //номера строк (столбцов) ненулевых элементов
        public int[] jptr { set; get; }
        //номера строк (столбцов), с которых начинается jptr
        public int[] iptr { set; get; }

        //конструктор по умолчанию
        public CSlR_Matrix() { }
        //конструктор по файлам
        public CSlR_Matrix(string PATH)
        {
            char[] Separator = new char[] {' '};

            //размер системы
            using (var Reader = new StreamReader(File.Open(PATH + "Size.txt", FileMode.Open)))
            {
                N = Convert.ToInt32(Reader.ReadLine());
                //выделение памяти под массивы di и iptr
                iptr = new int[N + 1];
                di   = new double[N];
            }

            //диагональ матрицы
            using (var Reader = new StreamReader(File.Open(PATH + "di.txt", FileMode.Open)))
            {
                for (int i = 0; i < N; i++)
                {
                    di[i] = Convert.ToDouble(Reader.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0]);
                }
            }

            //массив iptr
            using (var Reader = new StreamReader(File.Open(PATH + "iptr.txt", FileMode.Open)))
            {
                for (int i = 0; i <= N; i++)
                {
                    iptr[i] = Convert.ToInt32(Reader.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0]);
                }
            }

            //выделение памяти под массивы jptr, altr, autr
            int Size = iptr[N] - 1;
            jptr = new int[Size];
            altr = new double[Size];
            autr = new double[Size];
            var Reader1 = new StreamReader(File.Open(PATH + "jptr.txt", FileMode.Open));
            var Reader2 = new StreamReader(File.Open(PATH + "altr.txt", FileMode.Open));
            var Reader3 = new StreamReader(File.Open(PATH + "autr.txt", FileMode.Open));
            for (int i = 0; i < Size; i++)
            {
                jptr[i] = Convert.ToInt32 (Reader1.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0]);
                altr[i] = Convert.ToDouble(Reader2.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0]);
                autr[i] = Convert.ToDouble(Reader3.ReadLine().Split(Separator, StringSplitOptions.RemoveEmptyEntries)[0]);
            }
            Reader1.Close(); Reader2.Close(); Reader3.Close();
        }

        //-------------------------------------------------------------------------------------------------

        //умножение матрицы на вектор y = A * x
        public void Mult_MV (Vector X, Vector Y)
        {
            for (int i = 0; i < N; i++) Y.Elem[i] = X.Elem[i] * di[i];
            for (int i = 0; i < N; i++)
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                {
                    Y.Elem[i] += X.Elem[jptr[j] - 1] * altr[j];
                    Y.Elem[jptr[j] - 1] += X.Elem[i] * autr[j];
                }
        }

        //-------------------------------------------------------------------------------------------------

        //умножение транспонированной матрицы на вектор y = At * x
        public void Mult_MtV(Vector X, Vector Y)
        {
            for (int i = 0; i < N; i++) Y.Elem[i] = X.Elem[i] * di[i];
            for (int i = 0; i < N; i++)
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                {
                    Y.Elem[i] += X.Elem[jptr[j] - 1] * autr[j];
                    Y.Elem[jptr[j] - 1] += X.Elem[i] * altr[j];
                }
        }

        //-------------------------------------------------------------------------------------------------

        //решение СЛАУ L * x = F с нижним треугольником матрицы
        public void Slau_L(Vector X, Vector F)
        {
            for (int i = 0; i < N; i++)
            {
                X.Elem[i] = F.Elem[i];
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                    X.Elem[i] -= X.Elem[jptr[j] - 1] * altr[j];
                X.Elem[i] /= di[i];
            }
        }

        //-------------------------------------------------------------------------------------------------

        //решение СЛАУ Lt * x = F с нижним транспонированным треугольником матрицы
        public void Slau_Lt(Vector X, Vector F)
        {
            double[] V = new double[N];
            for (int i = 0; i < N; i++) V[i] = F.Elem[i];
            for (int i = N - 1; i >= 0; i--)
            {
                X.Elem[i] = V[i] / di[i];
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                    V[jptr[j] - 1] -= X.Elem[i] * altr[j];
            }
        }

        //-------------------------------------------------------------------------------------------------

        //решение СЛАУ U * x = F с верхним треугольником матрицы
        public void Slau_U(Vector X, Vector F)
        {
            for (int i = 0; i < N; i++) X.Elem[i] = F.Elem[i];
            for (int i = N - 1; i >= 0; i--)
            {
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                {
                    X.Elem[jptr[j] - 1] -= X.Elem[i] * autr[j];
                }
            }
        }

        //-------------------------------------------------------------------------------------------------

        //решение СЛАУ Ut * x = F с верхним транспонированным треугольником матрицы
        public void Slau_Ut(Vector X, Vector F)
        {
            for (int i = 0; i < N; i++)
            {
                X.Elem[i] = F.Elem[i];
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                    X.Elem[i] -= X.Elem[jptr[j] - 1] * autr[j];
            }
        }

        //-------------------------------------------------------------------------------------------------

        //операция неполного ILU-разложения (Incomplete LU-decomposition) в формате CSlR 
        public CSlR_Matrix Create_ILU_Decomposition()
        {
            var Matrix = new CSlR_Matrix();
            Matrix.N = N;
            Matrix.iptr = iptr;
            Matrix.jptr = jptr;
            int N_autr = autr.Length;
            Matrix.autr = new double[N_autr];
            Matrix.altr = new double[N_autr];
            Matrix.di   = new double[N];

            for (int i = 0; i < N_autr; i++) { Matrix.altr[i] = altr[i]; Matrix.autr[i] = autr[i]; }
            for (int i = 0; i < N; i++) { Matrix.di[i] = di[i]; }

            //начинаем с i = 1, т.к. в первой строке нижнего треугольника только диагональный элемент
            for (int i = 1; i < N; i++)
            {
                for (int j = iptr[i] - 1; j < iptr[i + 1] - 1; j++)
                {
                    for (int a = iptr[i] - 1; a < j; a++)
                    {
                        for (int b = iptr[jptr[j] - 1] - 1; b < iptr[jptr[j]] - 1; b++)
                        {
                            if (jptr[a] == jptr[b])
                            {
                                Matrix.altr[j] -= Matrix.altr[a] * Matrix.autr[b];
                                Matrix.autr[j] -= Matrix.autr[a] * Matrix.altr[b];
                            }
                        }
                    }
                    Matrix.autr[j] /= Matrix.di[jptr[j] - 1];
                    Matrix.di[i] -= Matrix.autr[j] * Matrix.altr[j];
                }
            }
            return Matrix;
        }
    }
}