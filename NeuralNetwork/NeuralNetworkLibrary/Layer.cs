using Newtonsoft.Json;

namespace NeuralNetworkLibrary
{
    public abstract class Layer
    {
        protected string name;
        
        protected int neuronCount;

        protected double[] values;

        public string Name => name;
        
        public int NeuronCount => neuronCount;

        public double[] Values => values;

        protected Layer(string name, int neuronCount)
        {
            this.name = name;
            this.neuronCount = neuronCount;
            values = new double[this.neuronCount];
        }
        
        public string Print()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}