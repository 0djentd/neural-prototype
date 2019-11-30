using System;

namespace ConsoleApp4
{
    internal class Utility
    {
        public static void ShowNeuronMap(NeuronLayer[] neuronLayers)
        {
            Console.WriteLine("\n========= Neuron map ========\n");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                Console.Write("----- Layer [" + x + "] -----\n\n");
                for (int y = 0; y < neuronLayers[x].neuron.Count; y++)
                {
                    Console.Write("Neuron [" + y + "] ");
                    Console.Write("V=(" + Math.Round(neuronLayers[x].neuron[y].Value, 2) + ") ");
                    if (x < neuronLayers.Length - 1)
                    {
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.Write("oE=(" + Math.Round(neuronLayers[x].neuron[y].OutE, 2) + ") ");
                        Console.Write("oD=(" + Math.Round(neuronLayers[x].neuron[y].OutD, 2) + ")\n");
                    }
                    if (x > 0)
                    {
                        for (int z = 0; z < neuronLayers[x - 1].neuron.Count; z++)
                        {
                            Console.Write("E=(" + Math.Round(neuronLayers[x].neuron[y].Error[z], 4) + ") ");
                            Console.Write("D=(" + Math.Round(neuronLayers[x].neuron[y].Delta[z], 4) + ") ");
                            Console.Write("R[" + z + "]=(" + Math.Round(neuronLayers[x].neuron[y].RecivedValueFrom[z], 2) + ") ");
                            Console.Write("DeltaW [" + z + "]=(" + (neuronLayers[x].neuron[y].WeightsFrom[z] - neuronLayers[x].neuron[y].OldWeightsFrom[z]) + ") ");
                            Console.Write("W[" + z + "]=(" + Math.Round(neuronLayers[x].neuron[y].WeightsFrom[z], 2) + ") ");
                            Console.Write("\n");
                        }
                    }
                    Console.WriteLine();
                }
                Console.Write("\n");
            }
            Console.WriteLine();
        }

        public static void ShowResults(NeuronLayer[] neuronLayers)
        {
            double errorSum = 0;
            Console.WriteLine("========================");
            for (int i = 0; i < neuronLayers[^1].neuron.Count; i++)
            {
                errorSum += neuronLayers[^1].neuron[i].OutE;
                Console.Write("[" + i + "] V=" + Math.Round(neuronLayers[^1].neuron[i].Value, 2) + " E=" + Math.Round(neuronLayers[^1].neuron[i].OutE, 2) + "\n");
            }
            Console.WriteLine("Sum of errors are " + Math.Round(errorSum, 4));
            Console.WriteLine("========================");
        }

        public static void ClearValues(NeuronLayer[] neuronLayers)
        {
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neuron.Count; y++)
                {
                    neuronLayers[x].neuron[y].Value = 0;
                    neuronLayers[x].neuron[y].OutD = 0;
                    neuronLayers[x].neuron[y].OutE = 0;
                }
            }
        }

        public static void CopyValues(NeuronLayer[] neuronLayers)
        {
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neuron.Count; y++)
                {
                    neuronLayers[x].neuron[y].OldWeightsFrom = neuronLayers[x].neuron[y].WeightsFrom;
                }
            }
        }

        public static double GetRandom()
        {
            Random random = new Random();
            return (random.NextDouble() * 1.8 - 1);
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