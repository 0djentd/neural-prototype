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
        { 1, 0, 1},
        { 1, 1, 1},
        { 0, 0, 0}
        };
        private readonly double[,] OutputValuesTraining = new double[4, 3] {
        { 0, 0, 1},
        { 1, 1, 0},
        { 0, 0, 1},
        { 0, 1, 0}
        };


        //creating objects and giving them some info about other objects
        public NeuronLayer[] Init()
        {
            layersNum = 3;
            neuronNum.Add(3);
            neuronNum.Add(3);
            neuronNum.Add(3);

            //Initializing array of layers (which is arrays of neurones)
            NeuronLayer[] neuronLayers = new NeuronLayer[layersNum];
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                //Console.WriteLine("\n--- Initializing " + x + " layer ---\n");
                neuronLayers[x] = new NeuronLayer();
                for (int y = 0; y < neuronNum[x]; y++)
                {
                    //Console.WriteLine("[" + x + "][" + y + "] initializing neuron");
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
            //Utility.ShowNeuronMap(neuronLayers);
            return neuronLayers;
        }

        public void Feedforward(NeuronLayer[] neuronLayers, int count)
        {
            //Console.WriteLine("Started feedforward");
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
                countIn = 1;
                /*
                Console.WriteLine("\n==========");
                Console.WriteLine("Feed #" + i);
                Console.WriteLine("==========\n");
                Console.WriteLine("Exercise #" + countIn + "\n");
                */            
                // in values
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
                // set in values as vaues of 1st layer
                for (int v = 0; v < inputValue.GetLength(0); v++)
                {
                    neuronLayers[0].neurons[v].Value = inputValue[v];
                }
                // out Values
                if (RandomInput == false)
                {
                    for (int h = 0; h < neuronLayers[^1].neurons.Count; h++)
                    {
                        outputValue[h] = OutputValuesTraining[countIn, h];
                    }

                }
                Feed(neuronLayers);
                //Utility.ShowNeuronMap(neuronLayers);
                //Utility.ShowNeuronMap(neuronLayers, 2);
                //Utility.ShowNeuronMap(neuronLayers);
                Backpropagation(neuronLayers, outputValue, i);

                countIn++;
                if (countIn > 3) countIn = 0;
            }
        }

        public void Feed(NeuronLayer[] neuronLayers)
        {
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    //Console.WriteLine("[" + x + "][" + y + "]");
                    if (x > 0) neuronLayers[x].neurons[y].Z();
                    if (x < neuronLayers.Length - 1) neuronLayers[x].neurons[y].Work(y);
                }
            }
        }

        public void Backpropagation(NeuronLayer[] neuronLayers, double[] correctOutput, int k)
        {
            //Console.WriteLine("Backprop start.");
            //Console.WriteLine("started backpropagation");
            Utility.CopyValues(neuronLayers);
            for (int i = 0; i < neuronLayers[^1].neurons.Count; i++)
            {
                //Console.WriteLine("out.");
                neuronLayers[^1].neurons[i].OutE = correctOutput[i] - neuronLayers[^1].neurons[i].Value;
                neuronLayers[^1].neurons[i].OutD = Functions.SigmoidDerivative(neuronLayers[^1].neurons[i].Value) * neuronLayers[^1].neurons[i].OutE;
                for (int y = 0; y < neuronLayers[^2].neurons.Count; y++)
                {
                    //Console.WriteLine("hidden.");
                    neuronLayers[^1].neurons[i].Error[y] = (neuronLayers[^1].neurons[i].RecivedValueFrom[y] * neuronLayers[^1].neurons[i].OutD);
                    neuronLayers[^1].neurons[i].Delta[y] = neuronLayers[^1].neurons[i].Error[y] * Functions.SigmoidDerivative(neuronLayers[^1].neurons[i].RecivedValueFrom[y]);
                    neuronLayers[^1].neurons[i].WeightsFrom[y] += neuronLayers[^1].neurons[i].RecivedValueFrom[y] * neuronLayers[^1].neurons[i].OutD * learningRate;
                    DeepLearning(neuronLayers, neuronLayers.Length-2, y);
                }
            }
            //Utility.ShowNeuronMap(neuronLayers, neuronLayers.Length-1);

           Utility.ShowNeuronMap(neuronLayers);
            Utility.ClearValues(neuronLayers);
            //Console.WriteLine("Backprop done.");
        }

        public void DeepLearning(NeuronLayer[] neuronLayers, int l, int i)
        {
            for (int y = 0; y < neuronLayers[l-1].neurons.Count; y++)
            {
                //Console.WriteLine("deep.");
                neuronLayers[l].neurons[i].Error[y] = (neuronLayers[l].neurons[i].RecivedValueFrom[y] * neuronLayers[l].neurons[i].Delta[y]);
                neuronLayers[l].neurons[i].Delta[y] = neuronLayers[l].neurons[i].Error[y] * Functions.SigmoidDerivative(neuronLayers[l].neurons[i].RecivedValueFrom[y]);
                neuronLayers[l].neurons[i].WeightsFrom[y] += neuronLayers[l].neurons[i].RecivedValueFrom[y] * neuronLayers[l].neurons[i].Delta[y] * learningRate;
                if (l>1) DeepLearning(neuronLayers, l-1, y);
            }
        }

        static void Main(string[] args)
        {
            Program Prog = new Program();
            NeuronLayer[] neuronNetwork = Prog.Init();
            Console.WriteLine("\nEnter training counter:");
            Prog.Feedforward(neuronNetwork, Convert.ToInt32(Console.ReadLine()));
            Utility.ShowNeuronMap(neuronNetwork);
        }
    }
}
