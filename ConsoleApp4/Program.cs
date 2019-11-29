using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    internal class Program
    {
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

        //number of layers
        private int layersNum;

        //Learning rate
        private double learningRate = 1;

        //number of neurones in each layer
        private List<int> neuronNum = new List<int>();

        //random training input for testing
        private bool RandomInput = false;

        public void Backpropagation(NeuronLayer[] nL, double[] correctOutput, int k)
        {
            Utility.CopyValues(nL);
            for (int i = 0; i < nL[^1].n.Count; i++)
            {
                nL[^1].n[i].OutE = correctOutput[i] - nL[^1].n[i].Value;
                nL[^1].n[i].OutD = Functions.SigmoidDerivative(nL[^1].n[i].Value) * nL[^1].n[i].OutE;
                for (int y = 0; y < nL[^2].n.Count; y++)
                {
                    nL[^1].n[i].Error[y] = (nL[^1].n[i].RecivedValueFrom[y] * nL[^1].n[i].OutD);
                    nL[^1].n[i].Delta[y] = nL[^1].n[i].Error[y] * Functions.SigmoidDerivative(nL[^1].n[i].RecivedValueFrom[y]);
                    nL[^1].n[i].WeightsFrom[y] += nL[^1].n[i].RecivedValueFrom[y] * nL[^1].n[i].OutD * learningRate;
                    //DeepLearning(nL, nL.Length - 2, y);
                }
            }
            Utility.ShowNeuronMap(nL);
            Utility.ClearValues(nL);
            Console.WriteLine("Backprop done.");
        }

        public void DeepLearning(NeuronLayer[] nL, int l, int i)
        {
            for (int y = 0; y < nL[l - 1].n.Count; y++)
            {
                Console.WriteLine("deep.");
                nL[l].n[i].Error[y] = (nL[l].n[i].RecivedValueFrom[y] * nL[l].n[i].Delta[y]);
                nL[l].n[i].Delta[y] = nL[l].n[i].Error[y] * Functions.SigmoidDerivative(nL[l].n[i].RecivedValueFrom[y]);
                nL[l].n[i].WeightsFrom[y] += nL[l].n[i].RecivedValueFrom[y] * nL[l].n[i].Delta[y] * learningRate;
                if (l > 1) DeepLearning(nL, l - 1, y);
            }
        }

        public void Feed(NeuronLayer[] nL)
        {
            for (int x = 0; x < nL.Length; x++)
            {
                for (int y = 0; y < nL[x].n.Count; y++)
                {
                    //Console.WriteLine("[" + x + "][" + y + "]");
                    if (x < nL.Length)
                    nL[x].n[y].Z();
                    if (x < nL.Length - 1) nL[x].n[y].Work(y);
                }
            }
        }

        public void Feedforward(NeuronLayer[] nL, int count)
        {
            //Console.WriteLine("Started feedforward");
            double[] inputValueRandom = new double[nL[0].n.Count];

            if (RandomInput == true)
            {
                for (int h = 0; h < nL[0].n.Count; h++)
                {
                    inputValueRandom[h] = Utility.GetRandom();
                }
            }
            int countIn = 0;
            double[] inputValue = new double[nL[0].n.Count];
            double[] outputValue = new double[nL[^1].n.Count];
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
                    for (int h = 0; h < nL[0].n.Count; h++)
                    {
                        inputValue[h] = inputValueRandom[h];
                    }
                }
                else
                {
                    for (int h = 0; h < nL[0].n.Count; h++)
                    {
                        inputValue[h] = InputValuesTraining[countIn, h];
                    }
                }
                // set in values as vaues of 1st layer
                for (int v = 0; v < inputValue.GetLength(0); v++)
                {
                    nL[0].n[v].Value = inputValue[v];
                }
                // out Values
                if (RandomInput == false)
                {
                    for (int h = 0; h < nL[^1].n.Count; h++)
                    {
                        outputValue[h] = OutputValuesTraining[countIn, h];
                    }
                }
                Feed(nL);
                //Utility.ShowNeuronMap(nL);
                //Utility.ShowNeuronMap(nL, 2);
                //Utility.ShowNeuronMap(nL);
                Backpropagation(nL, outputValue, i);

                countIn++;
                if (countIn > 3) countIn = 0;
            }
        }

        //creating objects and giving them some info about other objects
        public NeuronLayer[] Init()
        {
            layersNum = 3;
            neuronNum.Add(3);
            neuronNum.Add(3);
            neuronNum.Add(3);

            //Initializing array of layers (which is arrays of neurones)
            //nL = neuronLayer; n = neuron
            NeuronLayer[] nL = new NeuronLayer[layersNum];
            for (int x = 0; x < nL.Length; x++)
            {
                //Console.WriteLine("\n--- Initializing " + x + " layer ---\n");
                nL[x] = new NeuronLayer();
                for (int y = 0; y < neuronNum[x]; y++)
                {
                    //Console.WriteLine("[" + x + "][" + y + "] initializing neuron");
                    nL[x].n.Add(new Neuron());
                    nL[x].n[y].Bias = 0;
                    //applies "parent neurones" to all layers except first
                    if (x > 0)
                    {
                        //Console.WriteLine("[" + x + "][" + y + "]" + "Initializing parent neurons and weights");
                        nL[x].n[y].Parents = nL[x - 1].n.ToArray();
                        nL[x].n[y].Init();
                    }
                }

                //applies "target neurones" to all layers except last one
                if (x < layersNum && x != 0)
                {
                    //Console.WriteLine("Initializing " + (x - 1) + " layer targetNeurons list");
                    for (int y = 0; y < neuronNum[x - 1]; y++)
                    {
                        //Console.WriteLine("[" + (x - 1) + "][" + y + "]" + "Initializing targetNeurons ");
                        nL[x - 1].n[y].TargetNeurons = nL[x].n.ToArray();
                    }
                }
            }

            //Utility.ShowNeuronMap(nL);
            //Utility.ShowNeuronMap(nL);
            return nL;
        }

        private static void Main()
        {
            Program Prog = new Program();
            NeuronLayer[] neuronNetwork = Prog.Init();
            Console.WriteLine("\nEnter training counter:");
            Prog.Feedforward(neuronNetwork, Convert.ToInt32(Console.ReadLine()));
            Utility.ShowNeuronMap(neuronNetwork);
        }
    }
}