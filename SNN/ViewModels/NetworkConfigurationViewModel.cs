using SNN.Commands;
using SNN.Models;
using SNN.Services;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SNN.ViewModels
{
    public class NetworkConfigurationViewModel : ViewModelBase
    {
        private static Random _random = new Random();
       // public ObservableCollection<NeuronViewModel> Neurons { get; set; }

        private ObservableCollection<NeuronViewModel> _neurons;

        public ObservableCollection<NeuronViewModel> Neurons
        {
            get { return _neurons; }
            set
            {
                if (_neurons != value)
                {
                    _neurons = value;
                    OnPropertyChanged(nameof(Neurons));
                    OnPropertyChanged(nameof(NeuronCountWithStatusOne));
                    OnPropertyChanged(nameof(NeuronCountWithStatusMinusOne));
                }
            }
        }

        public ObservableCollection<WeightViewModel> Weights { get; set; }

        private WeightMatrixViewModel _weightMatrix { get; set; }

        public WeightMatrixViewModel WeightMatrix
        {
            get { return _weightMatrix; }
            set
            {
                _weightMatrix = value;
                OnPropertyChanged(nameof(WeightMatrix));
            }
        }

        private NeuronViewModel _selectedNeuron;
        

        private NeuronViewModel _selectedNeuronSecond;

        private WeightViewModel _selectedWeight;
        private int _flag = 0;

        public int NeuronCountWithStatusOne
        {
            get { return Neurons.Count(n => n.SelectedConnectionType.Type == 1); }
        }

        public int NeuronCountWithStatusMinusOne
        {
            get { return Neurons.Count(n => n.SelectedConnectionType.Type == -1); }
        }


        //----------------------------------------------------------------------
        //Параметры сети
        private double tmp;
        private int _neuronNumbers;
        public int NeuronNumbers
        {
            get
            {
                return _neuronNumbers;
            }
            set
            {
                _neuronNumbers = value;
                OnPropertyChanged(nameof(NeuronNumbers));

            }
        }
        // Пороговое значение мембранного потенциала

        
        private double _p;
        public double P
        {
            get
            {
                return _p;
            }
            set
            {
                if(value > 0)
                {
                    _p = value;
                    OnPropertyChanged(nameof(P));
                    SetInitialMembranePotentials();
                    UpdateParameterPValue();
                    VisibleQ = (_p - _r) >= 0;
                }
                
            }
        }
        
        private bool _visibleQ;
        public bool VisibleQ
        {
            get { return _visibleQ; }
            set
            {
                _visibleQ = value;
                OnPropertyChanged(nameof(VisibleQ));
            }
        }
        private double _external;
        public double External
        {
            get { return _external; }
            set
            {
                if (value > 0)
                {
                    _external = value;
                    OnPropertyChanged(nameof(External));
                }
            }

        }
        // Равновесное значение мембранного потенциала
        

        private double _r;
        public double R
        {
            get
            {
                return _r;
            }
            set
            {
                if (value > 0)
                {
                    _r = value;
                    OnPropertyChanged(nameof(R));
                    SetInitialMembranePotentials();
                    UpdateParameterRValue();
                    VisibleQ = (_p - _r) >= 0;
                }
                    
            }
        }
        // Скоростной параметр
       

        private double _alpha;
        public double Alpha
        {
            get { return _alpha; }
            set
            {
                if(value > 0)
                {
                    _alpha = value;
                    OnPropertyChanged(nameof(Alpha));
                }
                
            }

        }


        // Период рефрактерности        
       

        private double _tr;
        public double Tr
        {
            get => _tr;
            set
            {
                if(value > 0)
                {
                    _tr = value;
                    OnPropertyChanged(nameof(Tr));
                }
                
            }
        }
        //----------------------------------------------------------------------
        public ICommand CreateNetworkCommand { get; }
        public ICommand CreateNeuronCommand { get; }

        public NetworkConfigurationViewModel(NavigationService<NetworkDynamicsViewModel> networkDinamicsNavigationService)
        {
            CreateNetworkCommand = new NavigateCommand<NetworkDynamicsViewModel>(networkDinamicsNavigationService);
            DeleteNeuronCommand = new DeleteNeuronCommand(this);
            CreateNeuronCommand = new CreateNeuronCommand(this);
            DeleteWeightCommand = new DeleteWeightCommand(this);
            CreateWeightCommand = new CreateWeightCommand(this);
            FullyConnectedNetworkCommand = new FullyConnectedNetworkCommand(this);
            AddQValueParameterCommand = new AddQValueParameterCommand(this);
            DeleteQValueParameterCommand = new DeleteQValueParameterCommand(this);
            GetConfOne = new GetConfigurationOneCommand(this);
            InitialHyperparameters();
             Neurons = new ObservableCollection<NeuronViewModel>
            {
                new NeuronViewModel("1",  new Point(50,50),_r, _p),
                new NeuronViewModel("2",  new Point(150,50), _r, _p),
                new NeuronViewModel("3",  new Point(50,150), _r, _p)

            };

            Weights = new ObservableCollection<WeightViewModel>
            {
                new WeightViewModel(Neurons[0], Neurons[1]),
                new WeightViewModel(Neurons[1], Neurons[2])
            };
            SetInitialWeights();
            SetInitialMembranePotentials();
            WeightMatrix = new WeightMatrixViewModel(Neurons, Weights);
            
        }
        public void updateValueSelected()
        {
            OnPropertyChanged(nameof(SelectedNeuron));
        }


        //----------------------------------



        //----------------------------------

        public void InitialHyperparameters()
        {
            _alpha = 0.5;
            _r = 20;
            _p = 17;
            _tr = 3;
      
        }
        public void UpdateWeightsLine()
        {
            foreach (WeightViewModel el in Weights)
            {
                el.UpdateLine();
            }
        }


        public NeuronViewModel takeNeuronInNeurons(int id)
        {
            foreach (NeuronViewModel neuron in Neurons)
            {
                if (neuron.Id == id)
                {
                    return neuron;
                }
            }
            return null;
        }

        public NeuronViewModel SelectedNeuron
        {
            get { return _selectedNeuron; }
            set
            {
                
                    if (!(_selectedNeuron is null))
                {
                    _selectedNeuron.ColorRectangle = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9B42"));
                }
                _selectedNeuron = value;
                if (_selectedNeuron != null)
                {
                    _selectedNeuron.ColorRectangle = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0FA3B1"));
                }
                OnPropertyChanged(nameof(SelectedNeuron));
            }
        }
        public NeuronViewModel SelectedNeuronSecond
        {
            get { return _selectedNeuronSecond; }
            set
            {
                _selectedNeuronSecond = value;
                OnPropertyChanged(nameof(SelectedNeuronSecond));
            }
        }

        public WeightViewModel SelectedWeight
        {
            get { return _selectedWeight; }
            set
            {
                if(!(_selectedWeight is null))
                {
                    _selectedWeight.ColorLine = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9c9695"));
                }
                _selectedWeight = value;
                if(_selectedWeight != null)
                {
                    _selectedWeight.ColorLine = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0FA3B1"));
                }
                OnPropertyChanged(nameof(SelectedWeight));
            }
    }

        //--------------------------------------------------

        public RelayCommand EllipseMouseDownCommand { get; private set; }
        public RelayCommand EllipseMouseMoveCommand { get; private set; }
        public RelayCommand EllipseMouseUpCommand { get; private set; }

        public ICommand DeleteNeuronCommand { get; }

        public ICommand DeleteWeightCommand { get; }

        public ICommand CreateWeightCommand { get; }

        public ICommand FullyConnectedNetworkCommand { get; }

        public ICommand AddQValueParameterCommand { get; }
        public ICommand DeleteQValueParameterCommand { get; }

        public ICommand GetConfOne { get; }




        //--------------------------------------------------

        private bool _isTab1Visible;
        private bool _isTab2Visible;

        public bool IsTab1Visible {
            get { return _isTab1Visible; }
            set
            {
                _isTab1Visible = value;
                OnPropertyChanged(nameof(IsTab1Visible));
            }
        }

        public bool IsTab2Visible
        {
            get { return _isTab2Visible; }
            set
            {
                _isTab2Visible = value;
                OnPropertyChanged(nameof(IsTab2Visible));
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        public void ChangeColor()
        {
            if (_isTab1Visible == false && _isTab2Visible != false)
            {
                if(SelectedNeuron != null)
                {
                    SelectedNeuron.ColorRectangle =  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9B42"));
                }
            }

            if(_isTab1Visible !=false && _isTab2Visible== false)
            {
                if(SelectedWeight != null)
                {
                    SelectedWeight.ColorLine = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9c9695"));
                }
            }
        }

        public int Flag
        {
            get { return _flag; }
            set 
            {
             _flag = value;
            OnPropertyChanged(nameof(Flag));
            }
        }

        public void UpdateParameterPValue()
        {
            foreach(NeuronViewModel neuron in Neurons)
            {
                neuron.ParameterPValue = _p;
            }
        }

        public void UpdateParameterRValue()
        {
            foreach (NeuronViewModel neuron in Neurons)
            {
                neuron.ParameterRValue = _r;
            }
        }

        public void SetInitialWeights()
        {
            if (Neurons.Count == 0)
                return;
            double stddev = Math.Sqrt(2.0 / Neurons.Count);
            foreach (WeightViewModel weight in Weights)
            {
                if(weight.SelectedConnectionType.Type == 1) //Двунаправленная связь
                {

                    weight.ValueFirstToSecond = SetInitialWeight();
                    weight.ValueSecondToFirst = SetInitialWeight();
                }
                if(weight.SelectedConnectionType.Type == 2) // 1-->2
                {
                   
                    weight.ValueFirstToSecond = SetInitialWeight();
                    weight.ValueSecondToFirst = 0;
                }
                if(weight.SelectedConnectionType.Type == 3) // 2-->1
                {
                    weight.ValueSecondToFirst = SetInitialWeight();
                    weight.ValueFirstToSecond = 0;
                }
            }
            
        }
        public double SetInitialWeight()
        {
           double stddev = Math.Sqrt(2.0 / Neurons.Count);
           double u1 = 1.0 - _random.NextDouble(); //uniform(0,1] random doubles
           double u2 = 1.0 - _random.NextDouble();
           double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
           return randStdNormal * stddev;
        }

        public void SetInitialMembranePotentials()
        {
            foreach(NeuronViewModel neuron in Neurons)
            {
                neuron.SetInitialMembranePotential();
            }            
            
        }

               

    }


}
