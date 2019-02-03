using System;
using System.IO;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class ConsoleOutputLayer : OutputLayer
    {
        public ConsoleOutputLayer(string name, int neuronCount, ShockingLayer shockingLayer) : base(name, neuronCount, shockingLayer)
        {
            OnResult += (sender, doubles) =>
            {
                Console.WriteLine("Results from " + name);
                Console.WriteLine(JsonConvert.SerializeObject(doubles));
            };
        }
    }
}