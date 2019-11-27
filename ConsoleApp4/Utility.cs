using System;

namespace ConsoleApp4
{
    class Utility
    {
        public static double Sigmoid(double x)
        {
            double y = (1 / (1 + Math.Exp(Math.Round(-x))));
            Console.WriteLine("Sigmoided " + x + " to " + y);
            return y;
        }
        public static double SigmoidDerivative(double sigmoid)
        {
            Console.WriteLine("Sigmoid derivative of " + sigmoid + " is " + (sigmoid * (1 - sigmoid)));
            return (sigmoid * (1 - sigmoid));
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
            return Math.Round(random.NextDouble() * 2 - 1, 6);
        }

        public double[,] ArraySum(double[,] arrayA, double[,] arrayB)
        {
            double[,] arrayC = new double[arrayA.GetLength(0), arrayA.GetLength(1)];
            for (int i = 0; i < arrayC.GetLength(0); i++)
            {
                for (int x = 0; x < arrayC.GetLength(1); x++)
                {
                    arrayC[x, i] = arrayA[x, i] + arrayB[x, i];
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

        public double[,] ArrayMult(double[,] arrayA, double[,] arrayB)
        {
            double[,] arrayC = new double[arrayA.GetLength(0), arrayB.GetLength(1)];
            for (int i = 0; i < arrayC.GetLength(0); i++)
            {
                for (int x = 0; x < arrayC.GetLength(1); x++)
                {
                    arrayC[x, i] = arrayA[x, i] * arrayB[i, x];
                }
            }
            return arrayC;
        }

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

        public static double[] Get0Dimension(double[,] arrayName, int z)
        {
            double[] result = new double[arrayName.GetLength(1)];
            for (int i = 0; i < arrayName.GetLength(1); i++)
            {
                result[i] = arrayName[z, i];
            }
            return result;
        }
    }
}
