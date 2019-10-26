using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption
{
    class SpecialMath
    {
        // Вычсиление сочетаний из N по M
        public static int Cominations(int N, int M)
        {
            return Factorial(N) / (Factorial(M) * Factorial(N - M));
        }

        public static int Factorial(int num)
        {
            int n = 1;
            int result = 1;
            while (n <= num)
            {
                result *= n;
                n++;
            }
            return result;
        }





        // Перевод дробного числа в двоичную СС
        public static string FloatToBin(double x)
        {
            string result = System.String.Empty;
            int help = 0;
            for (int i = 0; i < 20; i++)
            {
                x *= 2;
                help = (int)Math.Truncate(x);
                x -= help;
                result += help.ToString();
            }
            return result;
        }
    }
}
