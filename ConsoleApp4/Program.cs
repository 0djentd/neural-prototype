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
        List<int> neuronNum = new List<int>();
        private Neuron[] InputLayer = new Neuron[12];
        private Neuron[] InnerLayer = new Neuron[12];
        private Neuron[] InnerLayer2 = new Neuron[12];
        private Neuron[] OutputLayer = new Neuron[6];
        //training values
        private double[,] InputValues = new double[4, 3]{
            {1,0,0},
            {1,1,0},
            {1,1,1},
            {0,0,0}
        };
        private double[,] OutputValues = new double[4, 1]
        {
            {0 },
            {1 },
            {1 },
            {0 }
        };
        private double[,] OutputValuesLearned = new double[4, 1]
        {
            {0 },
            {0 },
            {0 },
            {0 }
        };

        

        //creating objects and giving them some info about other objects
        public void Init()
        {
            Console.WriteLine("Enter layers number >2:");
            layersNum = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i<layersNum; i++)
            {
                Console.WriteLine("Enter " + i + " layer neuron number :");
                neuronNum.Add(Convert.ToInt32(Console.ReadLine())-1);
            }

            NeuronLayer[] neuronLayers = new NeuronLayer[layersNum];
            for (int x = 0; x<neuronLayers.Length; x++)
            {
                Console.WriteLine("Initializing " + x + " layer");
                neuronLayers[x] = new NeuronLayer();
                for (int y = 0; y <= neuronNum[x]; y++)
                {
                    Console.WriteLine("["+x+"]["+y+"]"+"Initializing neuron");
                    neuronLayers[x].neurons.Add(new Neuron());
                    
                    if (x > 0)
                    {
                        Console.WriteLine("[" + x + "][" + y + "]"+"Initializing parent neurons and weights");
                        neuronLayers[x].neurons[y].Parents = neuronLayers[x - 1].neurons.ToArray();
                        neuronLayers[x].neurons[y].Init();
                    }
                }
                
                if (x != layersNum && x != 0)
                {
                    Console.WriteLine("Initializing " + (x - 1) + " layer targetNeurons list");
                    for (int y = 0; y <= neuronNum[x-1]; y++)
                    {
                        Console.WriteLine("[" + (x-1) + "][" + y + "]" + "Initializing targetNeurons ");
                        neuronLayers[x - 1].neurons[y].TargetNeurons = neuronLayers[x].neurons.ToArray();
                    }                   
                }

            }

            Console.WriteLine("\nEnter training counter");
            Learn(Convert.ToInt16(Console.ReadLine()));
        }

        public void Learn(int count)
        {
            int inputValuesCounter = 0;
            Console.WriteLine("Training started");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("=============================================\nSetting new training example as input layers value (" + inputValuesCounter + ")");
                for (int x = 0; x < InputLayer.Length; x++)
                {
                    if (RandomInput == false)
                    {
                        //InputLayer[x].Value = Sigmoid(InputValues[inputValuesCounter, x]);
                        InputLayer[x].Value = InputValues[inputValuesCounter, x];
                    }
                    else
                    {
                        InputLayer[x].Value = Utility.GetRandom();
                    }

                }
                for (int x = 0; x < InputLayer.Length; x++)
                {
                    Console.WriteLine(InputLayer[x].Value);
                }
                if (RandomInput == false) inputValuesCounter++;
                Console.WriteLine("-------------------\nActivating input layer\n");
                for (int x = 0; x < InputLayer.Length; x++)
                {
                    InputLayer[x].Work(x);
                }
                Console.WriteLine("-------------------\nActivating inner layer\n");
                for (int x = 0; x < InnerLayer.Length; x++)
                {
                    InnerLayer[x].Value = Utility.Sigmoid(InnerLayer[x].Value + InnerLayer[x].Bias);
                    InnerLayer[x].Work(x);
                }
                Console.WriteLine("-------------------\nActivating second inner layer\n");
                for (int x = 0; x < InnerLayer2.Length; x++)
                {
                    InnerLayer2[x].Value = Utility.RelU(InnerLayer2[x].Value + InnerLayer2[x].Bias);
                    InnerLayer2[x].Work(x);
                }
                Console.WriteLine("-------------------\nActivating output layer\n");
                for (int x = 0; x < OutputLayer.Length; x++)
                {
                    Console.WriteLine("OUTPUT IS " + OutputLayer[x].Value + "\n");
                    if (RandomInput == false) OutputValuesLearned[inputValuesCounter - 1, x] = OutputLayer[x].Value;
                }
                Utility.Display2DArray(OutputValuesLearned);
                Utility.Display2DArray(OutputValues);
                if (inputValuesCounter == 4) inputValuesCounter = 0;
                ClearValues();
                Console.WriteLine("\nDone.\n");
            }
        }

        

        public void ClearValues()
        {
            Console.WriteLine("Clearing values of all neurons");
            for (int x = 0; x < InputLayer.Length; x++)
            {
                InputLayer[x].Value = 0;
            }
            for (int x = 0; x < InnerLayer.Length; x++)
            {
                InnerLayer[x].Value = 0;
            }
            for (int x = 0; x < InnerLayer2.Length; x++)
            {
                InnerLayer2[x].Value = 0;
            }
            for (int x = 0; x < OutputLayer.Length; x++)
            {
                OutputLayer[x].Value = 0;
            }
        }

        static void Main(string[] args)
        {
            Program Prog = new Program();
            Prog.Init();
        }
    }
}
