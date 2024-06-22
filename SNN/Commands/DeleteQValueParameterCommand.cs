using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Commands
{
    public class DeleteQValueParameterCommand:CommandBase
    {
        private readonly NetworkConfigurationViewModel _networkConfigViewModel;

        public DeleteQValueParameterCommand(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _networkConfigViewModel = networkConfigViewModel;
        }

        public override void Execute(object parameter)
        {
            _networkConfigViewModel.SelectedNeuron.ExternalInfluence = false;
            _networkConfigViewModel.SelectedNeuron.ReadyForExternal = true;
            _networkConfigViewModel.SelectedNeuron.ReadyNotForExternal = false;

        }
    }
}
