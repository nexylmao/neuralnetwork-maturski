using System;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class ElmanHiddenLayer : HiddenLayer
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