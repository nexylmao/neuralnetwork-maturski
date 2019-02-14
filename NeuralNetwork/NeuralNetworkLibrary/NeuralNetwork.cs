using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace NeuralNetworkLibrary
{
    public class TrainingSet
    {
        private double[] inputs;

        private double[] results;
        
        public double[] Inputs => inputs;

        public double[] Results => results;

        public TrainingSet(double[] inputs, double[] results)
        {
            this.inputs = inputs;
            this.results = results;
        }
    }
    
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
            Console.WriteLine("Saving " + layer.Name);
            configs.Add(layer.Name, ToConfig(layer));
            layer.SaveValues();
            Console.WriteLine("Saved Values");
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

        public void Shock()
        {
            inputLayer.Shock();
        }

        public IEnumerable<BackpropData> BackpropShock()
        {
            List<BackpropData> data = new List<BackpropData>();
            EventHandler<BackpropData> handler = (sender, computed) =>
            {
                data.Add(computed);
            };
            foreach (var hiddenLayer in hiddenLayers)
            {
                hiddenLayer.OnBackpropData += handler;
            }
            outputLayer.OnBackpropData += handler;
            
            inputLayer.Shock();

            foreach (var hiddenLayer in hiddenLayers)
            {
                hiddenLayer.OnBackpropData -= handler;
            }
            outputLayer.OnBackpropData -= handler;

            return data;
        }

        public double[] Results => outputLayer.Results;

        public double[] Inputs => inputLayer.Values;

        public class TrainingResults
        {
            private string name;
            
            private double[,] weightDeltas;

            private double[] biasDeltas;

            private double learningRate;

            public string Name => name;
            
            public double[,] WeightDeltas
            {
                get
                {
                    double[,] rated = new double[weightDeltas.GetLength(0), weightDeltas.GetLength(1)];
                    for (var i = 0; i < weightDeltas.GetLength(0); i++)
                    {
                        for (var j = 0; j < weightDeltas.GetLength(1); j++)
                        {
                            rated[i, j] = weightDeltas[i, j] * learningRate;
                        }
                    }
                    return rated;
                }
            }

            public double[] BiasDeltas
            {
                get
                {
                    double[] rated = new double[biasDeltas.Length];
                    for (var i = 0; i < biasDeltas.Length; i++)
                    {
                        rated[i] = biasDeltas[i] * learningRate;
                    }
                    return rated;
                }
            }

            public double LearningRate
            {
                get => learningRate;
                set => learningRate = value;
            }

            public TrainingResults(string name, double[,] weightDeltas, double[] biasDeltas)
            {
                this.name = name;
                this.weightDeltas = weightDeltas;
                this.biasDeltas = biasDeltas;
                learningRate = 1;
            }

            public TrainingResults(string name, double[,] weightDeltas, double[] biasDeltas, double learningRate)
            {
                this.name = name;
                this.weightDeltas = weightDeltas;
                this.biasDeltas = biasDeltas;
                this.learningRate = learningRate;
            }

            public static TrainingResults operator +(TrainingResults tr1, TrainingResults tr2)
            {
                var l0 = tr1.WeightDeltas.GetLength(0);
                var l1 = tr1.WeightDeltas.GetLength(1);
                double[,] added = new double[l0, l1];
                for (var i = 0; i < l0; i++)
                {
                    for (var j = 0; j < l1; j++)
                    {
                        added[i, j] = tr1.WeightDeltas[i, j] + tr2.WeightDeltas[i, j];
                    }
                }

                var bl = tr1.BiasDeltas.Length;
                double[] addedBiases = new double[bl];
                for (var i = 0; i < bl; i++)
                {
                    addedBiases[i] = tr1.BiasDeltas[i] + tr2.BiasDeltas[i];
                }
                
                return new TrainingResults(tr1.Name, added, addedBiases);
            }

            public static TrainingResults AddUp(params TrainingResults[] results)
            {
                if (results.Length == 1)
                {
                    return results[0];
                }
                else if (results.Length < 1)
                {
                    return null;
                }
                else
                {
                    var result = results[0];
                    for (var i = 1; i < results.Length; i++)
                    {
                        result += results[i];
                    }
                    return result;
                }
            }

            public static TrainingResults AddUp(IEnumerable<TrainingResults> results)
            {
                List<TrainingResults> list = new List<TrainingResults>(results);
                if (list.Count == 1)
                {
                    return list[0];
                }
                else if (list.Count < 1)
                {
                    return null;
                }
                else
                {
                    var result = list[0];
                    for (var i = 1; i < list.Count; i++)
                    {
                        result += list[i];
                    }
                    return result;
                }
            }

            public void ApplyToLayer(ShockableLayer layer)
            {
                if (name == layer.Name)
                {
                    for (var i = 0; i < layer.Weights.GetLength(0); i++)
                    {
                        for (var j = 0; j < layer.Weights.GetLength(1); j++)
                        {
                            layer.Weights[i, j] -= WeightDeltas[i, j];
                        }
                    }

                    for (var i = 0; i < layer.Biases.Length; i++)
                    {
                        layer.Biases[i] -= BiasDeltas[i];
                    }
                }
            }
        }
        
        public void Train(string json)
        {
            TextReader tr = new StreamReader(json + ".json");
            List<TrainingSet> sets = JsonConvert.DeserializeObject<List<TrainingSet>>(tr.ReadToEnd());
            tr.Close();

            int batchesCount = 0;
            
            for (int x = 0, y = 10; x < sets.Count; x += 10, y = Math.Min(sets.Count - x, 10))
            {
                batchesCount++;
                List<TrainingResults> batchResults = new List<TrainingResults>();
                var batch = sets.GetRange(x, y);

                foreach (var set in batch)
                {
                    for (var i = 0; i < set.Inputs.Length; i++)
                    {
                        Inputs[i] = set.Inputs[i];
                    }
                    
                    List<BackpropData> data = BackpropShock().ToList();
    
                    var cost = Utility.Cost(Results, set.Results);
                    var deltas = new double[set.Results.Length];
                    var costActivations = new double[set.Results.Length];
                    
                    for (var i = 0; i < Results.Length; i++)
                    {
                        deltas[i] = Results[i] - set.Results[i];
                        costActivations[i] = 2 * deltas[i];
                    }
                    
                    ShockableLayer current = outputLayer;
                    while (current != null)
                    {
//                        Console.WriteLine("Propagating " + current.Name);
                        
                        // do back prop for weights and biases
                        // bpds - back propagation datas
                        // bpd - back propagation data
                        // w - weights
                        // b - biases
                        var bpds = from d in data
                            where d.Name == current.Name
                            select d;
                        var bpd = bpds.ToList()[0];
                        var w = current.Weights;
                        var b = current.Biases;
    
                        var newWeights = new double[w.GetLength(0), w.GetLength(1)];
                        var newBiases = new double[b.Length];
                        
    //                    Console.WriteLine("Old Weights");
    //                    Console.WriteLine(Utility.PrintMatrix(w));
    //                    Console.WriteLine("Old Biases");
    //                    Console.WriteLine(Utility.PrintVector(b));
    //                    Console.WriteLine("CostActivations d(E)/d(a)");
    //                    Console.WriteLine(JsonConvert.SerializeObject(costActivations));
    //                    Console.WriteLine("ReceivedValues a(L-1)");
    //                    Console.WriteLine(JsonConvert.SerializeObject(bpd.Received));
    //                    Console.WriteLine("PreActivatedValues z");
    //                    Console.WriteLine(JsonConvert.SerializeObject(bpd.Preactivated));
                        
                        for (var i = 0; i < costActivations.Length; i++)
                        {
    //                        Console.WriteLine("CostActivation");
    //                        Console.WriteLine(costActivations[i]);
    //                        Console.WriteLine("PreActivated");
    //                        Console.WriteLine(bpd.Preactivated[i]);
    
                            var derivativeTanh = Utility.DerivativeTanh(bpd.Preactivated[i]);
                            
                            // weights
                            for (var j = 0; j < w.GetLength(1); j++)
                            {
    //                            Console.WriteLine("Received");
    //                            Console.WriteLine(bpd.Received[j]);
    //                            Console.WriteLine("Preactivated " + bpd.Preactivated[i]);
    //                            Console.WriteLine("DerivativeTanh " + derivativeTanh);
                                newWeights[i, j] = bpd.Received[j] * derivativeTanh * costActivations[i];
                            }
                            
                            // bias
                            newBiases[i] = Utility.DerivativeTanh(bpd.Preactivated[i]) * costActivations[i];
                        }
                        
//                        Console.WriteLine("weight deltas");
//                        Console.WriteLine(Utility.PrintMatrix(newWeights));
    
//                        Console.WriteLine("bias deltas");
//                        Console.WriteLine(Utility.PrintVector(newBiases));
                        
                        batchResults.Add(new TrainingResults(current.Name, newWeights, newBiases));
                        
                        current = current.ShockingLayer as ShockableLayer;
                        if (current != null)
                        {
                            // do calculate costActivations
    //                        Console.WriteLine("W[{0},{1}]", w.GetLength(0), w.GetLength(1));
                            
                            var newCostActivations = new double[w.GetLength(1)];
                            for (var i = 0; i < newCostActivations.Length; i++)
                            {
                                double sum = 0;
                                for (var j = 0; j < costActivations.Length; j++)
                                {
    //                                Console.WriteLine("weight " + w[j,i]);
    //                                Console.WriteLine("derivative " + Utility.DerivativeTanh(bpd.Preactivated[j]));
    //                                Console.WriteLine("costActivaition " + costActivations[j]);
                                    sum += w[j, i] * Utility.DerivativeTanh(bpd.Preactivated[j]) * costActivations[j];
                                }
    
                                newCostActivations[i] = sum;
                            }
    
                            costActivations = newCostActivations;
                        }           
                    }   
                }

                Console.WriteLine("Batch Done~!");
//                Console.WriteLine(JsonConvert.SerializeObject(batchResults, Formatting.Indented));
                
                foreach (var hiddenLayer in hiddenLayers)
                {
                    var layerResults = from l in batchResults
                        where l.Name == hiddenLayer.Name
                        select l;

                    Console.WriteLine(JsonConvert.SerializeObject(layerResults, Formatting.Indented));
                    
                    var hiddenAdjustments = TrainingResults.AddUp(layerResults);
                    
                    Console.WriteLine("Got {0} {1} adjustments", layerResults.ToList().Count, hiddenLayer.Name);
                    Console.WriteLine("Summed adjustments");
                    Console.WriteLine(Utility.PrintMatrix(hiddenAdjustments.WeightDeltas));
                    Console.WriteLine(Utility.PrintVector(hiddenAdjustments.BiasDeltas));
                    hiddenAdjustments.LearningRate = 1;
                    Console.WriteLine("Summed adjustments * learning rate");
                    Console.WriteLine(Utility.PrintMatrix(hiddenAdjustments.WeightDeltas));
                    Console.WriteLine(Utility.PrintVector(hiddenAdjustments.BiasDeltas));
                    
                    hiddenAdjustments.ApplyToLayer(hiddenLayer);
                }
                
                var outputResults = from l in batchResults
                    where l.Name == outputLayer.Name
                    select l;

                var outputAdjustments = TrainingResults.AddUp(outputResults);
                
                Console.WriteLine("Got {0} {1} adjustments", outputResults.ToList().Count, outputLayer.Name);
                Console.WriteLine("Summed adjustments");
                Console.WriteLine(Utility.PrintMatrix(outputAdjustments.WeightDeltas));
                Console.WriteLine(Utility.PrintVector(outputAdjustments.BiasDeltas));
                outputAdjustments.LearningRate = 1;
                Console.WriteLine("Summed adjustments * learning rate");
                Console.WriteLine(Utility.PrintMatrix(outputAdjustments.WeightDeltas));
                Console.WriteLine(Utility.PrintVector(outputAdjustments.BiasDeltas));

                outputAdjustments.ApplyToLayer(outputLayer);
                
                Console.WriteLine("(x,y) = ({0},{1})", x, y);
            }

            foreach (var hiddenLayer in hiddenLayers)
            {
                hiddenLayer.SaveValues();
            }
            outputLayer.SaveValues();
            Console.WriteLine("Trained on {0} batches.", batchesCount);
        }
        
        public void TestNeuralNetwork(string json)
        {
            TextReader tr = new StreamReader(json + ".json");
            List<TrainingSet> sets = JsonConvert.DeserializeObject<List<TrainingSet>>(tr.ReadToEnd());
            tr.Close();

            TextWriter tw = new StreamWriter($"Results {Name} {DateTime.Now.ToBinary()}.txt");
            
            double costSum = 0;
            
            foreach (var set in sets)
            {
                for (var i = 0; i < set.Inputs.Length; i++)
                {
                    Inputs[i] = set.Inputs[i];
                }
                
                Shock();

                double cost = Utility.Cost(Results, set.Results);
                costSum += cost;
                
                string result = string.Format("I:{0}|W:{1}|R:{2}|C:{3}", JsonConvert.SerializeObject(set.Inputs), JsonConvert.SerializeObject(set.Results), JsonConvert.SerializeObject(Results), cost);

                Console.WriteLine(result);
                tw.WriteLine(result);
            }

            Console.WriteLine("Cost sum = {0}", costSum);
            tw.WriteLine("Cost sum = {0}", costSum);
            tw.Close();
        }
    }
}