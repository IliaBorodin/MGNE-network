using SNN.Models;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace SNN.ViewModels
{
    public class WeightMatrixViewModel : ViewModelBase
    {
        private ObservableCollection<NeuronViewModel> _neurons;
        private ObservableCollection<WeightViewModel> _weights;

        public WeightMatrixViewModel(ObservableCollection<NeuronViewModel> neurons, ObservableCollection<WeightViewModel> weights)
        {
            _neurons = neurons;
            _weights = weights;

            _neurons.CollectionChanged += OnCollectionChanged;
            _weights.CollectionChanged += OnCollectionChanged;

            foreach (var neuron in _neurons)
            {
                neuron.PropertyChanged += (s, e) => OnPropertyChanged(nameof(Rows));
            }

            foreach (var weight in _weights)
            {
                weight.PropertyChanged += (s, e) => OnPropertyChanged(nameof(Rows));
            }
        }

        public void addSubscriptionNeuron(NeuronViewModel neuron)
        {
            neuron.PropertyChanged += (s, e) => OnPropertyChanged(nameof(Rows));
        }
        public void addSubscriptionWeight(WeightViewModel weight)
        {
            weight.PropertyChanged += (s, e) => OnPropertyChanged(nameof(Rows));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Rows));
        }

        public ObservableCollection<WeightMatrixRow> Rows
        {
            get
            {
                var rows = new ObservableCollection<WeightMatrixRow>();

                foreach (var neuron in _neurons)
                {
                    var row = new WeightMatrixRow
                    {
                        NeuronName = neuron.Name
                    };

                    foreach (var targetNeuron in _neurons)
                    {
                        var weight = _weights.FirstOrDefault(w =>
                            (w.NeuronFirst == neuron && w.NeuronSecond == targetNeuron) ||
                            (w.NeuronFirst == targetNeuron && w.NeuronSecond == neuron));

                        if (weight != null)
                        {
                            // Определяем, какой из нейронов первый в связи
                            if (weight.NeuronFirst == neuron)
                            {
                                row.Weights.Add(weight.ValueFirstToSecond);
                            }
                            else
                            {
                                row.Weights.Add(weight.ValueSecondToFirst);
                            }
                        }
                        else
                        {
                            row.Weights.Add(0); // Если связь отсутствует, добавляем ноль
                        }
                    }

                    rows.Add(row);
                }


                return rows;
            }
            
        }

     
     
    }

    
}
