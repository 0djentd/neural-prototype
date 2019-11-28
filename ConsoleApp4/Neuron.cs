using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    public class NeuronLayer
    {
        public List<Neuron> neurons = new List<Neuron>();
    }

    public class Neuron
    {
        //for this neuron
        private double value;
        private Neuron[] targetNeurons;
        private Neuron[] parents;
        private double bias = 0;

        //for neuron's parents
        private double[] weightsFrom = new double[32];
        private double[] oldWeightsFrom = new double[32];
        private double[] recivedValue = new double[32];
        private double e;

        //for backpropagation
        private double[] error = new double[32];
        private double[] delta = new double[32];

        public double Value { get => value; set => this.value = value; }
        public Neuron[] TargetNeurons { get => targetNeurons; set => targetNeurons = value; }
        public Neuron[] Parents { get => parents; set => parents = value; }
        public double Bias { get => bias; set => bias = value; }
        public double[] WeightsFrom { get => weightsFrom; set => weightsFrom = value; }
        public double OutE { get => e; set => e = value; }
        public double[] RecivedValueFrom { get => recivedValue; set => recivedValue = value; }
        public double[] Error { get => error; set => error = value; }
        public double[] OldWeightsFrom { get => oldWeightsFrom; set => oldWeightsFrom = value; }
        public double OutD { get; internal set; }
        public double[] Delta { get => delta; set => delta = value; }

        //x is representing working neurone's number in working layer
        public void Work(int x)
        {
            //Console.WriteLine("\nNeuron started. Value is " + this.Value + "\n");
            for (int i = 0; i < TargetNeurons.Length; i++)
            {
                //Console.WriteLine("Target neuron weight is " + TargetNeurons[i].Weights[x]);
                //Console.WriteLine("Worked out " + TargetNeurons[i].Value + this.Value * TargetNeurons[i].Weights[x] + "\n");
                double aw = this.Value * TargetNeurons[i].WeightsFrom[x];
                TargetNeurons[i].RecivedValueFrom[x] = aw;
                TargetNeurons[i].Value = TargetNeurons[i].Value + aw;
            }
        }

        public void Z()
        {
            /*for(int i = 0; i<this.WeightsFrom.Length; i++)
            {
                this.WeightsFrom[i] = Functions.Sigmoid(this.WeightsFrom[i])-1;
            }*/
            //Console.Write("\nZ [" + this.value + "]");
            this.Value = Functions.Sigmoid(this.Value + this.Bias);
            //Console.Write("--> [" + this.value + "]\n");
        }

        public void Init()
        {
            for (int i = 0; i < Parents.Length; i++)
            {
                this.WeightsFrom[i] = Utility.GetRandom();
                //Console.WriteLine("Weight #" + i + " created (" + Math.Round(Weights[i], 4) + ")");
            }
        }
    }
}
