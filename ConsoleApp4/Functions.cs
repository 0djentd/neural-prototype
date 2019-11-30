using System;

namespace ConsoleApp4
{
    internal class Functions
    {
        public double Derivative(double x, int functionType)
        {
            if (functionType == 0) return 1;
            else if (functionType == 1) return Functions.SigmoidDerivative(x);
            else if (functionType == 2) return Functions.TanHDerivative(x);
            else if (functionType == 3) return Functions.ReLUDerivative(x);
            else if (functionType == 4) return Functions.LeReLUDerivative(x);
            else if (functionType == 5)
            {
                Console.WriteLine("Function type error");
                return 0;
            }
            else return 0;
        }

        public double Derivative(double x, int functionType, double k)
        {
            if (functionType == 0) return 1;
            else if (functionType == 1) return Functions.SigmoidDerivative(x);
            else if (functionType == 2) return Functions.TanHDerivative(x);
            else if (functionType == 3) return Functions.ReLUDerivative(x);
            else if (functionType == 4) return Functions.LeReLUDerivative(x);
            else if (functionType == 5) return Functions.EReLUDerivative(x, k);
            else return 0;
        }

        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public static double SigmoidDerivative(double sigmoid)
        {
            return sigmoid * (1 - sigmoid);
        }

        public static double TanH(double x)
        {
            double y = 2 / (1 + Math.Exp(-2 * x)) - 1;
            return y;
        }

        public static double TanHDerivative(double tanh)
        {
            double y = 1 - Math.Pow(tanh, 2);
            return y;
        }

        public static double ReLU(double x)
        {
            if (x < 0) return 0;
            return x;
        }

        public static double ReLUDerivative(double x)
        {
            if (x < 0) return 0;
            else return 1;
        }

        public static double LeReLU(double x)
        {
            if (x < 0) return x * 0.02;
            return x;
        }

        public static double LeReLUDerivative(double x)
        {
            if (x < 0) return 0.02;
            else return 1;
        }

        public static double EReLU(double x, double k)
        {
            if (x < 0) x = k * (Math.Exp(x) - 1);
            return x;
        }

        public static double EReLUDerivative(double x, double k)
        {
            if (x < 0) return x + k;
            else return 1;
        }
    }
}