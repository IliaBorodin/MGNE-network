using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Commands
{
    internal class DeleteWeightCommand : CommandBase
    {
        private readonly NetworkConfigurationViewModel _networkConfigViewModel;
        public DeleteWeightCommand(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _networkConfigViewModel = networkConfigViewModel;
        }

        public override void Execute(object parameter)
        {
            _networkConfigViewModel.Weights.Remove(_networkConfigViewModel.SelectedWeight);
            _networkConfigViewModel.IsTab1Visible = false;
            _networkConfigViewModel.IsTab2Visible = false;
        }
    }
}
