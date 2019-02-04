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

        class TrainingSet
        {
            private double[] inputs;

            private double[] results;

            public double[] Inputs => inputs;

            public double[] Results => results;

            public TrainingSet(double[] inputs, double[] results)
            {
                this.inputs = inputs;
                this.results = results;
            }
        }
        
        static void CreateEntireRGBMap()
        {
            List<TrainingSet> sets = new List<TrainingSet>();
            for (uint i = 0; i <= 255; i+=5)
            {
                for (uint j = 0; j <= 255; j+=5)
                {
                    for (uint k = 0; k <= 255; k+=5)
                    {
                        double[] inputs = new double[] {i, j, k};
                        double[] result = ContrastColor(i, j, k);
                        sets.Add(new TrainingSet(inputs, result));
                        Console.WriteLine("[{0},{1},{2}] = [{3},{4}]", i, j, k, result[0], result[1]);
                    }
                }
            }

            TextWriter tw = new StreamWriter("RGBMap.json");
            tw.Write(JsonConvert.SerializeObject(sets));
            tw.Close();
        }

        static void LoadAndTest(NeuralNetwork RGB)
        {
            TextReader tr = new StreamReader("RGBMap.json");
            List<TrainingSet> sets = JsonConvert.DeserializeObject<List<TrainingSet>>(tr.ReadToEnd());
            tr.Close();

            TextWriter tw = new StreamWriter("Results.txt");
            
            double costSum = 0;
            
            foreach (var set in sets)
            {
                for (var i = 0; i < set.Inputs.Length; i++)
                {
                    RGB.Inputs[i] = set.Inputs[i];
                }
                
                RGB.Shock();

                double cost = Utility.Cost(RGB.Results, set.Results);
                costSum += cost;
                
                string result = string.Format("I:[{0},{1},{2}]|W:[{3},{4}]|R:[{5},{6}]|C:{7}", set.Inputs[0],
                    set.Inputs[1], set.Inputs[2], set.Results[0], set.Results[1], RGB.Results[0], RGB.Results[1], cost);

                Console.WriteLine(result);
                tw.WriteLine(result);
            }

            Console.WriteLine("Cost sum = {0}", costSum);
            tw.WriteLine("Cost sum = {0}", costSum);
            tw.Close();
        }
        
        static double[] ContrastColor(uint R, uint G, uint B)
        {
            return (0.299 * R + 0.587 * G + 0.114 * B) / 255 > 0.5 ? new double[] {1, 0} : new double[] {0, 1};
        }
        
        static void Main(string[] args)
        {
            NeuralNetwork RGB = WorkWithRGBNetwork();
            LoadAndTest(RGB);
        }
    }
}