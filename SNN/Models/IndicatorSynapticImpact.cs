using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Models
{
    public class IndicatorSynapticImpact
    {
        private Neuron _sender;
        private Neuron _receiver;
        private int _value;
        public Neuron Sender {
            get { return _sender; }
            set { _sender = value; }
        }
        public Neuron Receiver {
            get { return _receiver; }
            set { _receiver = value; }
        }
        public int Value {
            get { return _value;}
            set {  _value = value; }
        }


        public IndicatorSynapticImpact(Neuron sender, Neuron receiver, int value)
        {
            _sender = sender;
            _receiver = receiver;
            _value = value;
        }

    }
}
