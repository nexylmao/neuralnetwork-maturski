using System;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class HiddenLayer : ShockableLayer, ShockingLayer
    {
        private ShockableLayer target;
        
        public bool Shock()
        {
            if (target != null)
            {
                target.Shock(values);
                return true;
            }
            else
            {
                return false;
            }
        }

        public ShockableLayer GetShockingLayer()
        {
            return target;
        }

        public void SetShockingLayer(ShockableLayer layer)
        {
            target = layer;
        }

        public int GetNeuronCount()
        {
            return neuronCount;
        }

        public HiddenLayer(string name, int neuronCount, ShockingLayer shockingLayer) : base(name, neuronCount, shockingLayer)
        {
            OnResult += (sender, doubles) =>
            {
                Shock();
            };
        }
    }
}