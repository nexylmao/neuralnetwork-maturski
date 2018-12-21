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
            Axon a1 = new Axon(21);
            Axon a2 = new Axon(9);
            Axon a3 = new Axon(1);

            Dendrite d1 = new Dendrite(1);
            d1.Handle = values =>
            {
                return values.Average() * d1.Weight;
            };
            Dendrite d2 = new Dendrite(2);
            d2.Handle = values =>
            {
                return values.Average() * d2.Weight;
            };
            Dendrite d3 = new Dendrite(3);
            d3.Handle = values =>
            {
                return values.Average() * d3.Weight;
            };

            // this is basically what layers need to do
            // collect all emits, and send it to dendrite
            InputLayer input = new InputLayer();
            input.Axons.Add(a1);
            input.Axons.Add(a2);
            input.Axons.Add(a3);

            OutputLayer output = new ConsoleOutputLayer();
            output.Dendrites.Add(d1);
            output.Dendrites.Add(d2);
            output.Dendrites.Add(d3);

            double[] emitted = input.Emit();
            Console.Write("Axons emitted : {\n\t");
            foreach(double x in emitted)
            {
                Console.Write(" {0} ", x);
            }
            Console.WriteLine("\n}");
            output.Receive(emitted);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
