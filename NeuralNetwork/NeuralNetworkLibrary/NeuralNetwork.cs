using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public class NeuralNetwork
    {
        public static void LoadWeightsAndBiases(ShockableLayer shockableLayer)
        {
            shockableLayer.LoadValues();
        }
        
        public static NeuralNetwork LoadNetwork(string networkName)
        {
            TextReader textReader = new StreamReader(networkName + ".json");
            var configs = JsonConvert.DeserializeObject<Dictionary<string, LayerConfig>>(textReader.ReadToEnd());
            textReader.Close();

            NeuralNetwork neuralNetwork = new NeuralNetwork(networkName);
            
            ShockingLayer last = null;
            foreach (var kvp in configs)
            {
                switch (kvp.Value.Type)
                {
                        case "InputLayer":
                            last = new InputLayer(kvp.Key, kvp.Value.NeuronCount);
                            neuralNetwork.inputLayer = (InputLayer)last;
                            break;
                        case "HiddenLayer":
                            last = new HiddenLayer(kvp.Key, kvp.Value.NeuronCount, last);
                            LoadWeightsAndBiases((ShockableLayer)last);
                            neuralNetwork.hiddenLayers.Add((HiddenLayer)last);
                            break;
                        case "OutputLayer":
                            neuralNetwork.outputLayer = new OutputLayer(kvp.Key, kvp.Value.NeuronCount, last);
                            LoadWeightsAndBiases(neuralNetwork.outputLayer);
                            break;
                        case "ConsoleOutputLayer":
                            neuralNetwork.outputLayer = new ConsoleOutputLayer(kvp.Key, kvp.Value.NeuronCount, last);
                            LoadWeightsAndBiases(neuralNetwork.outputLayer);
                            break;
                }
            }
            
            return neuralNetwork;
        }
        
        private string name;
        
        private InputLayer inputLayer;

        private OutputLayer outputLayer;

        private List<HiddenLayer> hiddenLayers;

        public string Name => name;

        public InputLayer InputLayer => inputLayer;

        public OutputLayer OutputLayer => outputLayer;

        public List<HiddenLayer> HiddenLayers => hiddenLayers;

        public NeuralNetwork(string name)
        {
            this.name = name;
            hiddenLayers = new List<HiddenLayer>();
        }

        public NeuralNetwork(string name, InputLayer inputLayer)
        {
            this.name = name;
            hiddenLayers = new List<HiddenLayer>();

            this.inputLayer = inputLayer;
            RecurseLayerAddition(inputLayer.GetShockingLayer());
        }

        private void RecurseLayerAddition(ShockableLayer shockableLayer)
        {
            if (shockableLayer != null)
            {
                if (shockableLayer.GetType() == typeof(HiddenLayer))
                {
                    HiddenLayer hiddenLayer = (HiddenLayer) shockableLayer;
                    hiddenLayers.Add(hiddenLayer);
                    RecurseLayerAddition(hiddenLayer.GetShockingLayer());
                }
                else
                {
                    this.outputLayer = (OutputLayer)shockableLayer;
                }
            }
        }
        
        private class LayerConfig
        {
            private int neuronCount;
            private string type;

            public int NeuronCount => neuronCount;

            public string Type => type;

            public LayerConfig(int neuronCount, string type)
            {
                this.neuronCount = neuronCount;
                this.type = type;
            }
        }

        private LayerConfig ToConfig(Layer layer)
        {
            return new LayerConfig(layer.NeuronCount, layer.GetType().Name);
        }

        private void InternalSave(ShockableLayer layer, Dictionary<string, LayerConfig> configs)
        {
            if (layer == null) return;
            configs.Add(layer.Name, ToConfig(layer));
            layer.SaveValues();
            if (((IList) layer.GetType().GetInterfaces()).Contains(typeof(ShockingLayer)))
            {
                InternalSave(((ShockingLayer)layer).GetShockingLayer(), configs);
            }
        }
        
        public void SaveNetwork()
        {
            if (inputLayer == null) return;
            var layers = new Dictionary<string, LayerConfig> {{inputLayer.Name, ToConfig(inputLayer)}};
            InternalSave(inputLayer.GetShockingLayer(), layers);
            TextWriter textWriter = new StreamWriter(name + ".json");
            textWriter.Write(JsonConvert.SerializeObject(layers, Formatting.Indented));
            textWriter.Close();
        }
    }
}