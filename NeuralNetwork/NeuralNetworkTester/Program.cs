using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkLibrary;

namespace NeuralNetworkTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Axon a = new Axon(1);
            Axon b = new Axon(2);
            Axon c = new Axon(3);
            Dendrite d = new Dendrite(1);
            d.Handle = (values) =>
            {
                return values.Sum() * d.Weight;
            };

            // this is basically what layers need to do
            // collect all emits, and send it to dendrite
            InputLayer input = new InputLayer();
            input.Axons.Add(a);
            input.Axons.Add(b);
            input.Axons.Add(c);
            OutputLayer output = new OutputLayer();
            output.Dendrites.Add(d);
            output.Receive(input.Emit());

            Console.ReadKey(true);
        }
    }
}
