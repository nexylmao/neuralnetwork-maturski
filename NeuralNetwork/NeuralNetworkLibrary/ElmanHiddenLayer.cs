using System;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class ElmanHiddenLayer : ShockableLayer, ShockingLayer
    {
        private ShockableLayer target;

        public int Layers => elmanContextLayer.Layers;
        
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

        public ElmanHiddenLayer(string name, int neuronCount, int contextLayers, ShockingLayer shockingLayer) : base(name, neuronCount, contextLayers, shockingLayer)
        {
            OnResult += (sender, doubles) =>
            {
                values = doubles;
                Shock();
            };
        }
    }
}