using System;
using System.IO;
using OfficeOpenXml;
using NeuralNetworkLibrary;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing;

namespace NeuralNetworkTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLayer = new InputLayer("Input layer", 5);
            var hiddenLayer = new HiddenLayer("Hidden layer 1", 5, inputLayer);
            var outputLayer = new ConsoleOutputLayer("Output layer", 3, hiddenLayer);

            Console.WriteLine(JsonConvert.SerializeObject(inputLayer));
            Console.WriteLine(JsonConvert.SerializeObject(hiddenLayer));
            Console.WriteLine(JsonConvert.SerializeObject(outputLayer));

            Utility.SaveNetwork(inputLayer);
            
//            inputLayer.Shock();
        }
    }
}