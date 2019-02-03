namespace NeuralNetworkLibrary
{
    public interface ShockingLayer
    {
        bool Shock();

        ShockableLayer GetShockingLayer();

        void SetShockingLayer(ShockableLayer layer);

        int GetNeuronCount();
    }
}