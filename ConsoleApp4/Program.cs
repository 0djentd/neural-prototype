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

            int layersNum = 4;
            int[] neuronNum = { 3, 5, 4, 1 };
            int batch = 0;
            double learningRate = 0.3;
            bool biased = true;
            Network network = new Network(layersNum, neuronNum, biased);

            Utility.ShowNeuronMap(network.layer, false);
            network.Randomize();

            network.layer[0].SetFunctionsAll(2);
            network.layer[1].SetFunctionsAll(2);
            network.layer[2].SetFunctionsAll(2);
            network.layer[3].SetFunctionsAll(2);


            double[,] InputValuesTraining = new double[4, 3] {
            { 0, 1, 1},
            { 1, 1, 0},
            { 1, 0, 1},
            { 0, 1, 1}
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
            
            network.Learn(InputValuesTraining, OutputValuesTraining, epoch, batch, learningRate, debug);
            network.Predict(InputValuesPredict);

            sw.Stop();
            Console.WriteLine("Elapsed "+sw.ElapsedMilliseconds+" milliseconds");
        }
    }
}