using System;

namespace ConsoleApp4
{
    class Functions
    {
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
            return (2 / (1 + Math.Exp(-2 * x))) - 1;
        }

        public static double TanHDerivative(double tanh)
        {
            return (1 - Math.Pow(tanh, 2));
        }

        public static double ReLU(double x)
        {
            if (x < 0) return 0;
            return x;
        }

        public static double ReLUDerivative(double relu)
        {
            if (relu < 0) return 0;
            return 1;
        }

        public static double LeReLU(double x)
        {
            if (x < 0) return x * 0.1;
            return x;
        }

        public static double LeReLUDerivative(double lerelu)
        {
            if (lerelu < 0) return 0.1;
            return 1;
        }

        public static double EReLU(double x, double k)
        {
            if (x < 0) return k * (Math.Exp(x) - 1);
            return x;
        }

        public static double EReLUDerivative(double erelu, double k)
        {
            if (erelu < 0) return erelu + k;
            else return 1;
        }

        public static double Softmax(int neuronNum, Layer layer)
        {
            double expSum = 0;
            for (int i = 0; i < layer.neuron.Count; i++)
            {
                expSum += Math.Exp(layer.neuron[i].Output);
            }
            return Math.Exp(layer.neuron[neuronNum].Output) / expSum;
        }

        public static double SoftmaxDerivative(int neuronNum, Layer layer)
        {
            double expSum = 0;
            for (int i = 0; i < layer.neuron.Count; i++)
            {
                expSum += Math.Exp(layer.neuron[i].Output);
            }
            double neuronExp = Math.Exp(layer.neuron[neuronNum].Output);
            return (neuronExp * (expSum - neuronExp)) / Math.Pow(expSum, 2);
        }
    }
}