using System.Collections.Generic;

namespace NeuralNetworkLibrary
{
    public class NeuralNetwork
    {
        private InputLayer inputLayer;

        private OutputLayer outputLayer;

        private List<HiddenLayer> hiddenLayer;

        public InputLayer InputLayer => inputLayer;

        public OutputLayer OutputLayer => outputLayer;

        public List<HiddenLayer> HiddenLayer => hiddenLayer;
    }
}