using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class Network
    {
        public List<Layer> Layers { get; private set; }

        public Network()
        {
            Layers = new List<Layer>();
        }

        public void AddLayer(Layer layer)
        {
            Layers.Add(layer);
        }

        public void Build()
        {
            int i = 0;
            foreach (Layer layer in Layers)
            {
                if (i >= Layers.Count - 1)
                {
                    break;
                }
                Layer next = Layers[i + 1];
                CreateNetwork(layer, next);
                i++;
            }
        }

        public void CreateNetwork(Layer from, Layer to)
        {
            foreach (Neuron toNeuron in to.Neurons)
            {
                foreach (Neuron fromNeuron in from.Neurons)
                {
                    toNeuron.Dendrites.Add(new Dendrite() { InputPulse = fromNeuron.OutputPulse, Weight = to.Weight });
                }
            }
        }

        public string Print()
        {
            return JsonConvert.SerializeObject(Layers, Formatting.Indented);
        }
    }
}
