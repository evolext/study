using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace math_space
{
    public interface Imatrix
    {
        int N { get; set; }
        int M { get; set; }
    }
    class Matrix : Imatrix
    {
        public int N { get; set; }
        public int M { get; set; }
        public double[][] Elem { set; get; }

        public Matrix() { }

        public Matrix(int n, int m)
        {
            N = n;
            M = m;
            Elem = new double[n][];
            for (int i = 0; i < n; i++)
                Elem[i] = new double[m];
        }

        //Считывание бинарных файлов
        public Matrix(string PATH)
        {
            // Размер системы
            using (var Reader = new BinaryReader(File.Open(PATH + "Size.bin", FileMode.Open))) // путь можно написать как \\DATA где data папка с файлами
            {
                this.N = Reader.ReadInt32();
                this.M = this.N;
            }

            using (var Reader = new BinaryReader(File.Open(PATH + "Matrix.bin", FileMode.Open)))
            {
                try
                {
                    this.Elem = new double[N][];
                    for (int i = 0; i < M; i++)
                    {
                        this.Elem[i] = new double[N];
                        for (int j = 0; j < N; j++)
                            this.Elem[i][j] = Reader.ReadDouble();
                    }
                }
                catch { throw new Exception("Matrix: data file is not correct..."); }
            }
        }





        delegate void Thread_Solver(int Number);

        public double Cond_Matrix()
        {
            Matrix At = new Matrix(N, N);

            // Проверка на квадратность
            if (M != N)
                throw new Exception("M != N");

            int Number_Threads = Environment.ProcessorCount;

            var Semaphores = new bool[Number_Threads];

            var Norma_Row_A = new double[Number_Threads];
            var Norma_Row_A1 = new double[Number_Threads];

            var Start_Solver = new Thread_Solver(Number =>
            {
                var A1 = new Vector(M);
                double S1, S2;

                int Begin = N / Number_Threads * Number;
                int End = Begin + N / Number_Threads;

                if (Number + 1 == Number_Threads)
                    End += N % Number_Threads;

                for (int i = Begin; i < End; i++)
                {
                    A1.Elem[i] = 1.0;

                    At = this.Copy();
                    At.Transpose();

                    A1 = Householder_method.Solve_Slough(At, A1);

                    S1 = .0; S2 = .0;
                    for (int j = 0; j < M; j++)
                    {
                        S1 += Math.Abs(Elem[i][j]);
                        S2 += Math.Abs(A1.Elem[j]);
                        A1.Elem[j] = .0;
                    }
                    if (Norma_Row_A[Number] < S1)
                        Norma_Row_A[Number] = S1;
                    if (Norma_Row_A1[Number] < S2)
                        Norma_Row_A1[Number] = S2;
                }
                Semaphores[Number] = true;
            });

            for (int I = 0; I < Number_Threads; I++)
            {
                int Number = Number_Threads - I - 1;
                ThreadPool.QueueUserWorkItem(Par => Start_Solver(Number));
            }

            Start_Solver(0);

            while (Array.IndexOf<bool>(Semaphores, false) != -1);

            for (int i = 1; i < Number_Threads; i++)
            {
                if (Norma_Row_A[0] < Norma_Row_A[i])
                    Norma_Row_A[0] = Norma_Row_A[i];
                if (Norma_Row_A1[0] < Norma_Row_A1[i])
                    Norma_Row_A1[0] = Norma_Row_A1[i];
            }

            return Norma_Row_A[0] * Norma_Row_A1[0];

        }




        // Ввод матрицы
        public void Input()
        {
            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.M; j++)
                    this.Elem[i][j] = double.Parse(Console.ReadLine());
            }
            Console.Clear();
        }
        // Создание матрицы случайныых чисел
        public void RandMatrix()
        {
            var r = new Random();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    this.Elem[i][j] = r.NextDouble() * 100;
            }
        }
        // Вывод матрицы консоль
        public void Print()
        {
            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.M; j++)
                    Console.Write(this.Elem[i][j] + "\t");
                Console.Write("\n");
            }
        }
        // Создание копии матрицы
        public Matrix Copy()
        {
            Matrix Result = new Matrix(this.N, this.M);
            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.M; j++)
                    Result.Elem[i][j] = this.Elem[i][j];
            }
            return Result;
        }
        // транспонирование матрицы
        public void Transpose()
        {
            double temp = .0;
            for (int i = 0; i < this.N; i++)
            {
                for (int j = i; j < this.M; j++)
                {
                    temp = this.Elem[i][j];
                    this.Elem[i][j] = this.Elem[j][i];
                    this.Elem[j][i] = temp;
                }
            }
        }

        // Умножение матрицы на веткор
        public static Vector operator *(Matrix T, Vector V)
        {
            if (T.M != V.N)
                throw new Exception("Unequal lengths\n");
            Vector result = new Vector(V.N);
            for (int i = 0; i < T.N; i++)
            {
                for (int j = 0; j < T.M; j++)
                    result.Elem[i] += T.Elem[i][j] * V.Elem[j];
            }
            return result;
        }

        // Произведение матрицы на матрицу
        public Matrix Matrix_mult(Matrix other)
        {
            if (this.M != other.N)
                throw new Exception("Unequal length\n");
            Matrix result = new Matrix(this.N, other.M);

            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < other.M; j++)
                {
                    for (int k = 0; k < this.M; k++)
                        result.Elem[i][j] += Elem[i][k] * other.Elem[k][j];
                }
            }

            return result;
        }

        // Перестановка двух строк матрицы местами i-ой и j-ой
        public void Row_swap(int i, int j)
        {
            double[] temp = Elem[i];
            Elem[i] = Elem[j];
            Elem[j] = temp;
        }

        // Домножение одной строки на число и прибавление к другой
        public void Row_add(int i, int j, double coef)
        {
            for (int k = 0; k < this.M; k++)
                this.Elem[j][k] += this.Elem[i][k] * coef;
        }
    }
}