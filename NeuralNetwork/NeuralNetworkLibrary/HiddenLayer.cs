using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class HiddenLayer : NeuralHiddenLayer
    {
        protected InputLayer inputLayer;
        protected OutputLayer outputLayer;

        // public InputLayer InputLayer { get => inputLayer; }
        // public OutputLayer OutputLayer { get => outputLayer; }
        // public IEnumerable<Axon> Axons { get => inputLayer.Axons; }
        // public IEnumerable<Dendrite> Dendrites { get => outputLayer.Dendrites; }

        public HiddenLayer()
        {
            inputLayer = new InputLayer();
            outputLayer = new OutputLayer();
        }

        public HiddenLayer(IEnumerable<Dendrite> dendrites)
        {
            inputLayer = new InputLayer();
            outputLayer = new OutputLayer();

            outputLayer.Dendrites.AddRange(dendrites);
            foreach(Dendrite dendrite in dendrites)
            {
                inputLayer.Axons.Add(new Axon());
            }
        }

        public void AddDendrite(Dendrite dendrite)
        {
            outputLayer.Dendrites.Add(dendrite);
            inputLayer.Axons.Add(new Axon());
        }

        public void AddDendrites(IEnumerable<Dendrite> dendrites)
        {
            outputLayer.Dendrites.AddRange(dendrites);
            foreach (Dendrite dendrite in dendrites)
            {
                inputLayer.Axons.Add(new Axon());
            }
        }

        public double[] Passthrough(double[] values)
        {
            outputLayer.Receive(values);
            for (int i = 0; i < outputLayer.Dendrites.Count; i++)
            {
                inputLayer.Axons[i].Value = outputLayer.Calculated[i];
            }
            return inputLayer.Emit();
        }
    }
}
