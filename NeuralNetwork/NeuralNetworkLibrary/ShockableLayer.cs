using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;

namespace NeuralNetworkLibrary
{
    public class BackpropData
    {
        private string name;
        
        private double[] received;

        private double[] preactivated;

        private double[] calculated;

        public string Name => name;
        
        public double[] Received => received;

        public double[] Preactivated => preactivated;

        public double[] Calculated => calculated;

        public BackpropData(string name, double[] received, double[] preactivated, double[] calculated)
        {
            this.name = name;
            this.received = received;
            this.preactivated = preactivated;
            this.calculated = calculated;
        }
    }
    
    public abstract class ShockableLayer : Layer
    {
        protected double[,] weights;

        protected double[] biases;

        protected ShockingLayer shockingLayer;

        protected ElmanContextLayer elmanContextLayer;
        
        protected JordanContextLayer jordanContextLayer;

        public double[,] Weights => weights;

        public double[] Biases => biases;

        public ShockingLayer ShockingLayer => shockingLayer;

        public event EventHandler<double[]> OnResult;

        public event EventHandler<BackpropData> OnBackpropData;
        
        public ShockableLayer(string name, int neuronCount, ShockingLayer shockingLayer) : base(name, neuronCount)
        {
            createLayer(shockingLayer);
        }

        public ShockableLayer(string name, int neuronCount, int contextLayers, ShockingLayer shockingLayer) : base(name, neuronCount)
        {
            createElmanContext(contextLayers);
            createLayer(shockingLayer);
        }

        public ShockableLayer(string name, int neuronCount, JordanContextLayer contextLayer, ShockingLayer shockingLayer) : base(name, neuronCount)
        {
            this.jordanContextLayer = contextLayer;
            createLayer(shockingLayer);
        }
      
        private void createLayer(ShockingLayer shockingLayer)
        {
            this.shockingLayer = shockingLayer;
            if (this.shockingLayer.GetShockingLayer() != this)
            {
                this.shockingLayer.SetShockingLayer(this);
            }

            biases = new double[neuronCount];
            if (elmanContextLayer != null)
            {
                weights = new double[neuronCount, this.shockingLayer.GetNeuronCount() + neuronCount];
            }
            else if (jordanContextLayer != null)
            {
                weights = new double[neuronCount, this.shockingLayer.GetNeuronCount() + jordanContextLayer.Neurons];
            }
            else
            {
                weights = new double[neuronCount, this.shockingLayer.GetNeuronCount()];   
            }
            var r = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < weights.GetLength(0); i++)
            {
                for (var j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = (r.NextDouble() * 2) - 1;
                }
            }
        }

        private void createElmanContext(int contextLayers)
        {
            elmanContextLayer = new ElmanContextLayer(neuronCount, contextLayers);
        }
        
        public void SetShockingLayer(ShockingLayer shockingLayer)
        {
            this.shockingLayer = shockingLayer;
            if (this.shockingLayer.GetShockingLayer() != this)
            {
                this.shockingLayer.SetShockingLayer(this);
            }
        }

        public void Shock(double[] values)
        {
            double[] multiplication = null;
            double[] addition = null;
            double[] result = null;
            
            if (elmanContextLayer != null)
            {
                var contextValues = elmanContextLayer.Dequeue();
                var newValues = new List<double>();
                newValues.AddRange(values);
                newValues.AddRange(contextValues);
                values = newValues.ToArray();
            }
            else if (jordanContextLayer != null)
            {
                var contextValues = jordanContextLayer.Current;
                var newValues = new List<double>();
                newValues.AddRange(values);
                newValues.AddRange(contextValues);
                values = newValues.ToArray();
            }
            
            multiplication = Utility.Multiply(weights, values);
            addition = Utility.AddUp(multiplication, biases);
            result = Utility.Tanh(addition);

            elmanContextLayer?.Enqueue(result);
            
            OnResult?.Invoke(this, result);
            OnBackpropData?.Invoke(this, new BackpropData(name, values, addition, result));
        }
        
        public void SaveValues()
        {
            Console.WriteLine("Saving ({0})...", name);
            using (var package = new ExcelPackage())
            {
                var weights = package.Workbook.Worksheets.Add(Name + " - Weights");
                var weightsRoot = weights.Cells["A1"];
                weightsRoot.Value = "WEIGHTS";
                weightsRoot.Style.Locked = true;
                for (var i = 0; i < Weights.GetLength(0); i++)
                {
                    weights.Cells[1, i + 2].Value = "NEURON " + (i + 1);
                    weights.Cells[1, i + 2].Style.Locked = true;
                }
                for (var i = 0; i < Weights.GetLength(1); i++)
                {
                    weights.Cells[i + 2, 1].Value = "INPUT " + (i + 1);
                    weights.Cells[i + 2, 1].Style.Locked = true;
                }
                for (var i = 0; i < Weights.GetLength(0); i++)
                {
                    for (var j = 0; j < Weights.GetLength(1); j++)
                    {
                        weights.Cells[j + 2, i + 2].Value = Weights[i, j];
                    }
                }

                var biases = package.Workbook.Worksheets.Add(Name + " - Biases");
                biases.Cells[1, 1].Value = "BIASES";
                biases.Cells[2, 1].Value = "VALUES";
                for (var i = 0; i < Biases.Length; i++)
                {
                    biases.Cells[1, i + 2].Value = "NEURON " + (i + 1);
                    biases.Cells[1, i + 2].Style.Locked = true;
                    biases.Cells[2, i + 2].Value = Biases[i];
                }

                package.SaveAs(new FileStream(Name + ".xlsx", FileMode.OpenOrCreate));
                Console.WriteLine("Saved.");
            }
        }

        public void LoadValues()
        {
            FileStream file = new FileStream(Name + ".xlsx", FileMode.Open);
            using (var package = new ExcelPackage(file))
            {
                int neurons = 0, inputs = 0;
                foreach (var sheet in package.Workbook.Worksheets)
                {
                    if (sheet.Name.Contains("Weights"))
                    {
                        // rows
                        while (sheet.Cells[inputs + 2, 1].Value != null)
                        {
                            inputs++;
                        }

                        // columns
                        while (sheet.Cells[1, neurons + 2].Value != null)
                        {
                            neurons++;
                        }

                        for (var i = 0; i < inputs; i++)
                        {
                            for (var j = 0; j < neurons; j++)
                            {
                                weights[j, i] = double.Parse(sheet.Cells[i + 2, j + 2].Value.ToString());
                            }
                        }
                    }
                    else
                    {
                        for (var i = 0; i < neurons; i++)
                        {
                            biases[i] = double.Parse(sheet.Cells[2, i + 2].Value.ToString());
                        }
                    }
                }
            }
        }
    }
}