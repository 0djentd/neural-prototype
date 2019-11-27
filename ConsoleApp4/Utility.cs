using System;

namespace ConsoleApp4
{
    class Utility
    {
        public static double Sigmoid(double x)
        {
            Console.WriteLine("Sigmoided " + x + " to " + (1 / (1 + Math.Exp(-x))));
            return (1 / (1 + Math.Exp(-x)));
        }

        public static double RelU(double x)
        {
            double y = x;
            if (y < 0) y = 0;
            Console.WriteLine("RelU " + x + " to " + y);
            return y;
        }

        public static double GetRandom()
        {
            Random random = new Random();
            return random.NextDouble() * 2 - 1;
        }

        public double[,] ArraySum(double[,] arrayA, double[,] arrayB)
        {
            double[,] arrayC = new double[arrayA.GetLength(0), arrayA.GetLength(1)];
            for (int i =0; i<arrayC.GetLength(0); i++)
            {
                for (int x = 0; x <arrayC.GetLength(1); x++)
                {
                    arrayC[x,i] = arrayA[x,i] + arrayB[x,i];
                }
            }
            return arrayC;
        }
        public double[,] ArraySub(double[,] arrayA, double[,] arrayB)
        {
            double[,] arrayC = new double[arrayA.GetLength(0), arrayA.GetLength(1)];
            for (int i = 0; i < arrayC.GetLength(0); i++)
            {
                for (int x = 0; x < arrayC.GetLength(1); x++)
                {
                    arrayC[x, i] = arrayA[x, i] - arrayB[x, i];
                }
            }
            return arrayC;
        }
        /*
        public double[,] ArraySub(double[,] arrayA, double[,] arrayB)
        {
            double[,] arrayC = new double[arrayA.GetLength(0), arrayA.GetLength(1)];
            for (int i = 0; i < arrayC.GetLength(0); i++)
            {
                for (int x = 0; x < arrayC.GetLength(1); x++)
                {
                    arrayC[x, i] = arrayA[x, i] - arrayB[x, i];
                }
            }
            return arrayC;
        }*/

        public static void Display2DArray(Array arrayName)
        {
            for (int i = 0; i < arrayName.GetLength(0); i++)
            {
                Console.Write("|");
                for (int x = 0; x < arrayName.GetLength(1); x++)
                {
                    Console.Write(" " + arrayName.GetValue(i, x));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static void DisplayArray(Array arrayName)
        {
            Console.Write("|");
            for (int i = 0; i < arrayName.GetLength(0); i++)
            {
                Console.Write(" " + arrayName.GetValue(i));
            }
            Console.WriteLine();
        }
    }
}
