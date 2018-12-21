using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class OutputLayer : NeuralReceiverlayer
    {
        protected List<Dendrite> dendrites;

        protected double[] calculated;

        public List<Dendrite> Dendrites { get => dendrites; }
        public double[] Calculated { get => calculated; }

        public OutputLayer()
        {
            dendrites = new List<Dendrite>();
        }

        public virtual void Receive(double[] values)
        {
            calculated = new double[dendrites.Count];
            for (int i = 0; i < dendrites.Count; i++)
            {
                calculated[i] = dendrites[i].Handle(values);
            }
        }
    }
}
