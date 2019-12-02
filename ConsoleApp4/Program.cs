using System;
using System.Diagnostics;

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
            bool debug = false;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            NeuralNetwork network = new NeuralNetwork();
            int layersNum = 4;
            int[] neuronNum = { 3, 3, 3, 1 };
            int batch = 0;
            double learningRate = 0.5;

            NeuronLayer[] neuronNetwork = network.Init(layersNum, neuronNum, true);
            Utility.ShowNeuronMap(neuronNetwork, false);
            NeuralNetwork.Randomize(neuronNetwork);

            neuronNetwork[0].SetFunctionsAll(4);
            neuronNetwork[1].SetFunctionsAll(2);
            neuronNetwork[2].SetFunctionsAll(2);
            neuronNetwork[2].SetFunctionsAll(2);


            double[,] InputValuesTraining = new double[4, 3] {
            { 0, 1, 1},
            { 1, 0, 1},
            { 1, 1, 1},
            { 0, 0, 1}
            };

            double[,] InputValuesPredict = new double[1, 3] {
            { 1, 0, 0},
            };

            double[,] OutputValuesTraining = new double[4, 1] {
            { 0},
            { 1},
            { 1},
            { 0}
            };

            /*
            double[,] InputValuesTraining = new double[2, 1] {
            { 0.2},
            { 0.7},
            };

            double[,] OutputValuesTraining = new double[2, 1] {
            { 0.2},
            { 0.7 }
            };*/

            Console.WriteLine("\nEnter epoch:");
            int epoch = Convert.ToInt32(Console.ReadLine());
            
            network.Learn(neuronNetwork, InputValuesTraining, OutputValuesTraining, epoch, batch, learningRate, debug);
            network.Predict(neuronNetwork, InputValuesPredict);

            sw.Stop();
            Console.WriteLine("Elapsed "+sw.ElapsedMilliseconds+" milliseconds");
        }
    }
}