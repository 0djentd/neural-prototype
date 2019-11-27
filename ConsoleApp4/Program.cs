using System;
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

            //array of layers (which is array of neurones)
            NeuronLayer[] neuronLayers = new NeuronLayer[layersNum];
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                Console.WriteLine("Initializing " + x + " layer");
                neuronLayers[x] = new NeuronLayer();
                for (int y = 0; y < neuronNum[x]; y++)
                {
                    Console.WriteLine("[" + x + "][" + y + "]" + "Initializing neuron");
                    neuronLayers[x].neurons.Add(new Neuron());

                    if (x > 0)
                    {
                        Console.WriteLine("[" + x + "][" + y + "]" + "Initializing parent neurons and weights");
                        neuronLayers[x].neurons[y].Parents = neuronLayers[x - 1].neurons.ToArray();
                        neuronLayers[x].neurons[y].Init();
                    }
                }

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
                Console.WriteLine("\nFeed #" + i);
                //double[] inputValue = Utility.Get0Dimension(input, inputCycle);
                double[] inputValue = new double[] { 1, -0.4, 0.3 };
                for (int v = 0; v < inputValue.GetLength(0); v++)
                {
                    neuronLayers[0].neurons[v].Value = inputValue[v];
                }

                for (int x = 0; x < neuronLayers.Length; x++)
                {
                    for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                    {
                        if (x < neuronLayers.Length - 1) neuronLayers[x].neurons[y].Work(y);
                        Console.WriteLine("[" + x + "][" + y + "]");
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
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    Console.WriteLine("[" + x + "][" + y + "] Value " + Math.Round(neuronLayers[x].neurons[y].Value, 4) + " cleared");
                    neuronLayers[x].neurons[y].Value = 0;
                }
            }
        }

        static void Main(string[] args)
        {
            Program Prog = new Program();
            Prog.Init();
        }
    }
}
