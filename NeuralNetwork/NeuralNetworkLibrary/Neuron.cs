using System;
using System.Collections.Generic;

namespace NeuralNetworkLibrary
{
    public class Neuron
    {
        public Pulse OutputPulse { get; private set; }

        public List<Dendrite> Dendrites { get; private set; }

        public double Weight { get; set; }

        public Neuron()
        {
            Dendrites = new List<Dendrite>();
            OutputPulse = new Pulse();
        }

        public void CalculateOutput()
        {
            double sum = 0.0;
            foreach(Dendrite dendrite in Dendrites)
            {
                sum += dendrite.InputPulse.Value * dendrite.Weight;
            }
            OutputPulse.Value = Tanh(sum);
        }

        public static double Tanh(double value)
        {
            return Math.Tanh(value);
        }
    }
}
