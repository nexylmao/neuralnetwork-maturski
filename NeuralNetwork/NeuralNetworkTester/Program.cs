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
//            var inputLayer = new InputLayer("Input layer", 5);
//            var hiddenLayer = new HiddenLayer("Hidden layer 1", 5, inputLayer);
//            var outputLayer = new ConsoleOutputLayer("Output layer", 3, hiddenLayer);
//            
//            var neuralNetwork = new NeuralNetwork("Basic example network", inputLayer);
//            neuralNetwork.InputLayer.Shock();
//            neuralNetwork.SaveNetwork();

            NeuralNetwork neuralNetwork = NeuralNetwork.LoadNetwork("Basic example network");

//            Console.WriteLine("Input Layer");
//            Console.WriteLine(neuralNetwork.InputLayer.Print());

//            Console.WriteLine("Hidden Layers");
//            foreach (HiddenLayer hiddenLayer in neuralNetwork.HiddenLayers)
//            {
//                Console.WriteLine(hiddenLayer.Print());
//            }

//            Console.WriteLine("Output Layer");
//            Console.WriteLine(neuralNetwork.OutputLayer.Print());

            HiddenLayer hiddenLayer = neuralNetwork.HiddenLayers[0];
            Console.WriteLine(Utility.PrintMatrix(hiddenLayer.Weights));
            Console.WriteLine(Utility.PrintVector(hiddenLayer.Biases));
            
            Console.WriteLine(Utility.PrintMatrix(neuralNetwork.OutputLayer.Weights));
            Console.WriteLine(Utility.PrintVector(neuralNetwork.OutputLayer.Biases));
            
//            neuralNetwork.InputLayer.Shock();
        }
    }
}