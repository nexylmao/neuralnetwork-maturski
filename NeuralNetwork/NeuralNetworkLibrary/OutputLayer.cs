using System;
using System.IO;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class OutputLayer : ShockableLayer
    {
        private double[] results;

        public double[] Results => results;

        public OutputLayer(string name, int neuronCount, ShockingLayer shockingLayer) : base(name, neuronCount, shockingLayer)
        {
            OnResult += (sender, doubles) =>
            {
                results = doubles;
//                TextWriter tw = new StreamWriter("Result - " + DateTime.Now.ToBinary() + ".txt");
//                tw.WriteLine(JsonConvert.SerializeObject(doubles));
//                tw.Close();
            };
        }
    }
}