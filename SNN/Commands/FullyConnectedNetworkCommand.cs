using SNN.Models;
using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNN.Commands
{
    public class FullyConnectedNetworkCommand : CommandBase
    {
        private readonly NetworkConfigurationViewModel _networkConfigViewModel;

        public FullyConnectedNetworkCommand(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _networkConfigViewModel = networkConfigViewModel;
        }
        public override void Execute(object parameter)
        {
            for (int i = 0; i < _networkConfigViewModel.Neurons.Count; i++)
            {
                for (int j = i + 1; j < _networkConfigViewModel.Neurons.Count; j++)
                {
                    WeightViewModel weight = new WeightViewModel(_networkConfigViewModel.Neurons[i], _networkConfigViewModel.Neurons[j]);
                    if (!_networkConfigViewModel.Weights.Contains(weight))
                    {
                        _networkConfigViewModel.Weights.Add(weight);
                        weight.ValueFirstToSecond = _networkConfigViewModel.SetInitialWeight();
                        weight.ValueSecondToFirst = _networkConfigViewModel.SetInitialWeight();
                        if (_networkConfigViewModel.WeightMatrix != null)
                        {
                            _networkConfigViewModel.WeightMatrix.addSubscriptionWeight(weight);
                        }
                    }


                }
            }
        }
    }
}
