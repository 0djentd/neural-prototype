using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
    class Layer
    {
        public List<Neuron> neuron = new List<Neuron>();
        private int layerNumber;
        private int biasNeurons = 0;
        private int assignedFunctions = 0;

        public int LayerNumber { get => layerNumber; set => layerNumber = value; }
        public int BiasNeurons { get => biasNeurons; set => biasNeurons = value; }
        public int AssignedFunctions { get => assignedFunctions; set => assignedFunctions = value; }

        public void SetFunctions(int count, int function)
        {
            int availableFunctions = this.neuron.Count - this.BiasNeurons - this.AssignedFunctions;
            if (availableFunctions >= count)
            {
                for (int i = this.AssignedFunctions; i < count; i++)
                {
                    this.neuron[i].FunctionType = function;
                    Console.WriteLine("Set " + layerNumber + " layer " + count + " neuron function to " + function);
                }
            }
            else
            {
                Console.WriteLine("Error assigning functions to neurons. Too many.");
            }
        }

        public void SetFunctionsAll(int function)
        {
            for (int i = 0; i < this.neuron.Count - this.BiasNeurons; i++)
            {
                this.neuron[i].FunctionType = function;
            }
            Console.WriteLine("Set all " + layerNumber + " layer functions to " + function);
        }

        public void SetFunctionsAuto()
        {
            int availableFunctions = this.neuron.Count - this.BiasNeurons - this.AssignedFunctions;
            for (int i = AssignedFunctions; i < availableFunctions / 3; i++)
            {
                this.neuron[i].FunctionType = 1;
                this.neuron[i + 1].FunctionType = 2;
                this.neuron[i + 2].FunctionType = 3;
                this.AssignedFunctions += 3;
                Console.WriteLine("Set " + layerNumber + " layer functions to sigmoid, tanh and relu");
            }
            for (int i = AssignedFunctions; i < (neuron.Count - this.BiasNeurons) % 3; i++)
            {
                this.neuron[i].FunctionType = 2;
                this.AssignedFunctions += 1;
                Console.WriteLine("Set function to tanh");
            }
        }

        public void AddBias()
        {
            Console.WriteLine("Added bias to layer " + LayerNumber);
            this.neuron.Add(new Neuron());
            this.BiasNeurons += 1;
            this.neuron[neuron.Count - 1].Bias = true;
            this.neuron[neuron.Count - 1].Output = 1;
        }
    }
}
