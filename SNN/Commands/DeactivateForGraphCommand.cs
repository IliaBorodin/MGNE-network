using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Commands
{
    internal class DeactivateForGraphCommand : CommandBase
    {
        private readonly NetworkDynamicsViewModel _networkDynamicsViewModel;

        public DeactivateForGraphCommand(NetworkDynamicsViewModel networkDynamicsViewModel)
        {
            _networkDynamicsViewModel = networkDynamicsViewModel;
        }
        public override void Execute(object parameter)
        {
            _networkDynamicsViewModel.CounterForGraph--;
            _networkDynamicsViewModel.SelectedNeuron.ReadyForGraph = true;
            _networkDynamicsViewModel.SelectedNeuron.ReadyNotForGraph = false;
            

        }
    }
}
