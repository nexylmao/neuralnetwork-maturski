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

        public void ValueHandler(object sender, double[] values)
        {
            Console.Write("Dendrite \"" + name + "\" > ");
            Console.WriteLine("Received " + values.Length + " values.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConsoleAxon axon = new ConsoleAxon("Main Console Axon");
            ConsoleDendrite dendrite = new ConsoleDendrite("Main Console Dendrite");

            axon.Value = 1;
            axon.AddDendrite(dendrite);

            dendrite.Pull();

            Console.ReadKey(true);
        }
    }
}
