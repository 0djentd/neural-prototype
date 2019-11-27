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
        private double[] InputValuesTraining = new double[3] { 1,0,0 };
        private double[] OutputValuesTraining = new double[2] { 1, 0 };


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


            //testing values
            /*
            layersNum = 3;
            neuronNum.Add(3);
            neuronNum.Add(4);
            neuronNum.Add(2);
            */

            //Initializing array of layers (which is arrays of neurones)
            NeuronLayer[] neuronLayers = new NeuronLayer[layersNum];
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                Console.WriteLine("\n--- Initializing " + x + " layer ---\n");
                neuronLayers[x] = new NeuronLayer();
                for (int y = 0; y < neuronNum[x]; y++)
                {
                    Console.WriteLine("[" + x + "][" + y + "] initializing neuron");
                    neuronLayers[x].neurons.Add(new Neuron());
                    neuronLayers[x].neurons[y].Bias = 0;

                    //applies "parent neurones" to all layers except first
                    if (x > 0)
                    {
                        //Console.WriteLine("[" + x + "][" + y + "]" + "Initializing parent neurons and weights");
                        neuronLayers[x].neurons[y].Parents = neuronLayers[x - 1].neurons.ToArray();
                        neuronLayers[x].neurons[y].Init();
                    }
                }

                //applies "target neurones" to all layers except last one 
                if (x < layersNum && x != 0)
                {
                    //Console.WriteLine("Initializing " + (x - 1) + " layer targetNeurons list");
                    for (int y = 0; y < neuronNum[x - 1]; y++)
                    {
                        //Console.WriteLine("[" + (x - 1) + "][" + y + "]" + "Initializing targetNeurons ");
                        neuronLayers[x - 1].neurons[y].TargetNeurons = neuronLayers[x].neurons.ToArray();
                    }
                }

            }

            Utility.ShowNeuronMap(neuronLayers);
            Console.WriteLine("\nEnter training counter:");
            Feedforward(neuronLayers, Convert.ToInt16(Console.ReadLine()));
            Utility.ShowNeuronMap(neuronLayers);
        }

        public void Feedforward(NeuronLayer[] neuronLayers, int count)
        {
            Console.WriteLine("Started feedforward");
            //int inputCycle = input.GetLength(0);
            double[] inputValueRandom = new double[neuronLayers[0].neurons.Count];
            if (RandomInput == true)
            {
                for (int h = 0; h < neuronLayers[0].neurons.Count; h++)
                {
                    inputValueRandom[h] = Utility.GetRandom();
                }
            }
            //input values
            double[] inputValue = new double[neuronLayers[0].neurons.Count];
            //double[] inputValue = Utility.Get0Dimension(input, inputCycle);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("\n==========");
                Console.WriteLine("Feed #" + i);
                Console.WriteLine("==========\n");
                if (RandomInput == true)
                {
                    for (int h = 0; h < neuronLayers[0].neurons.Count; h++)
                    {
                        inputValue[h] = inputValueRandom[h];
                    }
                }
                else
                {
                    //for testing purposes
                    inputValue[0] = 1;
                    inputValue[1] = 0;
                    inputValue[2] = 0;
                }
                for (int v = 0; v < inputValue.GetLength(0); v++)
                {
                    neuronLayers[0].neurons[v].Value = inputValue[v];
                }


                //                      FEED
                //===================================================
                for (int x = 0; x < neuronLayers.Length; x++)
                {
                    for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                    {
                        //Console.WriteLine("[" + x + "][" + y + "]");
                        if (x > 0) neuronLayers[x].neurons[y].zL();
                        if (x < neuronLayers.Length - 1) neuronLayers[x].neurons[y].Work(y);
                    }
                }
                //===================================================



                //Utility.ShowNeuronMap(neuronLayers);
                /*
                ErrorOut(neuronLayers, OutputValuesTraining);
                Learn(neuronLayers);
                Utility.ShowNeuronMap(neuronLayers, 2);*/
                Utility.ClearValues(neuronLayers);
            }
        }

        public void Learn(NeuronLayer[] neuronLayers)
        {
            Console.WriteLine("===== STARTED LEARNING =====");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    for (int z = 0; z < neuronLayers[x].neurons[y].Weights.Length; z++)
                    {
                        Console.Write("\n[" + x + "][" + y + "] W["+z+"] (" + Math.Round(neuronLayers[x].neurons[y].Weights[z], 4) + ") -> (");
                        neuronLayers[x].neurons[y].Weights[z] += neuronLayers[x].neurons[y].E;
                        Console.Write(Math.Round(neuronLayers[x].neurons[y].Weights[z], 4) + ")");
                    }
                }
            }
        }

        /*public void Learn(NeuronLayer[] neuronLayers, int layer)
        {
            Console.WriteLine("=====STARTED LEARNING!===== ["+layer+"]");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    for (int z = 0; z < neuronLayers[x].neurons[y].Weights.Length; z++)
                    {
                        Console.Write("\n[" + x + "][" + y + "] W[" + z + "] (" + Math.Round(neuronLayers[x].neurons[y].Weights[z], 2) + ") -> (");
                        neuronLayers[x].neurons[y].Weights[z] -= neuronLayers[x].neurons[y].E;
                        Console.Write(Math.Round(neuronLayers[x].neurons[y].Weights[z], 2) + ")");
                    }
                }
            }
        }*/

        public void Backpropagation(NeuronLayer[] neuronLayers, double[] correctOutput)
        {
            ErrorOut(neuronLayers, correctOutput);
        }

        public void ErrorOut(NeuronLayer[] neuronLayers, double[] correctOutput)
        {
            double[] errorOut = new double[neuronLayers[neuronLayers.Length - 1].neurons.Count];
            for (int i = 0; i< neuronLayers[neuronLayers.Length - 1].neurons.Count; i++)
            {
                errorOut[i] = Math.Pow((neuronLayers[neuronLayers.Length - 1].neurons[i].Value - correctOutput[i]), 2);
                neuronLayers[neuronLayers.Length - 1].neurons[i].E = errorOut[i];
            }
            Utility.ShowNeuronMap(neuronLayers, neuronLayers.Length - 1);
        }

        
        static void Main(string[] args)
        {
            Program Prog = new Program();
            Prog.Init();
        }
    }
}
