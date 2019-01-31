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
            Random r = new Random(DateTime.Now.Millisecond);

            Layer input = new Layer("Input Layer", 0.1, 2);
            Layer hidden1 = new Layer("Hidden Layer 1", 0.1, 2);
            Layer output = new Layer("Output Layer", 0.1, 1);

            foreach(Neuron n in input.Neurons)
            {
                n.OutputPulse.Value = r.Next(0, 100);
            }

            Network network = new Network();
            network.AddLayer(input);
            network.AddLayer(hidden1);
            network.AddLayer(output);

            network.Build();
            Console.WriteLine(network.Print());

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
