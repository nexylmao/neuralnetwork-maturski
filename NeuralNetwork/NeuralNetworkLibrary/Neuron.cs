using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class Neuron
    {
        protected Axon axon;
        protected Dendrite dendrite;

        public Axon Axon { get => axon; }
        public Dendrite Dendrite { get => dendrite; }

        public Neuron(double weight)
        {
            axon = new Axon();
            dendrite = new Dendrite(weight);
            dendrite.ReceiveValue += HandleShock;
        }

        public void HandleShock(object send, double value)
        {
            value *= Dendrite.Weight;
            axon.Emit(value);
        }
    }
}
