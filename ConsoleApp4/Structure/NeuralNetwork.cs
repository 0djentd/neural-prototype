using System;

namespace ConsoleApp4
{
    class NeuralNetwork
    {
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
                    for (int y = 0; y < layer[x - 1].neuron.Count; y++)
                    {
                        layer[x - 1].neuron[y].TargetNeurons = layer[x].neuron.ToArray();
                    }
                }
            }
            return layer;
        }


        public void Learn(NeuronLayer[] layer, double[,] inputData, double[,] outputData, int epoch, int batch, double learningRate, bool debug)
        {
            int countIn = 0;
            double[] CorrectOutput = new double[layer[^1].neuron.Count - layer[^1].BiasNeurons];
            for (int i = 0; i < epoch * inputData.GetLength(0); i++)
            {
                //clearing gradient and input/output/correct output values of neurons
                Utility.ClearInOutValues(layer);
                Gradient.Clear(layer);

                // input values
                if (i % 1 == 0) Console.WriteLine("\n\nFeed #" + i + "  exercise #" + countIn);
                for (int h = 0; h < layer[0].neuron.Count - layer[0].BiasNeurons; h++)
                {
                    layer[0].neuron[h].Output = inputData[countIn, h];
                }

                // out values
                for (int h = 0; h < layer[^1].neuron.Count - layer[^1].BiasNeurons; h++)
                {
                    CorrectOutput[h] = outputData[countIn, h];
                }

                Feedforward(layer);
                Backpropagation(layer, CorrectOutput, learningRate, debug);

                if (countIn == inputData.GetLength(0) - 1) countIn = 0;
                else countIn++;
            }
        }

        public void Feedforward(NeuronLayer[] layer)
        {
            for (int x = 0; x < layer.Length; x++)
            {
                for (int y = 0; y < layer[x].neuron.Count; y++)
                {
                    layer[x].neuron[y].Act();
                    if (x < layer.Length - 1) layer[x].neuron[y].Work(y);
                }
            }
        }


        public void Backpropagation(NeuronLayer[] layer, double[] correctOutput, double learningRate, bool debug)
        {
            Utility.BackupSynapseValues(layer);
            Gradient.Calculate(layer, correctOutput);
            if (debug == true)
            {
                Gradient.Show(layer);
                Utility.ShowNeuronMap(layer, true);
            }
            Optimize(layer, learningRate);
        }

        public double Optimize(NeuronLayer[] layer, double learningRate)
        {
            double amount = 0;
            for (int j = layer.Length - 1; j > 0;)
            {
                for (int i = 0; i < layer[j].neuron.Count - layer[j].BiasNeurons; i++)
                {
                    for (int z = 0; z < layer[j - 1].neuron.Count; z++)
                    {
                        layer[j].neuron[i].W_From[z] -= (layer[j].neuron[i].DeltaE_wrt_W[z] * learningRate);
                        amount = layer[j].neuron[i].W_From[z] - layer[j].neuron[i].Old_W_From[z];
                    }
                }
                --j;
            }
            return amount;
        }

        public void Predict(NeuronLayer[] layer, double[,] inputData)
        {
            Utility.ClearInOutValues(layer);
            Gradient.Clear(layer);
            int countIn = 0;
            for (int h = 0; h < layer[0].neuron.Count - layer[0].BiasNeurons; h++)
            {
                layer[0].neuron[h].Output = inputData[countIn, h];
            }
            Console.WriteLine("Feedforwarding new input data...");
            Feedforward(layer);
            Utility.ShowNeuronMap(layer, true);
        }

        public static void Randomize(NeuronLayer[] layer)
        {
            for (int l = 1; l < layer.Length; l++)
            {
                for (int i = 0; i < layer[l].neuron.Count; i++)
                {
                    layer[l].neuron[i].Init();
                }
            }
        }
    }
}