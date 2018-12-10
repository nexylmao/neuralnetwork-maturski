using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class Dendrite
    {
        protected double weight;

        public double Weight { get => weight; set => weight = value; }

        public Dendrite(double weight)
        {
            this.weight = weight;
        }

        public Func<double[], double> Handle;
    }
}
