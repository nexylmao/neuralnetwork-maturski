using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security;
using System.Text;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace NeuralNetworkLibrary
{
    public static class Utility
    {
        public static string PrintMatrix(double[,] matrix)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    stringBuilder.Append($"{Math.Round(matrix[i, j], 2)} ");
                }

                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public static string PrintVector(double[] vector)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < vector.Length; i++)
            {
                stringBuilder.AppendLine($"{Math.Round(vector[i], 2)} ");
            }
            return stringBuilder.ToString();
        }

        public static double[] Multiply(double[,] matrix, double[] vector)
        {
            if (matrix.GetLength(1) != vector.Length)
            {
                return null;
            }
            double[] results = new double[matrix.GetLength(0)];
            for (int i = 0; i < results.Length; i++)
            {
                for (int j = 0; j < vector.Length; j++)
                {
                    results[i] += vector[j] * matrix[i, j];
                }
            }
            return results;
        }

        public static double[] AddUp(double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                return null;
            }
            double[] results = new double[vector1.Length];
            for (int i = 0; i < vector1.Length; i++)
            {
                results[i] = vector1[i] + vector2[i];
            }
            return results;
        }

        public static double[] Tanh(double[] vector)
        {
            double[] results = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                results[i] = Math.Tanh(vector[i]);
            }
            return results;
        }

        public static void SaveValues(ShockableLayer layer)
        {
            using (var package = new ExcelPackage())
            {
                var weights = package.Workbook.Worksheets.Add(layer.Name + " - Weights");
                var weightsRoot = weights.Cells["A1"];
                weightsRoot.Value = "WEIGHTS";
                weightsRoot.Style.Locked = true;
                for (var i = 0; i < layer.Weights.GetLength(0); i++)
                {
                    weights.Cells[1, i + 2].Value = "INPUT " + (i + 1);
                    weights.Cells[1, i + 2].Style.Locked = true;
                    weights.Cells[i + 2, 1].Value = "NEURON " + (i + 1);
                    weights.Cells[i + 2, 1].Style.Locked = true;
                }
                for (var i = 0; i < layer.Weights.GetLength(0); i++)
                {
                    for (var j = 0; j < layer.Weights.GetLength(1); j++)
                    {
                        weights.Cells[i + 2, j + 2].Value = layer.Weights[i, j];
                    }
                }

                var biases = package.Workbook.Worksheets.Add(layer.Name + " - Biases");
                biases.Cells[1, 1].Value = "BIASES";
                biases.Cells[2, 1].Value = "VALUES";
                for (var i = 0; i < layer.Biases.Length; i++)
                {
                    biases.Cells[1, i + 2].Value = "NEURON " + (i + 1);
                    biases.Cells[1, i + 2].Style.Locked = true;
                    biases.Cells[2, i + 2].Value = layer.Biases[i];
                }
                
                package.SaveAs(new FileStream(layer.Name + ".xlsx", FileMode.OpenOrCreate));
            }
        }

        class LayerConfig
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

        private static LayerConfig ToConfig(Layer layer)
        {
            return new LayerConfig(layer.NeuronCount, layer.GetType().Name);
        }

        private static void InternalSave(ShockableLayer layer, Dictionary<string, LayerConfig> configs)
        {
            if (layer == null) return;
            configs.Add(layer.Name, ToConfig(layer));
            if (layer.GetType().GetInterfaces().Contains(typeof(ShockingLayer)))
            {
                InternalSave(((ShockingLayer)layer).GetShockingLayer(), configs);
            }
        }
        
        public static void SaveNetwork(InputLayer layer)
        {
            if (layer == null) return;
            var layers = new Dictionary<string, LayerConfig> {{layer.Name, ToConfig(layer)}};
            InternalSave(layer.GetShockingLayer(), layers);

            Console.WriteLine(JsonConvert.SerializeObject(layers));
        }
        
        
    }
}