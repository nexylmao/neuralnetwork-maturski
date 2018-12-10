using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkLibrary;

namespace NeuralNetworkTester
{
    public class ConsoleAxon : Axon
    {
        protected string name;

        public string Name { get => name; }

        public ConsoleAxon(string name)
        {
            this.name = name;
        }

        public override void Emit(double value)
        {
            Console.Write("Axon \"" + name + "\" > ");
            Console.WriteLine("Emitting " + value + " to " + targets.Count + " dendrite(s).");
            base.Emit(value);
        }
    }

    public class ConsoleDendrite : Dendrite
    {
        protected string name;

        public string Name { get => name; }

        public ConsoleDendrite(string name) : base(1)
        {
            this.name = name;
            ReceiveValue += ValueHandler;
        }

        public void ValueHandler(object sender, double value)
        {
            Console.Write("Dendrite \"" + name + "\" > ");
            Console.WriteLine("Received " + value + ".");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConsoleAxon axon = new ConsoleAxon("Main Console Axon");
            ConsoleDendrite dendrite = new ConsoleDendrite("Main Console Dendrite");
            Neuron neuron = new Neuron(1.2);

            axon.AddDendrite(neuron.Dendrite);
            neuron.Axon.AddDendrite(dendrite);

            axon.Emit(1);

            Console.ReadKey(true);
        }
    }
}
