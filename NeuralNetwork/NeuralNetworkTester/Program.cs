using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using NeuralNetworkLibrary;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing;

namespace NeuralNetworkTester
{
    class Program
    {
        static void CreatingFirstTest()
        {
            var inputLayer = new InputLayer("Input layer", 5);
            var hiddenLayer = new HiddenLayer("Hidden layer 1", 5, inputLayer);
            var outputLayer = new ConsoleOutputLayer("Output layer", 3, hiddenLayer);
            
            var neuralNetwork = new NeuralNetwork("Basic example network", inputLayer);
            neuralNetwork.InputLayer.Shock();
            neuralNetwork.SaveNetwork();   
        }

        static void LoadFirstNetwork()
        {
            NeuralNetwork neuralNetwork = NeuralNetwork.LoadNetwork("Basic example network");

            HiddenLayer hiddenLayer = neuralNetwork.HiddenLayers[0];
            Console.WriteLine(Utility.PrintMatrix(hiddenLayer.Weights));
            Console.WriteLine(Utility.PrintVector(hiddenLayer.Biases));
            
            Console.WriteLine(Utility.PrintMatrix(neuralNetwork.OutputLayer.Weights));
            Console.WriteLine(Utility.PrintVector(neuralNetwork.OutputLayer.Biases));

            var desired = new double[] {0, 0, 1};
            neuralNetwork.Shock();

            double cost = Utility.Cost(neuralNetwork.Results, desired);
            Console.WriteLine("Cost : " + cost);

            neuralNetwork.InputLayer.Shock();            
        }

        static void CreateDigitNetwork()
        {
            var inputLayer = new InputLayer("Image input", 784);
            var hiddenOne = new HiddenLayer("Hidden layer one", 16, inputLayer);
            var hiddenTwo = new HiddenLayer("Hidden layer two", 16, hiddenOne);
            var outputLayer = new ConsoleOutputLayer("Output layer", 10, hiddenTwo);
            
            var neuralNetwork = new NeuralNetwork("Handwritten numbers network", inputLayer);
            neuralNetwork.SaveNetwork();
        }

        static void CreateGiantNetwork()
        {
            var inputLayer = new InputLayer("IMG", 250000);
            var hiddenOne = new HiddenLayer("H1", 250, inputLayer);
            var hiddenTwo = new HiddenLayer("H2", 25, hiddenOne);
            var outputLayer = new OutputLayer("OUTPUTIMG", 10, hiddenTwo);
            
            var neuralNetwork = new NeuralNetwork("GIANT IMAGE NETWORK", inputLayer);
            neuralNetwork.SaveNetwork();
        }
        
        static void CreateRGBNetwork()
        {
            var input = new InputLayer("RGB Input", 3);
            var hidden = new HiddenLayer("RGB Hidden", 3, input);
            var output = new ConsoleOutputLayer("RGB Output", 2, hidden);
            
            var neuralNetwork = new NeuralNetwork("RGB Neural Network", input);
            neuralNetwork.SaveNetwork();
        }

        static NeuralNetwork WorkWithRGBNetwork()
        {
            NeuralNetwork RGB = NeuralNetwork.LoadNetwork("RGB Neural Network");
            return RGB;
        }
        
        static void CreateEntireRGBMap()
        {
            List<TrainingSet> sets = new List<TrainingSet>();
            double step = 0.05;
            for (double i = 0; i <= 1; i+=step)
            {
                for (double j = 0; j <= 1; j+=step)
                {
                    for (double k = 0; k <= 1; k+=step)
                    {
                        double[] inputs = new double[] {i, j, k};
                        double[] result = ContrastColor(i, j, k);
                        sets.Add(new TrainingSet(inputs, result));
                        Console.WriteLine("[{0},{1},{2}] = [{3},{4}]", i, j, k, result[0], result[1]);
                    }
                }
            }
            
            Random rng = new Random();
            
            var n = sets.Count;  
            while (n > 1) {  
                n--;  
                var k = rng.Next(n + 1);  
                var value = sets[k];  
                sets[k] = sets[n];  
                sets[n] = value;  
            }
            
            TextWriter tw = new StreamWriter("RGBMap.json");
            tw.Write(JsonConvert.SerializeObject(sets));
            tw.Close();
        }
        
        static double[] ContrastColor(double R, double G, double B)
        {
            R = (int) Math.Round(R * 255, 0);
            G = (int) Math.Round(G * 255, 0);
            B = (int) Math.Round(B * 255, 0);
            return (0.299 * R + 0.587 * G + 0.114 * B) / 255 > 0.5 ? new double[] {1, 0} : new double[] {0, 1};
        }
        
        static void Main(string[] args)
        {
            NeuralNetwork RGB = WorkWithRGBNetwork();
//            CreateEntireRGBMap();
//            RGB.TestNeuralNetwork("RGBTest");
            RGB.Train("RGBMap");
        }
    }
}