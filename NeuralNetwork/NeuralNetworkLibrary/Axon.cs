using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class Axon
    {
        protected double value;

        public double Value { get => value; set => this.value = value; }

        public Axon()
        {
            value = 1;
        }

        public Axon(double value)
        {
            this.value = value;
        }

        public double Emit()
        {
            return value;
        }
    }
}
