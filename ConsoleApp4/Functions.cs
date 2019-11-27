using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
    class Functions
    {
        public static double Sigmoid(double x)
        {
            double y = (1 / (1 + Math.Exp(-x)));
            //Console.WriteLine("Sigmoided " + x + " to " + y);
            return y;
        }
        public static double SigmoidDerivative(double sigmoid)
        {
            Console.WriteLine("Derivative of " + sigmoid + " is " + (sigmoid * (1 - sigmoid)));
            return (sigmoid * (1 - sigmoid));
        }

        public static double RelU(double x)
        {
            double y = x;
            if (y < 0) y = 0;
            Console.WriteLine("RelU " + x + " to " + y);
            return y;
        }
    }
}
