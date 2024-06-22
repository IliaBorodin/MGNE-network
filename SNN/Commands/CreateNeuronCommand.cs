using SNN.Models;
using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNN.Commands
{
    public class CreateNeuronCommand : CommandBase
    {

        private readonly NetworkConfigurationViewModel _networkConfigViewModel;

        public CreateNeuronCommand(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _networkConfigViewModel = networkConfigViewModel;
        }

        public override void Execute(object parameter)
        {
            NeuronViewModel neuron = new NeuronViewModel("1", new Point(0, 0), _networkConfigViewModel.R, _networkConfigViewModel.P);
            neuron.Name = "id" + _networkConfigViewModel.Neurons.Count;
            if (_networkConfigViewModel.takeNeuronInNeurons(neuron.Id) is null)
            {
                _networkConfigViewModel.Neurons.Insert(0, neuron);
                _networkConfigViewModel.SelectedNeuron = neuron;
                _networkConfigViewModel.SetInitialWeights();
                neuron.SetInitialMembranePotential();
                if (_networkConfigViewModel.WeightMatrix != null)
                {
                    _networkConfigViewModel.WeightMatrix.addSubscriptionNeuron(neuron);
                }
            }

        }
    }
}
