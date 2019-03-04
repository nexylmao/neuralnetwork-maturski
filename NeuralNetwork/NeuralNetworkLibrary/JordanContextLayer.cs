using System;
using System.Collections.Generic;

namespace NeuralNetworkLibrary
{
    public class JordanContextLayer
    {
        private static Queue<double[]> values;

        private static int neurons, layers;

        private static double[] current;

        public int Neurons => neurons;
        
        public int Layers => layers;

        public double[] Current => current;

        public JordanContextLayer(int neurons, int layers)
        {
            JordanContextLayer.neurons = neurons;
            JordanContextLayer.layers = layers;
            JordanContextLayer.values = new Queue<double[]>(layers);

            for (var i = 0; i < layers; i++)
            {
                JordanContextLayer.values.Enqueue(new double[JordanContextLayer.neurons]);
            }
        }

        private bool Check(double[] values)
        {
            if (values.Length != JordanContextLayer.neurons)
            {
                return false;
            }

            return true;
        }

        public void Enqueue(double[] values)
        {
            if (!Check(values))
            {
                throw new Exception("Number of values passed through is not valid.");
            }
            JordanContextLayer.values.Enqueue(values);
        }

        public void Dequeue()
        {
            JordanContextLayer.current = JordanContextLayer.values.Dequeue();
        }
    }
}