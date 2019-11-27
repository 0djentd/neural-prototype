using System;

namespace ConsoleApp4
{
    class Neuron
    {
        private double value;
        private Neuron[] targetNeurons;
        private double[] weights = new double[1024];
        private Neuron[] parents;
        private double bias = 0;
        public double Value { get => value; set => this.value = value; }
        public Neuron[] TargetNeurons { get => targetNeurons; set => targetNeurons = value; }
        public double[] Weights { get => weights; set => weights = value; }
        public Neuron[] Parents { get => parents; set => parents = value; }
        public double Bias { get => bias; set => bias = value; }

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
            Console.WriteLine("Neuron creating random weights");

            for (int i = 0; i < Parents.Length; i++)
            {
                Weights[i] = Utility.GetRandom();
                Console.WriteLine("Weight #" + i + " created (" + Weights[i] + ")");
            }
        }
    }
}
