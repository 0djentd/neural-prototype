using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    class Neuron
    {
        //for this neuron
        private double output;
        private double input;
        private bool bias = false;
        private int initType = 0;

        //info
        private NeuronLayer[] neuralNetwork;
        private Neuron[] targetNeurons;
        private Neuron[] parents;

        //type of act. function
        private int functionType = 0;
        //k for parametric relu  etc
        private double k = 0.5;
        //for softmax act. function
        private int layerNumber;
        private int neuronNumber;

        //for neuron's parents
        private double[] w_From = new double[128];
        private double[] oldWeightsFrom = new double[128];
        private double[] recivedInput = new double[128];
        private double[] deltaE_wrt_W = new double[128];

        //neuron 
        public double Input { get => input; set => input = value; }
        public double Output { get => output; set => this.output = value; }
        public bool Bias { get => bias; set => bias = value; }
        public double[] W_From { get => w_From; set => w_From = value; }
        public double[] Old_W_From { get => oldWeightsFrom; set => oldWeightsFrom = value; }
        public double[] RecivedInputFrom { get => recivedInput; set => recivedInput = value; }

        //info
        public NeuronLayer[] NeuralNetwork { get => neuralNetwork; set => neuralNetwork = value; }
        public Neuron[] TargetNeurons { get => targetNeurons; set => targetNeurons = value; }
        public Neuron[] Parents { get => parents; set => parents = value; }
        public int LayerNumber { get => layerNumber; set => layerNumber = value; }
        public int NeuronNumber { get => neuronNumber; set => neuronNumber = value; }

        //functions
        public int FunctionType { get => functionType; set => functionType = value; }
        public double K { get => k; set => k = value; }

        //for backpropagation
        public double DeltaE_wrt_Output { get; internal set; }
        public double DeltaE_wrt_In { get; internal set; }
        public double[] DeltaE_wrt_W { get => deltaE_wrt_W; set => deltaE_wrt_W = value; }
        public double Error { get; internal set; }

        //x is representing working neurone's number in working layer
        public void Work(int x)
        {
            for (int i = 0; i < this.TargetNeurons.GetLength(0); i++)
            {
                double aw = this.Output * this.TargetNeurons[i].W_From[x];
                this.TargetNeurons[i].RecivedInputFrom[x] = aw;
                this.TargetNeurons[i].Output += aw;
            }
        }

        //activation functions and their derivatives
        public void Act()
        {
            this.Input = this.Output;
            if (this.FunctionType == 0 || this.Bias == true) this.Output = this.Output;
            else if (this.FunctionType == 1) this.Output = Functions.Sigmoid(this.Output);
            else if (this.FunctionType == 2) this.Output = Functions.TanH(this.Output);
            else if (this.FunctionType == 3) this.Output = Functions.ReLU(this.Output);
            else if (this.FunctionType == 4) this.Output = Functions.LeReLU(this.Output);
            else if (this.FunctionType == 5) this.Output = Functions.EReLU(this.Output, K);
            else if (this.FunctionType == 6) this.Output = Functions.Softmax(this.NeuronNumber, NeuralNetwork[this.LayerNumber]);
        }

        public double Derivative()
        {
            if (this.FunctionType == 0 || this.Bias == true) return 1;
            else if (this.FunctionType == 1) return Functions.SigmoidDerivative(this.Output);
            else if (this.FunctionType == 2) return Functions.TanHDerivative(this.Output);
            else if (this.FunctionType == 3) return Functions.ReLUDerivative(this.Output);
            else if (this.FunctionType == 4) return Functions.LeReLUDerivative(this.Output);
            else if (this.FunctionType == 5) return Functions.EReLUDerivative(this.Output, K);
            else if (this.FunctionType == 6) return Functions.SoftmaxDerivative(this.NeuronNumber, NeuralNetwork[this.LayerNumber]);
            else return 0;
        }

        public void Init()
        {
            if (this.Bias == false)
            {
                for (int i = 0; i < this.Parents.Length; i++)
                {
                    Random random = new Random();
                    this.W_From[i] = random.NextDouble()*2-1 * Math.Sqrt(1.0 / this.Parents.Length);
                }
            }
        }
    }
}