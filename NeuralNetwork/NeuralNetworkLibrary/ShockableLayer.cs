using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace NeuralNetworkLibrary
{
    public abstract class ShockableLayer : Layer
    {
        protected double[,] weights;

        protected double[] biases;

        protected ShockingLayer shockingLayer;

        public double[,] Weights => weights;

        public double[] Biases => biases;

        public ShockingLayer ShockingLayer => shockingLayer;

        public event EventHandler<double[]> OnResult; 
        
        public ShockableLayer(string name, int neuronCount, ShockingLayer shockingLayer) : base(name, neuronCount)
        {
            this.shockingLayer = shockingLayer;
            if (this.shockingLayer.GetShockingLayer() != this)
            {
                this.shockingLayer.SetShockingLayer(this);
            }

            biases = new double[neuronCount];
            weights = new double[neuronCount, this.shockingLayer.GetNeuronCount()];
            var r = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < weights.GetLength(0); i++)
            {
                for (var j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = (r.NextDouble() * 2) - 1;
                }
            }
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
            var multiplication = Utility.Multiply(weights, values);
            var addition = Utility.AddUp(multiplication, biases);
            var result = Utility.Tanh(addition);
            
            OnResult.Invoke(this, result);
        }
        
        public void SaveValues()
        {
            using (var package = new ExcelPackage())
            {
                var weights = package.Workbook.Worksheets.Add(Name + " - Weights");
                var weightsRoot = weights.Cells["A1"];
                weightsRoot.Value = "WEIGHTS";
                weightsRoot.Style.Locked = true;
                for (var i = 0; i < Weights.GetLength(1); i++)
                {
                    weights.Cells[1, i + 2].Value = "INPUT " + (i + 1);
                    weights.Cells[1, i + 2].Style.Locked = true;
                }
                for (var i = 0; i < Weights.GetLength(0); i++)
                {
                    weights.Cells[i + 2, 1].Value = "NEURON " + (i + 1);
                    weights.Cells[i + 2, 1].Style.Locked = true;
                }
                for (var i = 0; i < Weights.GetLength(0); i++)
                {
                    for (var j = 0; j < Weights.GetLength(1); j++)
                    {
                        weights.Cells[i + 2, j + 2].Value = Weights[i, j];
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
                        while (sheet.Cells[neurons + 2, 1].Value != null)
                        {
                            neurons++;
                        }

                        while (sheet.Cells[1, inputs + 2].Value != null)
                        {
                            inputs++;
                        }

                        for (var i = 0; i < neurons; i++)
                        {
                            for (var j = 0; j < inputs; j++)
                            {
                                weights[i, j] = double.Parse(sheet.Cells[i + 2, j + 2].Value.ToString());
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