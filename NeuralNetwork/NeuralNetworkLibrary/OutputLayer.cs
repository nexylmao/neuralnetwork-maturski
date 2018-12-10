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

        public List<Dendrite> Dendrites { get => dendrites; }

        public OutputLayer()
        {
            dendrites = new List<Dendrite>();
        }

        public virtual void Receive(double[] values)
        {
            double[] calculated = new double[dendrites.Count];
            for (int i = 0; i < dendrites.Count; i++)
            {
                calculated[i] = dendrites[i].Handle(values);
            }
            Console.WriteLine(calculated);
        }
    }
}
