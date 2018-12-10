using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class Axon
    {
        protected List<Dendrite> targets;

        protected double value;

        public Axon()
        {
            targets = new List<Dendrite>();
        }

        public Axon(IEnumerable<Dendrite> targets)
        {
            foreach(Dendrite target in targets)
            {
                this.targets.Add(target);
                target.TargetedBy.Add(this);
            }
        }

        public double Value { get => value; set => this.value = value; }

        public void AddDendrite(Dendrite dendrite)
        {
            targets.Add(dendrite);
            dendrite.TargetedBy.Add(this);
        }
    }

    public class Dendrite
    {
        protected List<Axon> targetedBy;

        protected double weight;

        public EventHandler<double[]> ReceiveValue;

        public double Weight { get => weight; }

        public List<Axon> TargetedBy { get => targetedBy; set => targetedBy = value; }

        public Dendrite() { }

        public Dendrite(double weight)
        {
            this.weight = weight;
            this.targetedBy = new List<Axon>();
        }

        public void Pull()
        {
            List<double> values = new List<double>();
            foreach(Axon axon in targetedBy)
            {
                values.Add(axon.Value);
            }
            ReceiveValue(this, values.ToArray());
        }
    }
}
