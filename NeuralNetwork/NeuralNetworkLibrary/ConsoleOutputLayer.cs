using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class ConsoleOutputLayer : OutputLayer
    {
        public override void Receive(double[] values)
        {
            base.Receive(values);
            Console.WriteLine("Output layer returns { ");
            for (int i = 0; i < calculated.Length; i++)
            {
                Console.WriteLine("\tDendrite[{0}] => {1}", i, calculated[i]);
            }
            Console.WriteLine("}");
        }
    }
}
