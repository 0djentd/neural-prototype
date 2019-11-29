﻿using System;
namespace ConsoleApp4
{
    class Utility
    {

        public static void ShowNeuronMap(NeuronLayer[] neuronLayers)
        {
            Console.WriteLine("\n========= Neuron map ========\n");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                Console.Write("----- Layer [" + x + "] -----\n\n");
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    Console.Write("Neuron [" + y + "] ");
                    Console.Write("V=(" + Math.Round(neuronLayers[x].neurons[y].Value, 2) + ") ");
                    Console.Write("oE=(" + Math.Round(neuronLayers[x].neurons[y].OutE, 2) + ") ");
                    Console.Write("oD=(" + Math.Round(neuronLayers[x].neurons[y].OutD, 2) + ")\n");

                    if (x > 0) for (int z = 0; z < neuronLayers[x - 1].neurons.Count; z++)
                        {
                            Console.Write("E=(" + Math.Round(neuronLayers[x].neurons[y].Error[z], 2) + ") ");
                            Console.Write("D=(" + Math.Round(neuronLayers[x].neurons[y].Delta[z], 2) + ") ");
                            Console.Write("W[" + z + "]=(" + Math.Round(neuronLayers[x].neurons[y].WeightsFrom[z], 2) + ") ");
                            Console.Write("oldW[" + z + "]=(" + Math.Round(neuronLayers[x].neurons[y].OldWeightsFrom[z], 2) + ") \n");
                            //Console.Write("oW=(" + Math.Round(neuronLayers[x].neurons[y].Delta[z], 2) + ")\n");
                        }
                    Console.WriteLine();
                }
                Console.Write("\n");
            }
            Console.WriteLine();
        }

        public static void ShowNeuronMap(NeuronLayer[] neuronLayers, int layer)
        {
            //Console.WriteLine("\n========= Neuron map ========\n");
            //Console.Write("----- Layer [" + layer + "] -----\n\n");
            for (int y = 0; y < neuronLayers[layer].neurons.Count; y++)
            {
                Console.Write("Neuron [" + y + "] ");
                Console.Write("V=(" + Math.Round(neuronLayers[layer].neurons[y].Value, 2) + ") ");
                //Console.Write("B=(" + Math.Round(neuronLayers[layer].neurons[y].Bias, 2) + ") ");
                Console.Write("oE=(" + Math.Round(neuronLayers[layer].neurons[y].OutE, 4) + ") \n");
                if (layer > 0) for (int z = 0; z < neuronLayers[layer - 1].neurons.Count; z++)
                    {
                        //Console.Write("W[" + z + "]=(" + Math.Round(neuronLayers[layer].neurons[y].WeightsFrom[z], 2) + ") ");
                        //Console.Write("oW=(" + Math.Round(neuronLayers[layer].neurons[y].OldWeightsFrom[z], 2) + ")\n");
                    }
                Console.WriteLine();
            }
            Console.Write("\n\n");
        }

        public static void ClearValues(NeuronLayer[] neuronLayers)
        {
            //Console.WriteLine("Clearing values");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                //Console.Write("[" + x + "]");
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    //Console.Write(" (" + Math.Round(neuronLayers[x].neurons[y].Value, 2) + ")");
                    neuronLayers[x].neurons[y].Value = 0;
                }
                //Console.Write(" cleared\n");
            }
        }

        public static void CopyValues(NeuronLayer[] neuronLayers)
        {
            //Console.WriteLine("Copying values of weight to oldWeight");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neurons.Count; y++)
                {
                    neuronLayers[x].neurons[y].OldWeightsFrom = neuronLayers[x].neurons[y].WeightsFrom;
                }
            }
        }


        public static double GetRandom()
        {
            Random random = new Random();
            return (random.NextDouble() * 2 - 1);
        }
        public static double[] Get0Dimension(double[,] arrayName, int z)
        {
            double[] result = new double[arrayName.GetLength(1)];
            for (int i = 0; i < arrayName.GetLength(1); i++)
            {
                result[i] = arrayName[z, i];
            }
            return result;
        }
    }
}
