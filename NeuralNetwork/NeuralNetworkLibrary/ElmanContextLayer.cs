using System;
using System.Collections.Generic;

namespace NeuralNetworkLibrary
{
    public class ElmanContextLayer
    {
        private Queue<double[]> values;

        private int neurons, layers;

        public int Layers => layers;

        public ElmanContextLayer(int neurons, int layers)
        {
            this.neurons = neurons;
            this.layers = layers;
            this.values = new Queue<double[]>(layers);

            for (var i = 0; i < this.layers; i++)
            {
                this.values.Enqueue(new double[this.neurons]);
            }
        }

        private bool Check(double[] values)
        {
            if (values.Length != this.neurons)
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
            this.values.Enqueue(values);
        }

        public double[] Dequeue()
        {
            return this.values.Dequeue();
        }
    }
}