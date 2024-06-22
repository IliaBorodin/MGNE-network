using SNN.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace SNN.ViewModels
{


    public class WeightViewModel : ViewModelBase, IEquatable<WeightViewModel>
    {
        private NeuronViewModel _neuronFirst;
        private NeuronViewModel _neuronSecond;
        private double _x1Point;
        private double _x2Point;
        private double _y1Point;
        private double _y2Point;
        private Brush _colorLine = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9c9695"));
        private double _valueFirstToSecond;
        private double _valueSecondToFirst;

        private ObservableCollection<ConnectionType> _connectionTypes;
        private ConnectionType _selectedConnectionType;

        public ObservableCollection<ConnectionType> ConnectionTypes
        {
            get { return _connectionTypes; }
            set
            {
                _connectionTypes = value;
                OnPropertyChanged(nameof(ConnectionTypes));
            }
        }

        public ConnectionType SelectedConnectionType
        {
            get { return _selectedConnectionType; }
            set
            {
                _selectedConnectionType = value;
                if (_selectedConnectionType.Type == 2)
                {
                    ValueSecondToFirst = 0;
                }
                else if (_selectedConnectionType.Type == 3)
                {
                    ValueFirstToSecond = 0;
                }
                OnPropertyChanged(nameof(SelectedConnectionType));
            }
        }

       

        public double ValueFirstToSecond
        {
            get { return _valueFirstToSecond; }
            set
            {
                _valueFirstToSecond = value;            
                OnPropertyChanged(nameof(ValueFirstToSecond));              
            }
        }

       

        public double ValueSecondToFirst
        {
            get { return _valueSecondToFirst; }
            set
            {             
                _valueSecondToFirst = value;
                OnPropertyChanged(nameof(ValueSecondToFirst));
            }
            // OnPropertyChanged(nameof(ValueSecondToFirstStr));
        }
        
     

        public Brush ColorLine
        {
            get { return _colorLine; }
            set { _colorLine = value; OnPropertyChanged(nameof(ColorLine)); }
        }

        public WeightViewModel(NeuronViewModel firstNeuron, NeuronViewModel secondNeuron)
        {
            _neuronFirst = firstNeuron;
            _neuronSecond = secondNeuron;
            UpdateLine();
            _neuronFirst.PropertyChanged += Neuron_PropertyChanged;
            _neuronSecond.PropertyChanged += Neuron_PropertyChanged;

            UpdateConnectionTypes();
        }

        private void Neuron_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NeuronViewModel.Name))
            {
                UpdateConnectionTypes();
            }
        }

        private void UpdateConnectionTypes()
        {
            ConnectionTypes = new ObservableCollection<ConnectionType>
        {
            new ConnectionType { Name = "Двунаправленная", Type = 1 },
            new ConnectionType { Name = $"{_neuronFirst.Name} --> {_neuronSecond.Name}", Type = 2 },
            new ConnectionType { Name = $"{_neuronSecond.Name} --> {_neuronFirst.Name}", Type = 3 }
        };

            if (SelectedConnectionType == null || !ConnectionTypes.Contains(SelectedConnectionType))
            {
                SelectedConnectionType = ConnectionTypes.First();
            }
        }

        public void UpdateLine()
        {
            X1Point = _neuronFirst.PointObj.X + 25;
            Y1Point = _neuronFirst.PointObj.Y + 25;
            X2Point = _neuronSecond.PointObj.X + 25;
            Y2Point = _neuronSecond.PointObj.Y + 25;
        }

        public double X1Point
        {
            get { return _x1Point; }
            set
            {
                _x1Point = value;
                OnPropertyChanged(nameof(X1Point));
            }
        }
        public double X2Point
        {
            get { return _x2Point; }
            set
            {
                _x2Point = value;
                OnPropertyChanged(nameof(X2Point));
            }
        }
        public double Y1Point
        {
            get { return _y1Point; }
            set
            {
                _y1Point = value;
                OnPropertyChanged(nameof(Y1Point));
            }
        }
        public double Y2Point
        {
            get { return _y2Point; }
            set
            {
                _y2Point = value;
                OnPropertyChanged(nameof(Y2Point));
            }
        }

        public NeuronViewModel NeuronFirst
        {
            get { return _neuronFirst; }
            set
            {
                if (_neuronFirst != value)
                {
                    if (_neuronFirst != null)
                        _neuronFirst.PropertyChanged -= Neuron_PropertyChanged;

                    _neuronFirst = value;
                    OnPropertyChanged(nameof(NeuronFirst));

                    if (_neuronFirst != null)
                        _neuronFirst.PropertyChanged += Neuron_PropertyChanged;

                    UpdateConnectionTypes();
                }
            }
        }
        public NeuronViewModel NeuronSecond
        {
            get { return _neuronSecond; }
            set
            {
                if (_neuronSecond != value)
                {
                    if (_neuronSecond != null)
                        _neuronSecond.PropertyChanged -= Neuron_PropertyChanged;

                    _neuronSecond = value;
                    OnPropertyChanged(nameof(NeuronSecond));

                    if (_neuronSecond != null)
                        _neuronSecond.PropertyChanged += Neuron_PropertyChanged;

                    UpdateConnectionTypes();
                }
            }
        }

        public bool Equals(WeightViewModel other)
        {
            if (other == null)
                return false;

            return (this._neuronFirst == other._neuronFirst && this._neuronSecond == other._neuronSecond) ||
                   (this._neuronFirst == other._neuronSecond && this._neuronSecond == other._neuronFirst);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals(obj as WeightViewModel);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _neuronFirst.GetHashCode();
                hash = hash * 23 + _neuronSecond.GetHashCode();
                return hash;
            }
        }
    }

}
