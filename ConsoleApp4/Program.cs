using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    class Program
    {
        //random training input for testing
        private bool RandomInput = false;

        //number of layers
        private int layersNum;

        //number of neurones in each layer
        List<int> neuronNum = new List<int>();

        //Learning rate
        private double learningRate = 1;

        //training values
        private readonly double[,] InputValuesTraining = new double[4, 3] {
        { 0, 1, 1},
        { 1, 0, 0},
        { 1, 1, 1},
        { 0, 0, 0}
        };
        private readonly double[,] OutputValuesTraining = new double[4, 1] {
        { 0},
        { 1},
        { 1},
        { 0}
        };


        //creating objects and giving them some info about other objects
        public void Init()
        {
            /*Console.WriteLine("Enter layers number >2:");
            layersNum = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < layersNum; i++)
            {
                Console.WriteLine("Enter " + i + " layer neuron number :");
                neuronNum.Add(Convert.ToInt32(Console.ReadLine()));
            }*/


            //testing values

            layersNum = 3;
            neuronNum.Add(3);
            neuronNum.Add(32);
            neuronNum.Add(1);



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

            //Utility.ShowNeuronMap(neuronLayers);
            Console.WriteLine("\nEnter training counter:");
            Feedforward(neuronLayers, Convert.ToInt16(Console.ReadLine()));
            //Utility.ShowNeuronMap(neuronLayers);
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
            int countIn = 0;
            double[] inputValue = new double[neuronLayers[0].neurons.Count];
            double[] outputValue = new double[neuronLayers[neuronLayers.Length - 1].neurons.Count];
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
                    for (int h = 0; h < neuronLayers[0].neurons.Count; h++)
                    {
                        inputValue[h] = InputValuesTraining[countIn, h];
                    }

                }
                for (int v = 0; v < inputValue.GetLength(0); v++)
                {
                    neuronLayers[0].neurons[v].Value = inputValue[v];
                }
                // out Values
                if (RandomInput == false)
                {
                    for (int h = 0; h < neuronLayers[neuronLayers.Length - 1].neurons.Count; h++)
                    {
                        outputValue[h] = OutputValuesTraining[countIn, h];
                    }

                }

                //                      FEED
                //===================================================
                for (int x = 0; x < neuronLayers.Length; x++)
                {
                    for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                    {
                        //Console.WriteLine("[" + x + "][" + y + "]");
                        neuronLayers[x].neurons[y].Z();
                        if (x < neuronLayers.Length - 1) neuronLayers[x].neurons[y].Work(y);
                    }
                }
                //===================================================

                Utility.CopyValues(neuronLayers);
                Backpropagation(neuronLayers, outputValue);
                //Utility.ShowNeuronMap(neuronLayers);
                /*
                ErrorOut(neuronLayers, OutputValuesTraining);
                Learn(neuronLayers);
                Utility.ShowNeuronMap(neuronLayers, 2);*/
                Utility.ClearValues(neuronLayers);
                countIn++;
                if (countIn > 2) countIn = 0;
            }
        }

        public void Backpropagation(NeuronLayer[] neuronLayers, double[] correctOutput)
        {
            ErrorOut(neuronLayers, correctOutput);
        }

        public void ErrorOut(NeuronLayer[] neuronLayers, double[] correctOutput)
        {
            for (int i = 0; i < neuronLayers[^1].neurons.Count; i++)
            {
                neuronLayers[^1].neurons[i].E = (correctOutput[i] - neuronLayers[^1].neurons[i].Value);
                neuronLayers[^1].neurons[i].DeltaSum += Functions.SigmoidDerivative(neuronLayers[^1].neurons[i].Value) - neuronLayers[^1].neurons[i].E;
                for (int y = 0; y < neuronLayers[^2].neurons.Count; y++)
                {
                    neuronLayers[^1].neurons[i].WeightsFrom[y] = neuronLayers[^1].neurons[i].WeightsFrom[y] + ((neuronLayers[^1].neurons[i].DeltaSum / neuronLayers[^1].neurons[i].RecivedValueFrom[y]) * learningRate);
                    neuronLayers[^2].neurons[y].DeltaSum += neuronLayers[^1].neurons[i].DeltaSum / neuronLayers[^1].neurons[i].OldWeightsFrom[y] * Functions.SigmoidDerivative(neuronLayers[^2].neurons[i].Value);
                    for (int z = 0; z < neuronLayers[^3].neurons.Count; z++)
                    {
                        neuronLayers[^2].neurons[y].WeightsFrom[z] = neuronLayers[^2].neurons[y].WeightsFrom[z] + ((neuronLayers[^2].neurons[y].DeltaSum / neuronLayers[^2].neurons[y].RecivedValueFrom[z]) * learningRate);
                        neuronLayers[^3].neurons[z].DeltaSum += neuronLayers[^2].neurons[y].DeltaSum / neuronLayers[^2].neurons[y].OldWeightsFrom[z] * Functions.SigmoidDerivative(neuronLayers[^3].neurons[z].Value);
                    }
                }
            }
            Utility.ShowNeuronMap(neuronLayers, neuronLayers.Length - 1);
        }

        public void Error(NeuronLayer[] neuronLayers, double[] correctOutput)
        {
            for (int x = neuronLayers.Length - 1; x < 0; x--)
            {
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    neuronLayers[x].neurons[y].E = (correctOutput[y] - neuronLayers[x].neurons[y].Value);
                    neuronLayers[x].neurons[y].DeltaSum = Functions.SigmoidDerivative(neuronLayers[x].neurons[y].Value) - neuronLayers[x].neurons[y].E;
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
