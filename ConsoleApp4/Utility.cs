using System;

namespace ConsoleApp4
{
    internal class Utility
    {
        public static void ShowNeuronMap(NeuronLayer[] neuronLayers, bool output)
        {
            Console.WriteLine("\n========= Neuron map ========\n");
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                if (output == true) x = neuronLayers.Length - 1;
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
                    if (x > 0 && neuronLayers[x].neuron[y].Bias==false)
                    {
                        for (int z = 0; z < neuronLayers[x - 1].neuron.Count; z++)
                        {
                            Console.Write("R[" + z + "]=(" + Math.Round(neuronLayers[x].neuron[y].RecivedInputFrom[z], 2) + ") ");
                            Console.Write("DeltaW [" + z + "]=(" + (neuronLayers[x].neuron[y].W_From[z] - neuronLayers[x].neuron[y].Old_W_From[z]) + ") ");
                            Console.Write("W[" + z + "]=(" + Math.Round(neuronLayers[x].neuron[y].W_From[z], 2) + ") ");
                            Console.Write("\n");
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
            for (int i = 0; i<layer[^1].neuron.Count; i++)
            {
                error += Math.Abs(layer[^1].neuron[i].Error);
            }
            return error;
        }

        public static Neuron[] ArrayShortener(Neuron[] neurons, int biases)
        {
            Neuron[] a = new Neuron[neurons.Length - biases];
            for (int i = 0; i< a.Length; i++)
            {
                a[i] = neurons[i];
                Console.WriteLine(a[i].Bias);
            }
            return a;
        }

        public static void ShowResults(NeuronLayer[] neuronLayers)
        {
            Console.WriteLine("========================");
            for (int i = 0; i < neuronLayers[^1].neuron.Count; i++)
            {
                Console.Write("[" + i + "] V=" + Math.Round(neuronLayers[^1].neuron[i].Output, 2) + " E=" + Math.Round(neuronLayers[^1].neuron[i].Error, 2) + "\n");
            }
            Console.WriteLine("Sum of errors are " + Math.Round(ErrorSum(neuronLayers), 4));
            Console.WriteLine("========================");
        }

        public static double GetRandom(double neurones)
        {
            Random random = new Random();
            double number = (random.NextDouble() * 2.0 - 1.0) * Math.Sqrt(2.0 / neurones);
            Console.WriteLine("Generated " + number + " (" + neurones + ")");
            return number;
        }
    }
}