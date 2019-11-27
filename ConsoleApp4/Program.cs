﻿using System;

namespace ConsoleApp4
{
    class Program
    {
        //random training input for testing
        private bool RandomInput = true;
        //number of layers
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
            Console.WriteLine("\nCreating objects");
            for (int i = 0; i < OutputLayer.Length; i++)
            {
                OutputLayer[i] = new Neuron();
                Console.WriteLine(nameof(OutputLayer) + " neuron [" + i + "] " + +OutputLayer.Length);
            }
            for (int i = 0; i < InnerLayer2.Length; i++)
            {
                InnerLayer2[i] = new Neuron();
                InnerLayer2[i].TargetNeurons = OutputLayer;
                Console.WriteLine(nameof(InnerLayer2) + " neuron [" + i + "] " + OutputLayer.Length + " " + InnerLayer2[i].TargetNeurons.Length);
            }
            for (int i = 0; i < InnerLayer.Length; i++)
            {
                InnerLayer[i] = new Neuron();
                InnerLayer[i].TargetNeurons = InnerLayer2;
                Console.WriteLine(nameof(InnerLayer) + " neuron [" + i + "] " + InnerLayer2.Length + " " + InnerLayer[i].TargetNeurons.Length);
            }
            for (int i = 0; i < InputLayer.Length; i++)
            {
                InputLayer[i] = new Neuron();
                InputLayer[i].TargetNeurons = InnerLayer;
                Console.WriteLine(nameof(InputLayer) + " neuron [" + i + "] " + InnerLayer.Length + " " + InputLayer[i].TargetNeurons.Length);
            }

            Console.WriteLine("\nSetting parent neurons and initialising weights");
            for (int i = 0; i < OutputLayer.Length; i++)
            {
                Console.WriteLine(nameof(OutputLayer) + " neuron [" + i + "]");
                OutputLayer[i].Parents = InnerLayer2;
                OutputLayer[i].Init();
            }
            for (int i = 0; i < InnerLayer2.Length; i++)
            {
                Console.WriteLine(nameof(InnerLayer) + " neuron [" + i + "]");
                InnerLayer2[i].Parents = InnerLayer;
                InnerLayer2[i].Init();
            }
            for (int i = 0; i < InnerLayer.Length; i++)
            {
                Console.WriteLine(nameof(InnerLayer2) + " neuron [" + i + "]");
                InnerLayer[i].Parents = InputLayer;
                InnerLayer[i].Init();
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