using System;
using System.Data;
using NeuralNetworkLibrary;

namespace NeuralNetworkTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLayer = new InputLayer("Input layer", 5, null);
            inputLayer.SetValue(0, 2);
            inputLayer.SetValue(3, 4);
            inputLayer.SetValue(2, 5);

            var hiddenLayer = new HiddenLayer("Hidden layer 1", 5, inputLayer);
            var outputLayer = new ConsoleOutputLayer("Output layer", 3, hiddenLayer);

            inputLayer.Shock();
        }
    }
}
