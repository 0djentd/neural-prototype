﻿using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    class Program
    {
        //random training input for testing
        private bool RandomInput = true;
        //number of layers
        private int layersNum;
        //number of neurones in each layer
        List<int> neuronNum = new List<int>();
        //training values
        private double[,] InputValuesTraining = new double[4, 3]{
            {1,0,0},
            {1,1,0},
            {1,1,1},
            {0,0,0}
        };
        private double[,] OutputValuesTraining = new double[4, 1]
        {
            {0 },
            {1 },
            {1 },
            {0 }
        };


        //creating objects and giving them some info about other objects
        public void Init()
        {
            Console.WriteLine("Enter layers number >2:");
            layersNum = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < layersNum; i++)
            {
                Console.WriteLine("Enter " + i + " layer neuron number :");
                neuronNum.Add(Convert.ToInt32(Console.ReadLine()));
            }

            //array of layers (which is arrays of neurones)
            NeuronLayer[] neuronLayers = new NeuronLayer[layersNum];
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                Console.WriteLine("Initializing " + x + " layer");
                neuronLayers[x] = new NeuronLayer();
                for (int y = 0; y < neuronNum[x]; y++)
                {
                    Console.WriteLine("[" + x + "][" + y + "]" + "Initializing neuron");
                    neuronLayers[x].neurons.Add(new Neuron());
                    neuronLayers[x].neurons[y].Bias = Utility.GetRandom();
                    //applies "parent neurones" to all layers except first
                    if (x > 0)
                    {
                        Console.WriteLine("[" + x + "][" + y + "]" + "Initializing parent neurons and weights");
                        neuronLayers[x].neurons[y].Parents = neuronLayers[x - 1].neurons.ToArray();
                        neuronLayers[x].neurons[y].Init();
                    }
                }

                //applies "target neurones" to all layers except last one 
                if (x != layersNum && x != 0)
                {
                    Console.WriteLine("Initializing " + (x - 1) + " layer targetNeurons list");
                    for (int y = 0; y < neuronNum[x - 1]; y++)
                    {
                        Console.WriteLine("[" + (x - 1) + "][" + y + "]" + "Initializing targetNeurons ");
                        neuronLayers[x - 1].neurons[y].TargetNeurons = neuronLayers[x].neurons.ToArray();
                    }
                }

            }

            Console.WriteLine("\nEnter training counter");
            Feedforward(neuronLayers, InputValuesTraining, Convert.ToInt16(Console.ReadLine()));
        }

        public void Feedforward(NeuronLayer[] neuronLayers, double[,] input, int count)
        {
            Console.WriteLine("Started feedforward");
            int inputCycle = input.GetLength(0);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("\nFeed #" + i + "\n");

                //input values
                double[] inputValue = new double[neuronLayers[0].neurons.Count];
                //double[] inputValue = Utility.Get0Dimension(input, inputCycle);
                if (RandomInput == true)
                {
                    for (int h = 0; h < neuronLayers[0].neurons.Count; h++)
                    {
                        inputValue[h] = Utility.GetRandom();
                    }
                }
                else
                {
                    //for testing purposes
                    inputValue[0] = 0.1;
                    inputValue[1]=0.4;
                    inputValue[1] = -0.3;
                }
                for (int v = 0; v < inputValue.GetLength(0); v++)
                {
                    neuronLayers[0].neurons[v].Value = inputValue[v];
                }


                //feed
                for (int x = 0; x < neuronLayers.Length; x++)
                {
                    for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                    {
                        Console.WriteLine("[" + x + "][" + y + "]");
                        if (x < neuronLayers.Length - 1) neuronLayers[x].neurons[y].Work(y);
                    }
                }
                ClearValues(neuronLayers);
            }
        }

        public void ClearValues(NeuronLayer[] neuronLayers)
        {
            Console.WriteLine("Clearing values");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                Console.Write("[" + x + "]");
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    Console.Write(" (" + Math.Round(neuronLayers[x].neurons[y].Value, 2)+")");
                    neuronLayers[x].neurons[y].Value = 0;
                }
                Console.Write(" cleared\n");
            }
        }

        static void Main(string[] args)
        {
            Program Prog = new Program();
            Prog.Init();
        }
    }
}
