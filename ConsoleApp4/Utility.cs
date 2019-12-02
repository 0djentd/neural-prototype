using System;

namespace ConsoleApp4
{
    internal class Utility
    {
        public static void ShowNeuronMap(NeuronLayer[] neuronLayers, bool showOutput)
        {
            Console.WriteLine("\n========= Neuron map ========\n");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                if (showOutput == true) x = neuronLayers.Length - 1;
                Console.Write("----- Layer [" + x + "] -----\n\n");
                for (int y = 0; y < neuronLayers[x].neuron.Count; y++)
                {
                    Console.Write("Neuron [" + y + "] ");
                    if (neuronLayers[x].neuron[y].Bias == true)
                    {
                        Console.Write("(bias neuron) ");
                    }
                    Console.Write("O=(" + Math.Round(neuronLayers[x].neuron[y].Output, 2) + ") ");
                    Console.Write("I=(" + Math.Round(neuronLayers[x].neuron[y].Input, 2) + ") ");
                    if (x < neuronLayers.Length - 1)
                    {
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.Write("Error=(" + Math.Round(neuronLayers[x].neuron[y].Error, 2) + ") \n");
                    }
                    if (x > 0 && neuronLayers[x].neuron[y].Bias == false)
                    {
                        //cycle for previous layer neurons 
                        for (int z = 0; z < neuronLayers[x - 1].neuron.Count; z++)
                        {
                            Console.Write("R[" + z + "]=(" + Math.Round(neuronLayers[x].neuron[y].RecivedInputFrom[z], 2) + ") ");
                            Console.Write("DeltaW [" + z + "]=(" + Math.Round((neuronLayers[x].neuron[y].W_From[z] - neuronLayers[x].neuron[y].Old_W_From[z]), 2) + ") ");
                            Console.Write("W[" + z + "]=(" + Math.Round(neuronLayers[x].neuron[y].W_From[z], 2) + ") ");
                            if (neuronLayers[x - 1].neuron[z].Bias == true)
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

        public static double ErrorSum(NeuronLayer[] layer)
        {
            double error = 0;
            for (int i = 0; i < layer[^1].neuron.Count; i++)
            {
                error += Math.Abs(layer[^1].neuron[i].DeltaE_wrt_Output);
            }
            return error;
        }

        public static void ShowResults(NeuronLayer[] neuronLayers)
        {
            Console.WriteLine("========================");
            for (int i = 0; i < neuronLayers[^1].neuron.Count; i++)
            {
                Console.Write("[" + i + "] V=" + Math.Round(neuronLayers[^1].neuron[i].Output, 2) + " E=" + Math.Round(neuronLayers[^1].neuron[i].DeltaE_wrt_Output, 2) + "\n");
            }
            Console.WriteLine("Sum of errors are " + Math.Round(ErrorSum(neuronLayers), 4));
            Console.WriteLine("========================");
        }

        public static void ClearInOutValues(NeuronLayer[] neuronLayers)
        {
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neuron.Count - neuronLayers[x].BiasNeurons; y++)
                {
                    neuronLayers[x].neuron[y].Output = 0;
                    neuronLayers[x].neuron[y].Input = 0;
                }
            }
        }

        public static void BackupSynapseValues(NeuronLayer[] neuronLayers)
        {
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neuron.Count - neuronLayers[x].BiasNeurons; y++) //??
                {
                    for (int z = 0; z < neuronLayers[x].neuron[y].W_From.Length; z++)
                    {
                        neuronLayers[x].neuron[y].Old_W_From[z] = neuronLayers[x].neuron[y].W_From[z];
                    }
                }
            }
        }
    }
}