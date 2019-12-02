using System;

namespace ConsoleApp4
{
    static class Gradient
    {
        public static double Calculate(NeuronLayer[] layer, double[] correctOutput)
        {
            double total_Error = 0;
            for (int l = layer.Length - 1; l > 0;)
            {
                for (int j = 0; j < layer[l].neuron.Count - layer[l].BiasNeurons; j++)
                {
                    if (l == layer.Length - 1)
                    {
                        layer[l].neuron[j].Error = correctOutput[j] - layer[l].neuron[j].Output;
                        total_Error += 1 / 2 * Math.Pow(layer[l].neuron[j].Error, 2);
                        layer[l].neuron[j].DeltaE_wrt_Output = layer[l].neuron[j].Output - correctOutput[j];
                    }

                    layer[l].neuron[j].DeltaE_wrt_In = layer[l].neuron[j].Derivative();

                    for (int i = 0; i < layer[l - 1].neuron.Count; i++)
                    {
                        layer[l].neuron[j].DeltaE_wrt_W[i] = layer[l].neuron[j].RecivedInputFrom[i] * layer[l].neuron[j].DeltaE_wrt_In * layer[l].neuron[j].DeltaE_wrt_Output;
                    }
                }

                for (int i = 0; i < layer[l - 1].neuron.Count; i++)
                {
                    for (int j = 0; j < layer[l].neuron.Count - layer[l].BiasNeurons; j++)
                    {
                        layer[l - 1].neuron[i].DeltaE_wrt_Output += layer[l].neuron[j].W_From[i] * layer[l].neuron[j].DeltaE_wrt_Output * layer[l].neuron[j].DeltaE_wrt_In;
                    }
                }
                --l;
            }
            return total_Error;
        }


        public static void Show(NeuronLayer[] layer)
        {
            for (int l = layer.Length - 1; l > 0;)
            {
                Console.WriteLine(l + " layer backpropagation");
                for (int j = 0; j < layer[l].neuron.Count; j++)
                {
                    if (l == layer.Length - 1)
                    {
                        Console.WriteLine("[" + j + "] Error " + layer[l].neuron[j].Error);
                    }
                    Console.WriteLine("[" + j + "] DeltaE wrt Output " + layer[l].neuron[j].DeltaE_wrt_Output);
                    Console.WriteLine("[" + j + "] DeltaE wrt In " + layer[l].neuron[j].DeltaE_wrt_In);
                    for (int i = 0; i < layer[l - 1].neuron.Count; i++)
                    {
                        Console.WriteLine("[" + j + "]<-[" + i + "] Recived input " + layer[l].neuron[j].RecivedInputFrom[i]);
                        Console.WriteLine("[" + j + "]->[" + i + "] DeltaE wrt W " + layer[l].neuron[j].DeltaE_wrt_W[i]);
                    }
                }
                --l;
            }
        }


        public static void Clear(NeuronLayer[] layers)
        {
            for (int j = layers.Length - 1; j >= 0; j--)
            {
                for (int i = 0; i < layers[j].neuron.Count; i++)
                {
                    layers[j].neuron[i].DeltaE_wrt_Output = 0;
                    layers[j].neuron[i].DeltaE_wrt_In = 0;
                    if (j > 0)
                    {
                        for (int z = 0; z < layers[j - 1].neuron.Count; z++)
                        {
                            layers[j].neuron[i].Old_W_From[z] = 0;
                            layers[j].neuron[i].DeltaE_wrt_W[z] = 0;
                            layers[j].neuron[i].RecivedInputFrom[z] = 0;
                        }
                    }
                }
            }
        }
    }
}