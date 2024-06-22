using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNN.Commands
{
    public class AddQValueParameterCommand : CommandBase
    {
        private readonly NetworkConfigurationViewModel _networkConfigViewModel;

        public AddQValueParameterCommand(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _networkConfigViewModel = networkConfigViewModel;
        }

        public override void Execute(object parameter)
        {
            foreach(NeuronViewModel neuron in  _networkConfigViewModel.Neurons)
            {
                if(neuron.ReadyForExternal == false)
                {
                    MessageBox.Show("Один из нейронов уже получает внешнее воздействие");
                    return;
                } 
            }
            _networkConfigViewModel.SelectedNeuron.ExternalInfluence = true;
            _networkConfigViewModel.SelectedNeuron.ReadyForExternal = false;
            _networkConfigViewModel.SelectedNeuron.ReadyNotForExternal = true;


        }
    }
}
