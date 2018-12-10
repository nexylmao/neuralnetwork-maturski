using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class InputLayer : NeuralTransmitterLayer
    {
        protected List<Axon> axons;

        public List<Axon> Axons { get => axons; }

        public InputLayer()
        {
            axons = new List<Axon>();
        }

        public double[] Emit()
        {
            double[] values = new double[axons.Count];
            for (int i = 0; i < axons.Count; i++)
            {
                values[i] = axons[i].Emit();
            }
            return values;
        }
    }
}
