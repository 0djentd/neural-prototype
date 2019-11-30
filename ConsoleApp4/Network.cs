using System;

namespace ConsoleApp4
{
    internal class Network
    {
        public void Backpropagation(NeuronLayer[] layer, double[] correctOutput, Functions functions, double learningRate)
        {
            Utility.CopyValues(layer);
            for (int i = 0; i < layer[^1].neuron.Count; i++)
            {
                layer[^1].neuron[i].OutE = correctOutput[i] - layer[^1].neuron[i].Value;
                layer[^1].neuron[i].OutD = layer[^1].neuron[i].OutE * layer[^1].neuron[i].Derivative();
                for (int y = 0; y < layer[^2].neuron.Count; y++)
                {
                    layer[^1].neuron[i].Error[y] = layer[^1].neuron[i].RecivedValueFrom[y] * layer[^1].neuron[i].OutD; //same as outE but for each neuron of this layer
                    layer[^1].neuron[i].Delta[y] = layer[^1].neuron[i].Error[y] * functions.Derivative(layer[^1].neuron[i].RecivedValueFrom[y], layer[^2].neuron[y].FunctionType); //same as outD but for each neuron of this layer
                    layer[^1].neuron[i].WeightsFrom[y] += layer[^1].neuron[i].RecivedValueFrom[y] * layer[^1].neuron[i].OutD * learningRate;
                    //if (Math.Pow( layer[^1].neuron[i].Error[y], 2) > 0.001)
                    DeepLearning(layer, layer.Length - 2, y, functions, learningRate);
                }
            }
        }

        public void DeepLearning(NeuronLayer[] layer, int l, int i, Functions functions, double learningRate)
        {
            for (int y = 0; y < layer[l - 1].neuron.Count; y++)
            {
                layer[l].neuron[i].Error[y] = layer[l].neuron[i].RecivedValueFrom[y] * layer[l].neuron[i].Delta[y];
                layer[l].neuron[i].Delta[y] = layer[l].neuron[i].Error[y] * functions.Derivative(layer[l].neuron[i].RecivedValueFrom[y], layer[l - 1].neuron[y].FunctionType);
                layer[l].neuron[i].WeightsFrom[y] += layer[l].neuron[i].RecivedValueFrom[y] * layer[l].neuron[i].Delta[y] * learningRate;
                if (l - 1 > 0) DeepLearning(layer, l - 1, y, functions, learningRate);
            }
        }

        public void Feedforward(NeuronLayer[] layer)
        {
            for (int x = 0; x < layer.Length; x++)
            {
                for (int y = 0; y < layer[x].neuron.Count; y++)
                {
                    if (x > 0) layer[x].neuron[y].Act();
                    if (x < layer.Length - 1) layer[x].neuron[y].Work(y);
                }
            }
        }

        public void Learn(NeuronLayer[] layer, double[,] inputData, double[,] outputData, int count, double learningRate, bool randomInput)
        {
            Functions functions = new Functions();
            int countIn = 0;
            double[] outputValue = new double[layer[^1].neuron.Count];
            for (int i = 0; i < count; i++)
            {
                //countIn = 1;
                // input values
                if (randomInput == true)
                {
                    for (int h = 0; h < layer[0].neuron.Count; h++)
                    {
                        layer[0].neuron[h].Value = Utility.GetRandom();
                    }
                }
                else
                {
                    for (int h = 0; h < layer[0].neuron.Count; h++)
                    {
                        layer[0].neuron[h].Value = inputData[countIn, h];
                    }
                    for (int h = 0; h < layer[^1].neuron.Count; h++)
                    {
                        // out Values
                        outputValue[h] = outputData[countIn, h];
                    }
                }
                Feedforward(layer);
                Backpropagation(layer, outputValue, functions, learningRate);
                if (i % 1 == 0)
                {
                    Console.WriteLine("\n\nFeed #" + i + "  exercise #" + countIn);
                    Utility.ShowResults(layer);
                }
                Utility.ClearValues(layer);
                countIn++;
                if (countIn == inputData.GetLength(1)) countIn = 0;
            }
        }

        //creating objects and giving them some info about other objects
        public NeuronLayer[] Init(int layersNum, int[] neuronNum)
        {
            //Initializing array of layers (which is arrays of neurones)
            NeuronLayer[] layer = new NeuronLayer[layersNum];
            for (int x = 0; x < layer.Length; x++)
            {
                layer[x] = new NeuronLayer();
                for (int y = 0; y < neuronNum[x]; y++)
                {
                    layer[x].neuron.Add(new Neuron());
                    layer[x].neuron[y].LayerNumber = x;
                    layer[x].neuron[y].NeuronNumber = y;
                    layer[x].neuron[y].NeuralNetwork = layer;

                    //applies "parent neurones" to all layers except first
                    if (x > 0)
                    {

                        layer[x].neuron[y].Parents = layer[x - 1].neuron.ToArray();
                        layer[x].neuron[y].Init();
                    }

                }
                //applies "target neurones" to all layers except last one
                if (x < layersNum && x != 0)
                {
                    for (int y = 0; y < neuronNum[x - 1]; y++)
                    {
                        layer[x - 1].neuron[y].TargetNeurons = layer[x].neuron.ToArray();
                    }
                }
            }
            return layer;
        }
    }
}