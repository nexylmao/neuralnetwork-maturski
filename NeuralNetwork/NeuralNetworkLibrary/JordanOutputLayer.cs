using System;
using System.IO;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class JordanOutputLayer : ShockableLayer
    {
        private double[] results;

        public double[] Results => results;

        public int Layers => jordanContextLayer.Layers;
        
        private JordanContextLayer contextLayer;
        
        public JordanOutputLayer(string name, int neuronCount, JordanContextLayer contextLayer, ShockingLayer shockingLayer) : base(name, neuronCount, shockingLayer)
        {
            this.contextLayer = contextLayer;
            OnResult += (sender, doubles) =>
            {
                results = doubles;
                contextLayer.Dequeue();
                contextLayer.Enqueue(results);
//                TextWriter tw = new StreamWriter("Result - " + DateTime.Now.ToBinary() + ".txt");
//                tw.WriteLine(JsonConvert.SerializeObject(doubles));
//                tw.Close();
            };
        }
    }
}