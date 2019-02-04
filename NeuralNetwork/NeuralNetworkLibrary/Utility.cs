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
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    stringBuilder.Append($"{Math.Round(matrix[i, j], 2)} ");
                }

                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        public static string PrintVector(double[] vector)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < vector.Length; i++)
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
            var results = new double[matrix.GetLength(0)];
            for (var i = 0; i < results.Length; i++)
            {
                for (var j = 0; j < vector.Length; j++)
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
            var results = new double[vector1.Length];
            for (var i = 0; i < vector1.Length; i++)
            {
                results[i] = vector1[i] + vector2[i];
            }
            return results;
        }

        public static double[] Tanh(double[] vector)
        {
            var results = new double[vector.Length];
            for (var i = 0; i < vector.Length; i++)
            {
                results[i] = Math.Tanh(vector[i]);
            }
            return results;
        }

        public static double Cost(double[] results, double[] desired)
        {
            if (results.Length != desired.Length) return -1;

            double sum = 0;
            for (var i = 0; i < results.Length; i++)
            {
                sum += Math.Pow(results[i] - desired[i], 2);
            }

            return sum;
        }
    }
}