using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Models
{
    public class SynapticConnection
    {
        private NeuronViewModel _sender;
        private NeuronViewModel _receiver;
        private double _value;
        public NeuronViewModel Sender {
            get { return _sender; }
            set { _sender = value; }
        }
        public NeuronViewModel Receiver {
            get { return _receiver; }
            private set { _receiver = value; }
        }
        public Double Value {
            get { return _value; }
            set { _value = value; }
        }


        public SynapticConnection(NeuronViewModel sender, NeuronViewModel receiver, double value)
        {
            _sender = sender;
            _receiver = receiver;
            _value = value;
        }

    }
}
