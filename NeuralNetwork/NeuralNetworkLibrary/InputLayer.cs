using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class InputLayer : Layer, ShockingLayer
    {
        protected ShockableLayer shockingLayer;

        public ShockableLayer GetShockingLayer()
        {
            return shockingLayer;
        }

        public void SetShockingLayer(ShockableLayer layer)
        {
            shockingLayer = layer;
        }

        public int GetNeuronCount()
        {
            return neuronCount;
        }

        public InputLayer(string name, int neuronCount) : base(name, neuronCount)
        {
        }

        public InputLayer(string name, int neuronCount, ShockableLayer shockReceivingLayer) : base(name, neuronCount)
        {
            this.shockingLayer = shockReceivingLayer;
        }

        public double GetValue(int neuronIndex)
        {
            return values[neuronIndex];
        }

        public void SetValue(int neuronIndex, double value)
        {
            values[neuronIndex] = value;
        }

        public bool Shock()
        {
            if (shockingLayer != null)
            {
                shockingLayer.Shock(values);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}