using System;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class JordanHiddenLayer : ShockableLayer, ShockingLayer
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

        public JordanHiddenLayer(string name, int neuronCount, JordanContextLayer contextLayer, ShockingLayer shockingLayer) : base(name, neuronCount, contextLayer, shockingLayer)
        {
            OnResult += (sender, doubles) =>
            {
                values = doubles;
                Shock();
            };
        }
    }
}