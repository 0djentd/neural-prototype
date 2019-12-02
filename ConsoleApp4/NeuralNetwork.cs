using System;

namespace ConsoleApp4
{
    public class NeuralNetwork
    {
        public void Backpropagation(NeuronLayer[] layer, double[] correctOutput, double learningRate)
        {
            CopyValues(layer);
            Gradient(layer, correctOutput);
            //ShowGradient(layer);
            Utility.ShowNeuronMap(layer, true);
            double correction = Correction(layer, learningRate);
            Console.WriteLine("total amoun of correction is "+correction);
            ClearGradient(layer);
        }
        public double Gradient(NeuronLayer[] layer, double[] correctOutput)
        {
            double total_Error = 0;
            for (int l = layer.Length-1; l>0;)
            {
                for (int j = 0; j < layer[l].neuron.Count- layer[l].BiasNeurons; j++)
                {
                    if (l == layer.Length - 1)
                    {
                        layer[l].neuron[j].Error = correctOutput[j] - layer[l].neuron[j].Output;
                        total_Error += 1/2*Math.Pow(layer[l].neuron[j].Error, 2);
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

        public double Correction(NeuronLayer[] layers, double learningRate)
        {
            double amount = 0;
            for (int j = layers.Length - 1; j > 0;)
            {
                for (int i = 0; i < layers[j].neuron.Count - layers[j].BiasNeurons; i++)
                {
                    for (int z = 0; z < layers[j - 1].neuron.Count; z++)
                    {
                        layers[j].neuron[i].W_From[z] -= (layers[j].neuron[i].DeltaE_wrt_W[z] * learningRate);
                        amount = layers[j].neuron[i].W_From[z] - layers[j].neuron[i].Old_W_From[z];
                    }
                }
                --j;
            }
            return amount;
        }


        public void ShowGradient(NeuronLayer[] layer)
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


        public void ClearGradient(NeuronLayer[] layers)
        {
            for (int j = layers.Length - 1; j >= 0; j--)
            {
                for (int i = 0; i < layers[j].neuron.Count; i++)
                {
                    //layers[j].neuron[i].Error = 0;
                    layers[j].neuron[i].Output = 0;
                    layers[j].neuron[i].Input = 0;
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

        public void Feedforward(NeuronLayer[] layer)
        {
            for (int x = 0; x < layer.Length; x++)
            {
                for (int y = 0; y < layer[x].neuron.Count; y++)
                {
                    //Console.WriteLine("Working " + x + " " + y);
                    /*if (x > 0)*/ layer[x].neuron[y].Act();
                    if (x < layer.Length - 1) layer[x].neuron[y].Work(y);
                }
            }
        }

        //creating objects and giving them some info about other objects
        public NeuronLayer[] Init(int layersNum, int[] neuronNum, bool bias)
        {
            //Initializing array of layers (which is arrays of neurones)
            NeuronLayer[] layer = new NeuronLayer[layersNum];
            for (int x = 0; x < layer.Length; x++)
            {
                //init layer, which is array of neurones
                layer[x] = new NeuronLayer();
                layer[x].LayerNumber = x;
                //init neurons
                for (int y = 0; y < neuronNum[x]; y++)
                {
                    //init neuron
                    layer[x].neuron.Add(new Neuron());
                    layer[x].neuron[y].LayerNumber = x;
                    layer[x].neuron[y].NeuronNumber = y;
                    layer[x].neuron[y].NeuralNetwork = layer;

                    //applies "parent neurones" to all layers except first and randomize synapses
                    if (x > 0)
                    {
                        layer[x].neuron[y].Parents = layer[x - 1].neuron.ToArray();
                    }
                }

                //add bias as a last element of layer
                if (bias == true && x < layer.Length - 1)
                {
                    layer[x].AddBias();
                }

                //applies "target neurones" to all layers except last one
                //note that this process is happening for previous layer to "x" layer because of object initialization process
                if (x < layersNum && x != 0)
                {
                    for (int y = 0; y < layer[x-1].neuron.Count; y++)
                    {
                        layer[x - 1].neuron[y].TargetNeurons = layer[x].neuron.ToArray();
                    }
                }

                
            }
            return layer;
        }

        public static void Randomize(NeuronLayer[] layer)
        {
            for (int l =1; l<layer.Length; l++)
            {
                for (int i = 0; i < layer[l].neuron.Count; i++)
                {
                    layer[l].neuron[i].Init();
                }
            }
        }

        public void Learn(NeuronLayer[] layer, double[,] inputData, double[,] outputData, int epoch, int batch, double learningRate)
        {
            int countIn = 0;
            double[] outputValue = new double[layer[^1].neuron.Count - layer[^1].BiasNeurons];
            for (int i = 0; i < epoch*inputData.GetLength(0); i++)
            {
                //countIn = 1;
                // input values
                if (i % 1 == 0)
                {
                    Console.WriteLine("\n\nFeed #" + i + "  exercise #" + countIn);
                }
                for (int h = 0; h < layer[0].neuron.Count-layer[0].BiasNeurons; h++)
                {
                    layer[0].neuron[h].Output = inputData[countIn, h];
                }
                for (int h = 0; h < layer[^1].neuron.Count- layer[^1].BiasNeurons; h++)
                {
                    // out Values
                    outputValue[h] = outputData[countIn, h];
                }
                Feedforward(layer);
                Backpropagation(layer, outputValue, learningRate);
                ClearValues(layer);
                if (countIn == inputData.GetLength(0)-1) countIn = 0;
                else countIn++;
            }
        }

        public void Predict(NeuronLayer[] layer, double[,] inputData)
        {
            int countIn = 0;
            for (int h = 0; h < layer[0].neuron.Count - layer[0].BiasNeurons; h++)
            {
                layer[0].neuron[h].Output = inputData[countIn, h];
            }
            Console.WriteLine("Feedforwarding new input data...");
            Feedforward(layer);
            Console.WriteLine("This is the neuron map for new input data");
            Utility.ShowNeuronMap(layer, false);
            ClearValues(layer);
        }

        public static void ClearValues(NeuronLayer[] neuronLayers)
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

        public static void CopyValues(NeuronLayer[] neuronLayers)
        {
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neuron.Count-neuronLayers[x].BiasNeurons; y++) //??
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