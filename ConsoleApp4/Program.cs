﻿using System;

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
            int layersNum = 3;
            int[] neuronNum = { 3, 5, 2 };
            int batch = 0;
            double learningRate = 0.1;

            NeuronLayer[] neuronNetwork = network.Init(layersNum, neuronNum, true);
            neuronNetwork[0].SetFunctionsAll(0);
            neuronNetwork[1].SetFunctionsAll(2);
            neuronNetwork[2].SetFunctionsAll(2);
            
            double[,] InputValuesTraining = new double[4, 3] {
            { -1, 1, 1},
            { 1, 0, 1},
            { 1, 1, 1},
            { 0, 1, 1}
            };

            double[,] OutputValuesTraining = new double[4, 2] {
            { -1, -1},
            { 1, 0},
            { 1, 0},
            { 0, 0}
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
            network.Learn(neuronNetwork, InputValuesTraining, OutputValuesTraining, epoch, batch, learningRate);
            //Utility.ShowResults(neuronNetwork);
        }
    }
}