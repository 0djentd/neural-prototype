﻿using System;
using System.Collections.Generic;

namespace ConsoleApp4
{
    public class NeuronLayer
    {
        public List<Neuron> neuron = new List<Neuron>();
        public int assignedFunctions = 0;
        public void SetFunctions(int count, int function)
        {
            int availableFunctions = neuron.Count - this.assignedFunctions;
            if (availableFunctions >= count)
            {
                for (int i = this.assignedFunctions; i < count; i++)
                {
                    this.neuron[i].FunctionType = function;
                }
            }
            else
            {
                Console.WriteLine("Error assigning functions to neurons. Too many.");
            }
        }

        public void SetFunctionsAll(int function)
        {
            for (int i = 0; i < neuron.Count; i++)
            {
                this.neuron[i].FunctionType = function;
            }
            Console.WriteLine("Set all functions to " + function);
        }

        public void SetFunctionsAuto()
        {
            int availableFunctions = neuron.Count - this.assignedFunctions;
            for (int i = assignedFunctions; i < availableFunctions / 3; i++)
            {
                this.neuron[i].FunctionType = 1;
                this.neuron[i + 1].FunctionType = 2;
                this.neuron[i + 2].FunctionType = 3;
                this.assignedFunctions += 3;
                Console.WriteLine("Set functions to sigmoid, tanh and relu");
            }
            for (int i = assignedFunctions; i < neuron.Count % 3; i++)
            {
                this.neuron[i].FunctionType = 2;
                this.assignedFunctions += 1;
                Console.WriteLine("Set function to tanh");
            }
        }
    }

    public class Neuron
    {
        //for this neuron
        private double value;
        private double input;
        private Neuron[] targetNeurons;
        private Neuron[] parents;
        private double bias = 0.5;
        //type of act. function
        private int functionType = 0;
        //k for parametric relu  etc
        private double k = 0.1;
        //for softmax act. function
        private int layerNumber;
        private int neuronNumber;

        //for neuron's parents
        private NeuronLayer[] neuralNetwork;
        private double[] weightsFrom = new double[128];
        private double[] oldWeightsFrom = new double[128];
        private double[] recivedValue = new double[128];
        private double[] deltaOut_wrt_W = new double[128];
        private double[] deltaE_wrt_W = new double[128];
        private double e;

        //for backpropagation
        private double[] error = new double[128];

        private double[] delta = new double[128];

        public double Output { get => value; set => this.value = value; }
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
        public int FunctionType { get => functionType; set => functionType = value; }
        public double K { get => k; set => k = value; }
        public int LayerNumber { get => layerNumber; set => layerNumber = value; }
        public int NeuronNumber { get => neuronNumber; set => neuronNumber = value; }
        public NeuronLayer[] NeuralNetwork { get => neuralNetwork; set => neuralNetwork = value; }
        public double DeltaE_wrt_Output { get; internal set; }
        public double Out_wrt_In { get; internal set; }
        public double Input { get => input; set => input = value; }
        public double[] DeltaOut_wrt_W { get => deltaOut_wrt_W; set => deltaOut_wrt_W = value; }
        public double[] DeltaE_wrt_W { get => deltaE_wrt_W; set => deltaE_wrt_W = value; }
        public double DeltaOut_wrt_deltaIn { get; internal set; }

        //x is representing working neurone's number in working layer
        public void Work(int x)
        {
            //Console.WriteLine("\nNeuron started. Value is " + this.Value + "\n");
            for (int i = 0; i < this.TargetNeurons.Length; i++)
            {
                //Console.WriteLine("Target neuron weight is " + this.TargetNeurons[i].Weights[x]);
                //Console.WriteLine("Worked out " + this.TargetNeurons[i].Value + this.Value * this.TargetNeurons[i].Weights[x] + "\n");
                double aw = this.Output * this.TargetNeurons[i].WeightsFrom[x];
                this.TargetNeurons[i].RecivedValueFrom[x] = aw;
                this.TargetNeurons[i].Output = this.TargetNeurons[i].Output + aw;
            }
        }

        //activation functions and their derivatives
        public void Act()
        {
            this.Input = this.Output;
            if (this.FunctionType == 1) this.Output = Functions.Sigmoid(this.Output + this.Bias);
            else if (this.FunctionType == 2) this.Output = Functions.TanH(this.Output + this.Bias);
            else if (this.FunctionType == 3) this.Output = Functions.ReLU(this.Output + this.Bias);
            else if (this.FunctionType == 4) this.Output = Functions.LeReLU(this.Output + this.Bias);
            else if (this.FunctionType == 5) this.Output = Functions.EReLU(this.Output + this.Bias, K);
            else if (this.FunctionType == 6) this.Output = Functions.Softmax(this.NeuronNumber, NeuralNetwork[this.LayerNumber]);
        }

        public double Derivative()
        {
            if (this.FunctionType == 0) return 1;
            else if (this.FunctionType == 1) return Functions.SigmoidDerivative(this.Output + this.Bias);
            else if (this.FunctionType == 2) return Functions.TanHDerivative(this.Output + this.Bias);
            else if (this.FunctionType == 3) return Functions.ReLUDerivative(this.Output + this.Bias);
            else if (this.FunctionType == 4) return Functions.LeReLUDerivative(this.Output + this.Bias);
            else if (this.FunctionType == 5) return Functions.EReLUDerivative(this.Output + this.Bias, K);
            else if (this.FunctionType == 6) return Functions.SoftmaxDerivative(this.NeuronNumber, NeuralNetwork[this.LayerNumber]);
            else return 0;
        }

        public void Init()
        {
            for (int i = 0; i < Parents.Length; i++)
            {
                this.WeightsFrom[i] = Utility.GetRandom();
            }
        }
    }
}