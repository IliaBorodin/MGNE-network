using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNN.Commands
{
    internal class CreateWeightCommand : CommandBase
    {

        private readonly NetworkConfigurationViewModel _networkConfigViewModel;

        public CreateWeightCommand(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _networkConfigViewModel = networkConfigViewModel;
        }

        public override void Execute(object parameter)
        {
            _networkConfigViewModel.Flag++;
            if(_networkConfigViewModel.Flag == 2)
            {
                WeightViewModel weight = new WeightViewModel(_networkConfigViewModel.SelectedNeuronSecond, _networkConfigViewModel.SelectedNeuron);
                if (!_networkConfigViewModel.Weights.Contains(weight))
                {
                    _networkConfigViewModel.Weights.Add(weight);
                    //Инициализация веса
                    weight.ValueFirstToSecond = _networkConfigViewModel.SetInitialWeight();
                    weight.ValueSecondToFirst = _networkConfigViewModel.SetInitialWeight();

                    if (_networkConfigViewModel.WeightMatrix != null)
                    {
                        _networkConfigViewModel.WeightMatrix.addSubscriptionWeight(weight);
                    }
                   // MessageBox.Show($"{_networkConfigViewModel.Weights.Count}");                   
                }
                _networkConfigViewModel.Flag = 0;
            }
            if(_networkConfigViewModel.Flag == 1)
            {
                _networkConfigViewModel.SelectedNeuronSecond = _networkConfigViewModel.SelectedNeuron;
            }
        }
    }
}
