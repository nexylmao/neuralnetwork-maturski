using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public interface NeuralEmitterLayer
    {
        void Emit();
    }

    public interface NeuralReceiverLayer
    {
        event EventHandler<double[]> Receive;
    }
}
