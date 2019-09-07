using System;

namespace example_space
{
    class Program
    {
        static void Change(ref int[] arr)
        {
            arr[0] = 10;

            arr = new int[5] { 9, 8, 7, 6, 5 };

            arr[0] = 90;

        }

        static void Main()
        {
            int[] arr = { 1, 2, 3 };
            
            Change(ref arr);
            

        }
    }
}
