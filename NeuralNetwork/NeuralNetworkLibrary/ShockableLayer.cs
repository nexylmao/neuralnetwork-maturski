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