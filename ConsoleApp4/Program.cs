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
         * 6 - Softmax
         */

        private static void Main()
        {
            Network network = new Network();
            int layersNum = 5;
            int[] neuronNum = { 3, 4, 5, 4, 2 };
            double learningRate = 0.5;
            bool randomInput = false;

            NeuronLayer[] neuronNetwork = network.Init(layersNum, neuronNum);
            neuronNetwork[0].SetFunctionsAll(4);
            neuronNetwork[1].SetFunctionsAll(3);
            neuronNetwork[2].SetFunctionsAll(4);
            neuronNetwork[3].SetFunctionsAll(4);
            neuronNetwork[4].SetFunctionsAll(6);

            double[,] InputValuesTraining = new double[4, 3] {
            { 0.5, 1, 1},
            { 5, 0.5, 1},
            { 4, 1, 1},
            { 0.5, 1, 1}
            };

            double[,] OutputValuesTraining = new double[4, 2] {
            { 0, 1},
            { 1, 0},
            { 1, 0},
            { 0, 1}
            };

            Console.WriteLine("\nEnter training counter:");
            int count = Convert.ToInt32(Console.ReadLine());
            network.Learn(neuronNetwork, InputValuesTraining, OutputValuesTraining, count, learningRate, randomInput);

            //Utility.ShowNeuronMap(neuronNetwork);
            Utility.ShowResults(neuronNetwork);
        }
    }
}