using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNN.Commands
{
    internal class DeleteNeuronCommand : CommandBase
    {

        private readonly NetworkConfigurationViewModel _networkConfigViewModel;

        public DeleteNeuronCommand(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _networkConfigViewModel = networkConfigViewModel;
        }

        public override void Execute(object parameter)
        {
            // Создаем список для хранения элементов, которые нужно удалить
            var elementsToRemove = _networkConfigViewModel.Weights
                .Where(element => element.NeuronFirst == _networkConfigViewModel.SelectedNeuron ||
                                  element.NeuronSecond == _networkConfigViewModel.SelectedNeuron)
                .ToList();

            // Удаляем элементы из исходной коллекции после итерации
            foreach (var element in elementsToRemove)
            {
                _networkConfigViewModel.Weights.Remove(element);
            }
            _networkConfigViewModel.Neurons.Remove(_networkConfigViewModel.SelectedNeuron);
            _networkConfigViewModel.IsTab1Visible = false;
            _networkConfigViewModel.IsTab2Visible = false;
            _networkConfigViewModel.SetInitialWeights();

        }
    }
}
