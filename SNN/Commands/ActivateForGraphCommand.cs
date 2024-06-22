using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNN.Commands
{
    internal class ActivateForGraphCommand : CommandBase
    {
        private readonly NetworkDynamicsViewModel _networkDynamicsViewModel;

        public ActivateForGraphCommand(NetworkDynamicsViewModel networkDynamicsViewModel)
        {
            _networkDynamicsViewModel = networkDynamicsViewModel;
        }
        public override void Execute(object parameter)
        {
                if(_networkDynamicsViewModel.CounterForGraph == 2)
            {
                MessageBox.Show("Выбрано max нейронов");
                return;
            }
                _networkDynamicsViewModel.CounterForGraph++;
                if (_networkDynamicsViewModel.CounterForGraph == 1)
                {
                    _networkDynamicsViewModel.SelectedNeuron.ReadyForGraph = false;
                    _networkDynamicsViewModel.SelectedNeuron.ReadyNotForGraph = true;
                    //_networkDynamicsViewModel.GraphNeuronFirst = _networkDynamicsViewModel.SelectedNeuron;
                
                }
                if(_networkDynamicsViewModel.CounterForGraph == 2)
                {
                    _networkDynamicsViewModel.SelectedNeuron.ReadyForGraph = false;
                    _networkDynamicsViewModel.SelectedNeuron.ReadyNotForGraph = true;
                    //_networkDynamicsViewModel.GraphNeuronSecond = _networkDynamicsViewModel.SelectedNeuron;

                }
          
        }
    }
}
