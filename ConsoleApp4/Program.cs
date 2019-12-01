using System;

namespace ConsoleApp4
{
    internal class Program
    {
        /* 0 - No act. function
         * 1 - Sigmoid
         * 2 - TanH
         * 3 - ReLU
         * 4 - LeReLU
         * 5 - EReLU
         * 6 - Softmax (-)
         */

        private static void Main()
        {
            Network network = new Network();
            int layersNum = 4;
            int[] neuronNum = { 1, 5, 5, 1 };
            double learningRate = 0.030;

            NeuronLayer[] neuronNetwork = network.Init(layersNum, neuronNum);
            neuronNetwork[0].SetFunctionsAll(0);
            neuronNetwork[1].SetFunctionsAll(1);
            neuronNetwork[2].SetFunctionsAll(1);
            neuronNetwork[3].SetFunctionsAll(1);
            /*
            double[,] InputValuesTraining = new double[4, 3] {
            { 0, 1, 1},
            { 1, 0, 1},
            { 1, 1, 1},
            { 0, 1, 0}
            };

            double[,] OutputValuesTraining = new double[4, 2] {
            { 0.1, 0.7},
            { 0.7, 0.1},
            { 0.7, 0.1},
            { 0.1, 0.7}
            };
            */
            double[,] InputValuesTraining = new double[2, 1] {
            { 0},
            { 1},
            };

            double[,] OutputValuesTraining = new double[2, 1] {
            { 0},
            { 1},
            };

            Console.WriteLine("\nEnter training counter:");
            int count = Convert.ToInt32(Console.ReadLine());
            network.Learn(neuronNetwork, InputValuesTraining, OutputValuesTraining, count, learningRate);
            //Utility.ShowResults(neuronNetwork);
        }
    }
}