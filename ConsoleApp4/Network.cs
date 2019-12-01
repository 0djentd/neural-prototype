using System;

namespace ConsoleApp4
{
    internal class Network
    {
        public void Backpropagation(NeuronLayer[] layer, double[] correctOutput, double learningRate)
        {
            CopyValues(layer);
            Gradient(layer, correctOutput);
            //ShowGradient(layer);
            Utility.ShowNeuronMap(layer, true);
            Console.WriteLine("total amoun of correction is "+Correction(layer, learningRate));
            if (Utility.ErrorSum(layer) > 5)
            {
                Console.WriteLine("Need to reinitialize neurons.");
                //Randomize(layer);
            }
            ClearGradient(layer);
        }
        public void Gradient(NeuronLayer[] layer, double[] correctOutput)
        {
            for (int l = layer.Length-1; l>0;)
            {
                for (int j = 0; j < layer[l].neuron.Count; j++)
                {
                    if (l == layer.Length - 1)
                    {
                        layer[l].neuron[j].Error = correctOutput[j] - layer[l].neuron[j].Output;
                        layer[l].neuron[j].DeltaE_wrt_Output = correctOutput[j]- layer[l].neuron[j].Output;
                    }

                    layer[l].neuron[j].DeltaE_wrt_In = layer[l].neuron[j].Derivative() * layer[l].neuron[j].DeltaE_wrt_Output;

                    for (int i = 0; i < layer[l - 1].neuron.Count; i++)
                    {
                        //layer[l].neuron[j].DeltaE_wrt_W[i] = layer[l].neuron[j].DeltaE_wrt_In * layer[l].neuron[j].RecivedInputFrom[i];
                        layer[l].neuron[j].DeltaE_wrt_W[i] = layer[l].neuron[j].RecivedInputFrom[i] * layer[l].neuron[j].DeltaE_wrt_In;
                    }
                }
                for (int i = 0; i < layer[l - 1].neuron.Count; i++)
                {
                    for (int j = 0; j < layer[l].neuron.Count; j++)
                    {
                        layer[l - 1].neuron[i].DeltaE_wrt_Output += layer[l].neuron[j].W_From[i] * layer[l].neuron[j].DeltaE_wrt_In;
                    }
                }
                --l;
            }
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

        public double Correction(NeuronLayer[] layers, double learningRate)
        {
            double amount = 0;
            for (int j = layers.Length-1; j>0;)
            {
                for (int i = 0; i < layers[j].neuron.Count; i++)
                {
                    for (int z = 0; z < layers[j-1].neuron.Count; z++)
                    {
                        layers[j].neuron[i].W_From[z] += (layers[j].neuron[i].DeltaE_wrt_W[z]*learningRate);
                        amount = layers[j].neuron[i].W_From[z] - layers[j].neuron[i].Old_W_From[z];
                    }
                }
                --j;
            }
            return amount;
        }

        public void ClearGradient(NeuronLayer[] layers)
        {
            for (int j = layers.Length - 1; j > 0; j--)
            {
                for (int i = 0; i < layers[j].neuron.Count; i++)
                {
                    layers[j].neuron[i].Output = 0;
                    layers[j].neuron[i].Input = 0;
                    layers[j].neuron[i].DeltaE_wrt_Output = 0;
                    layers[j].neuron[i].DeltaE_wrt_In = 0;
                    for (int z = 0; z < layers[j - 1].neuron.Count; z++)
                    {
                        layers[j].neuron[i].Old_W_From[z] = 0;
                        layers[j].neuron[i].DeltaE_wrt_W[z] = 0;
                        layers[j].neuron[i].RecivedInputFrom[z] = 0;
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
                    /*if (x > 0)*/ layer[x].neuron[y].Act();
                    if (x < layer.Length - 1) layer[x].neuron[y].Work(y);
                }
            }
        }

        //creating objects and giving them some info about other objects
        public NeuronLayer[] Init(int layersNum, int[] neuronNum)
        {
            //Initializing array of layers (which is arrays of neurones)
            NeuronLayer[] layer = new NeuronLayer[layersNum];
            for (int x = 0; x < layer.Length; x++)
            {
                //init layer, which is array of neurones
                layer[x] = new NeuronLayer();
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
                    if (x == layer.Length - 1)
                    {
                        layer[x].neuron[y].Bias = 0;
                    }

                }

                //applies "target neurones" to all layers except last one
                //note that this process is happening for previous layer to "x" layer
                if (x < layersNum && x != 0)
                {
                    for (int y = 0; y < neuronNum[x - 1]; y++)
                    {
                        layer[x - 1].neuron[y].TargetNeurons = layer[x].neuron.ToArray();
                    }
                }
            }
            Randomize(layer);
            return layer;
        }

        public void Randomize(NeuronLayer[] layer)
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
            double[] outputValue = new double[layer[^1].neuron.Count];
            for (int i = 0; i < epoch*inputData.GetLength(0); i++)
            {
                //countIn = 0;
                // input values
                if (i % 1 == 0)
                {
                    Console.WriteLine("\n\nFeed #" + i + "  exercise #" + countIn);
                }
                for (int h = 0; h < layer[0].neuron.Count; h++)
                {
                    layer[0].neuron[h].Output = inputData[countIn, h];
                }
                for (int h = 0; h < layer[^1].neuron.Count; h++)
                {
                    // out Values
                    outputValue[h] = outputData[countIn, h];
                }
                Feedforward(layer);
                Backpropagation(layer, outputValue, learningRate);
                //if (Utility.ErrorSum(layer)>10/*| Utility.ErrorSum(layer)<0.05*/) i = count; 
                ClearValues(layer);
                if (countIn == inputData.GetLength(0)-1) countIn = 0;
                else countIn++;
            }
        }
        public static void ClearValues(NeuronLayer[] neuronLayers)
        {
            for (int x = 0; x < neuronLayers.Length; x++)
            {
                for (int y = 0; y < neuronLayers[x].neuron.Count; y++)
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
                for (int y = 0; y < neuronLayers[x].neuron.Count; y++)
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