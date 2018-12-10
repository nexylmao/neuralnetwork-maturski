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

        public Axon()
        {
            targets = new List<Dendrite>();
        }

        public Axon(IEnumerable<Dendrite> targets)
        {
            foreach(Dendrite target in targets)
            {
                this.targets.Add(target);
                target.TargetedBy = this;
            }
        }

        public void AddDendrite(Dendrite dendrite)
        {
            targets.Add(dendrite);
            dendrite.TargetedBy = this;
        }

        public virtual void Emit(double value)
        {
            foreach(Dendrite target in targets)
            {
                target.ReceiveValue(this, value);
            }
        }
    }

    public class Dendrite
    {
        protected Axon targetedBy;

        protected double weight;

        public EventHandler<double> ReceiveValue;

        public double Weight { get => weight; }

        public Axon TargetedBy { get => targetedBy; set => targetedBy = value; }

        public Dendrite() { }

        public Dendrite(double weight)
        {
            this.weight = weight;
        }

        public Dendrite(double weight, Axon targetedBy)
        {
            this.targetedBy = targetedBy;
            this.weight = weight;
        }
    }
}
