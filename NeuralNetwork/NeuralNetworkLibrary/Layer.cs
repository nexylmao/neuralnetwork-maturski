using System;
using System.Collections.Generic;

namespace NeuralNetworkLibrary
{
    public class Layer
    {
        public string Name { get; set; }

        public double Weight { get; set; }

        public List<Neuron> Neurons { get; set; }

        public Layer(string name, double weight, int neurons)
        {
            Neurons = new List<Neuron>();
            for (int i = 0; i < neurons; i++)
            {
                Neurons.Add(new Neuron());
            }
            Weight = weight;
            Name = name;
        }
    }
}
