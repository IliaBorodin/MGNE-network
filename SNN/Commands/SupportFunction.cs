using SNN.Models;
using SNN.ViewModels;
using System.Collections.ObjectModel;

namespace SNN.Commands
{
    public class SupportFunction
    {
        private readonly NetworkConfigurationViewModel _vm;

        public SupportFunction(NetworkConfigurationViewModel networkConfigViewModel)
        {
            _vm = networkConfigViewModel;
        }
        public void ThresholdNeuron()
        {
            if (_vm.VisibleQ == true)
                return;
            else if(_vm.VisibleQ == false)
            {
                foreach(NeuronViewModel neuron in _vm.Neurons)
                {
                    neuron.ExternalInfluence = false;
                }
            }
        }
        public ObservableCollection<Neuron> GenerateNeurons()
        {
            var neuronsList = new ObservableCollection<Neuron>();
            ThresholdNeuron();
            foreach (var neuron in _vm.Neurons)
            {
                var neuronModel = new Neuron(neuron, neuron.SelectedConnectionType.Type, _vm.P, _vm.R, _vm.Alpha, _vm.Tr, neuron.ExternalInfluence, _vm.External);

                neuronModel.Visualization = neuron;
                neuronModel.Status = neuron.SelectedConnectionType.Type;
                neuronModel.MembranePotential = (decimal)(neuron.MembranePotential);
                neuronsList.Add(neuronModel);
            }           
            return neuronsList;
        }

        public SynapticConnection[,] CreateSynapticMatrix()
        {
            int neuronCount = _vm.Neurons.Count;
            SynapticConnection[,] synapticMatrix = new SynapticConnection[neuronCount, neuronCount];

            // Инициализация всех соединений с value = 0
            for (int i = 0; i < neuronCount; i++)
            {
                for (int j = 0; j < neuronCount; j++)
                {
                    synapticMatrix[i, j] = new SynapticConnection(_vm.Neurons[i], _vm.Neurons[j], 0);
                }
            }

            // Обновление матрицы на основе существующих соединений
            foreach (var weight in _vm.Weights)
            {
                int senderIndex = _vm.Neurons.IndexOf(weight.NeuronFirst);
                int receiverIndex = _vm.Neurons.IndexOf(weight.NeuronSecond);

                if (weight.SelectedConnectionType.Type == 1 || weight.SelectedConnectionType.Type == 2)
                {
                    synapticMatrix[senderIndex, receiverIndex] = new SynapticConnection(weight.NeuronFirst, weight.NeuronSecond, weight.ValueFirstToSecond);
                }

                if (weight.SelectedConnectionType.Type == 1 || weight.SelectedConnectionType.Type == 3)
                {
                    synapticMatrix[receiverIndex, senderIndex] = new SynapticConnection(weight.NeuronSecond, weight.NeuronFirst, weight.ValueSecondToFirst);
                }
            }

            return synapticMatrix;
        }

        public IndicatorSynapticImpact[,] CreateZeroImpactMatrix(ObservableCollection<Neuron> neurons)
        {
            int neuronCount = _vm.Neurons.Count;
            IndicatorSynapticImpact[,] impactMatrix = new IndicatorSynapticImpact[neuronCount, neuronCount];

            // Инициализация всех связей с value = 0
            for (int i = 0; i < neuronCount; i++)
            {
                for (int j = 0; j < neuronCount; j++)
                {
                    impactMatrix[i, j] = new IndicatorSynapticImpact(neurons[i], neurons[j], 0);
                }
            }

            return impactMatrix;
        }




        private ObservableCollection<ObservableCollection<IndicatorSynapticImpact>> ConvertToObservableCollection(IndicatorSynapticImpact[,] matrix)
        {
            var observableCollection = new ObservableCollection<ObservableCollection<IndicatorSynapticImpact>>();

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                var rowCollection = new ObservableCollection<IndicatorSynapticImpact>();
                for (int j = 0; j < columns; j++)
                {
                    rowCollection.Add(matrix[i, j]);
                }
                observableCollection.Add(rowCollection);
            }

            return observableCollection;
        }


        public ObservableCollection<ObservableCollection<IndicatorSynapticImpact>> CreateIndicatorSynapticImpactMatrix(ObservableCollection<Neuron> neurons)
        {
            return ConvertToObservableCollection(CreateZeroImpactMatrix(neurons));
        }

    }
}
