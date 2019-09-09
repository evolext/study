using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace math_space
{
    public interface Ivector
    {
        int N { get; set; } //size
    }

    class vector : Ivector
    {
        public int N { set; get; }
        public double[] Elem { set; get; }
        public vector() { }
        public vector(int n)
        {
            N = n;
            Elem = new double[n];
        }

        // Копирование векторв в вектор
        public vector Copy()
        {
            vector result = new vector(N);

            for (int i = 0; i < N; i++)
                result.Elem[i] = this.Elem[i];

            return result;
        }

        // Скалярное произведение
        public double Scalar_product(vector other)
        {
            if (this.N != other.N)
                throw new Exception("Unequal length\n");

            double result = 0; 

            for (int i = 0; i < N; i++)
                result += this.Elem[i] * other.Elem[i];


            return result;
        }

        // Умножение столбца на строку
        public matrix Column_mult_row(vector other)
        {
            if (this.N != other.N)
                throw new Exception("Unequal length\n");

            matrix result = new matrix(this.N, this.N);

            for (int i = 0; i < this.N; i++)
            {
                for (int j = 0; j < this.N; j++)
                    result.Elem[i][j] = this.Elem[i] * other.Elem[j];
            }


            return result;
        }

    }
}
