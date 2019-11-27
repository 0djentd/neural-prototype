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
        private double[] weights = new double[128];
        private double[] error = new double[128];

        public double Value { get => value; set => this.value = value; }
        public Neuron[] TargetNeurons { get => targetNeurons; set => targetNeurons = value; }
        public Neuron[] Parents { get => parents; set => parents = value; }
        public double Bias { get => bias; set => bias = value; }
        public double[] Weights { get => weights; set => weights = value; }
        public double[] Error { get => error; set => error = value; }

        public void Work(int x)
        {
            Console.WriteLine("\nNeuron started. Value is " + this.Value + "\n");
            for (int i = 0; i < TargetNeurons.Length; i++)
            {
                Console.WriteLine("Target neuron weight is " + TargetNeurons[i].Weights[x]);
                Console.WriteLine("Worked out " + TargetNeurons[i].Value + this.Value * TargetNeurons[i].Weights[x] + "\n");
                TargetNeurons[i].Value = TargetNeurons[i].Value + this.Value * TargetNeurons[i].Weights[x];
            }
        }

        public void Init()
        {
            for (int i = 0; i < Parents.Length; i++)
            {
                Weights[i] = Utility.GetRandom();
                Console.WriteLine("Weight #" + i + " created (" + Math.Round(Weights[i],4) + ")");
            }
        }
    }
}
