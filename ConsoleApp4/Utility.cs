using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    class Utility
    {
        public static void ShowNeuronMap(List<Layer> layer, bool showOutput)
        {
            Console.WriteLine("\n========= Neuron map ========\n");
            for (int x = 0; x < layer.Count; x++)
            {
                if (showOutput == true) x = layer.Count - 1;
                Console.Write("----- Layer [" + x + "] -----\n\n");
                for (int y = 0; y < layer[x].neuron.Count; y++)
                {
                    Console.Write("Neuron [" + y + "] ");
                    if (layer[x].neuron[y].Bias == true)
                    {
                        Console.Write("(bias neuron) ");
                    }
                    Console.Write("O=(" + Math.Round(layer[x].neuron[y].Output, 2) + ") ");
                    Console.Write("I=(" + Math.Round(layer[x].neuron[y].Input, 2) + ") ");
                    if (x < layer.Count - 1)
                    {
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.Write("Error=(" + Math.Round(layer[x].neuron[y].Error, 2) + ") \n");
                    }
                    if (x > 0 && layer[x].neuron[y].Bias == false)
                    {
                        //cycle for previous layer neurons 
                        for (int z = 0; z < layer[x - 1].neuron.Count; z++)
                        {
                            Console.Write("R[" + z + "]=(" + Math.Round(layer[x].neuron[y].RecivedInputFrom[z], 2) + ") ");
                            Console.Write("DeltaW [" + z + "]=(" + Math.Round((layer[x].neuron[y].W_From[z] - layer[x].neuron[y].Old_W_From[z]), 2) + ") ");
                            Console.Write("W[" + z + "]=(" + Math.Round(layer[x].neuron[y].W_From[z], 2) + ") ");
                            if (layer[x - 1].neuron[z].Bias == true)
                            {
                                Console.Write(" (bias neuron) \n");
                            }
                            else Console.Write("\n");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static double ErrorSum(List<Layer> layer)
        {
            double error = 0;
            for (int i = 0; i < layer[^1].neuron.Count; i++)
            {
                error += Math.Abs(layer[^1].neuron[i].DeltaE_wrt_Output);
            }
            return error;
        }

        public static void ShowResults(List<Layer> layer)
        {
            Console.WriteLine("========================");
            for (int i = 0; i < layer[^1].neuron.Count; i++)
            {
                Console.Write("[" + i + "] V=" + Math.Round(layer[^1].neuron[i].Output, 2) + " E=" + Math.Round(layer[^1].neuron[i].DeltaE_wrt_Output, 2) + "\n");
            }
            Console.WriteLine("Sum of errors are " + Math.Round(ErrorSum(layer), 4));
            Console.WriteLine("========================");
        }

        public static void ClearInOutValues(List<Layer> layer)
        {
            for (int x = 0; x < layer.Count; x++)
            {
                for (int y = 0; y < layer[x].neuron.Count - layer[x].BiasNeurons; y++)
                {
                    layer[x].neuron[y].Output = 0;
                    layer[x].neuron[y].Input = 0;
                }
            }
        }

        public static void BackupSynapseValues(List<Layer> layer)
        {
            for (int x = 0; x < layer.Count; x++)
            {
                for (int y = 0; y < layer[x].neuron.Count - layer[x].BiasNeurons; y++) //??
                {
                    for (int z = 0; z < layer[x].neuron[y].W_From.Length; z++)
                    {
                        layer[x].neuron[y].Old_W_From[z] = layer[x].neuron[y].W_From[z];
                    }
                }
            }
        }
    }
}