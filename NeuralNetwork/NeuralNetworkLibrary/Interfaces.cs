using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public interface NeuralTransmitterLayer
    {
        double[] Emit();
    }

    public interface NeuralReceiverlayer
    {
        void Receive(double[] values);
    }

    public interface NeuralHiddenLayer
    {
        double[] Passthrough(double[] values);
    }
}
