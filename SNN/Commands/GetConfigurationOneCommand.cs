using SNN.Models;
using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNN.Commands
{
    public class GetConfigurationOneCommand : CommandBase
    {
        private readonly NetworkConfigurationViewModel _networkConfigViewModel;

        public GetConfigurationOneCommand(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _networkConfigViewModel = networkConfigViewModel;
        }
        public override void Execute(object parameter)
        {
            _networkConfigViewModel.Neurons.Clear();
            _networkConfigViewModel.Weights.Clear();
            _networkConfigViewModel.Neurons = new ObservableCollection<NeuronViewModel>
            {
                new NeuronViewModel("1",  new Point(50,50),_networkConfigViewModel.R, _networkConfigViewModel.P, 15, 1),
                new NeuronViewModel("2",  new Point(50,25),_networkConfigViewModel.R, _networkConfigViewModel.P, 13, 1),
                new NeuronViewModel("3",  new Point(100,50),_networkConfigViewModel.R, _networkConfigViewModel.P, 11, 1),
                new NeuronViewModel("4",  new Point(100,75),_networkConfigViewModel.R, _networkConfigViewModel.P, 9, 1),
                new NeuronViewModel("5",  new Point(50,100),_networkConfigViewModel.R, _networkConfigViewModel.P, 7, 1),
                new NeuronViewModel("6",  new Point(50,75),_networkConfigViewModel.R, _networkConfigViewModel.P, 5, 1),

            };

        }
    }
}
