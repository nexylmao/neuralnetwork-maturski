using System;
using System.Text;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public abstract class ShockableLayer : Layer
    {
        protected double[,] weights;

        protected double[] biases;

        protected ShockingLayer shockingLayer;

        public double[,] Weights => weights;

        public double[] Biases => biases;

        public ShockingLayer ShockingLayer => shockingLayer;

        public event EventHandler<double[]> OnResult; 
        
        public ShockableLayer(string name, int neuronCount, ShockingLayer shockingLayer) : base(name, neuronCount)
        {
            this.shockingLayer = shockingLayer;
            if (this.shockingLayer.GetShockingLayer() != this)
            {
                this.shockingLayer.SetShockingLayer(this);
            }

            biases = new double[neuronCount];
            weights = new double[neuronCount, this.shockingLayer.GetNeuronCount()];
            var r = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < weights.GetLength(0); i++)
            {
                for (var j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = (r.NextDouble() * 2) - 1;
                }
            }
        }

        public void SetShockingLayer(ShockingLayer shockingLayer)
        {
            this.shockingLayer = shockingLayer;
            if (this.shockingLayer.GetShockingLayer() != this)
            {
                this.shockingLayer.SetShockingLayer(this);
            }
        }
        
        public void Shock(double[] values)
        {
            var multiplication = Utility.Multiply(weights, values);
            var addition = Utility.AddUp(multiplication, biases);
            var result = Utility.Tanh(addition);
            
            OnResult.Invoke(this, result);
        }
    }
}