using Microsoft.VisualBasic;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using SNN.Commands;
using SNN.Models;
using SNN.Services;
using SNN.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SNN.ViewModels
{
    public class NetworkDynamicsViewModel : ViewModelBase
    {
        public NetworkConfigurationViewModel ConfigurationViewModel { get; }
        private ObservableCollection<Neuron> _neurons;
        public ObservableCollection<Neuron> Neurons
        {
            get { return _neurons; }
            set
            {
                _neurons = value;
            }
        }
        // Команды 
        public ICommand StartNetworkCommand { get; }
        public ICommand SingleStepNetworkCommand { get; }
        public ICommand CreateGraphicCommand { get; }
        public ICommand ActivateForGraphCommand { get; }
        public ICommand DeactivateForGraphCommand { get; }
        public ICommand FewStepNetworkCommand { get; }
        public ICommand NavigateCommand { get; }

        public SupportFunction helper { get; }
        //*****************************************************
        //------------------------------------------------------
        private string _curTimer;
        public string CurTimer
        {
            get { return _curTimer; }
            set
            {
                _curTimer = value;
                OnPropertyChanged(nameof(CurTimer));

            }
        }

        private int _iteration = 0;
        public int Iteration
        {
            get { return _iteration; }
            set
            {
                _iteration = value;
                OnPropertyChanged(nameof(Iteration));
            }
        }
        private decimal _time = 0;
        public decimal Time {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
        //   public TimeSpan Delta { get; set; }
        private decimal _delta;
        public decimal Delta {
            get { return _delta; }
            set
            {
                _delta = value;
                OnPropertyChanged(nameof(Delta));
            }
        }
        private int _typeEvent;       
        public int TypeEvent {
            get { return _typeEvent;}
            set { _typeEvent = value; }
        }
        public int NumberOfCurrNeuron { get; set; }

        private bool _flag = false;
        public bool Flag
        {
            get { return _flag; }
            set {
                _flag = value;
                OnPropertyChanged(nameof(Flag));
                OnPropertyChanged(nameof(InverseFlag));
            }
        }

        public bool InverseFlag
        {
            get { return !_flag; }
            
        }
        private bool _availability = true;
        public bool Availability
        {
            get { return _availability; }
            set
            {
                _availability = value;
                OnPropertyChanged(nameof(Availability));
            }
        }

       

        private string _textBtn = "Старт";
        public string TextBtn
        {
            get { return _textBtn; }
            set {
                _textBtn = value;
                OnPropertyChanged(nameof(TextBtn));
            }
        }
        //------------------------------------------------------
        private Neuron _graphNeuronFirst;
        public Neuron GraphNeuronFirst
        {
            get { return _graphNeuronFirst; }
            set
            {
                _graphNeuronFirst = value;
                OnPropertyChanged(nameof(GraphNeuronFirst));
            }
        }
        private Neuron _graphNeuronSecond;
        public Neuron GraphNeuronSecond
        {
            get { return _graphNeuronSecond; }
            set
            {
                _graphNeuronSecond = value;
                OnPropertyChanged(nameof(GraphNeuronSecond));
            }
        }



        


        private Neuron _selectedNeuron;

        public Neuron SelectedNeuron
        {
            get { return _selectedNeuron; }
            set
            {
                _selectedNeuron = value;
                OnPropertyChanged(nameof(SelectedNeuron));
            }
        }



        public ObservableCollection<ObservableCollection<IndicatorSynapticImpact>> matrixM { get; set; }
        public SynapticConnection[,] matrixW { get; }
        public ICommand NavigateBackCommand { get; }

        public NetworkDynamicsViewModel(NavigationStore navigationStore, NetworkConfigurationViewModel configurationViewModel)
        {
            configurationViewModel.SelectedWeight = null;
            StartNetworkCommand = new StartNetwork(this);
            SingleStepNetworkCommand = new SingleStepNetwork(this);
            CreateGraphicCommand = new CreateGraphicCommand(this);
            ActivateForGraphCommand = new ActivateForGraphCommand(this);
            DeactivateForGraphCommand = new DeactivateForGraphCommand(this);
            FewStepNetworkCommand = new FewStepNetworkCommand(this);
            NavigateBackCommand = new NavigateBackCommand(navigationStore, configurationViewModel);
            helper = new SupportFunction(configurationViewModel);
            ConfigurationViewModel = configurationViewModel;
            _neurons = helper.GenerateNeurons();
            matrixW = helper.CreateSynapticMatrix();
            matrixM = helper.CreateIndicatorSynapticImpactMatrix(_neurons);
            GetStepBegin();
            //Обновление графика
            ListOfPoints = new ObservableCollection<PointData>();
            PlotModel = new PlotModel { Title = "Рассогласование элементов",
            Background = OxyColor.FromRgb(0xBB, 0xC1, 0xD1)
            };
            CurTimer = string.Format("{0}", Time);
        }

        

        public async void StartDynamics(bool singleStep = false)
        {
            Availability = false;
           
            do
            {
                decimal timePause = GetListCandidates();
                if (timePause == -1)
                {
                    MessageBox.Show("Сеть потухла", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    Availability = true;
                    return;
                }
                TimeSpan delay = TimeSpan.FromMilliseconds(Math.Round(Math.Abs((double)timePause), 3) * 1000);
                await Task.Delay(delay);
                RecalculationOtherNeuron();
                RecalculationRefractorinessNeuron();
                RecalculationReceptivityNeuron();
                await Task.Delay(100);
                foreach (Neuron neuron in _neurons)
                {
                    if (neuron.Status == 1)
                    {
                        neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5DF706"));
                    }
                    if (neuron.Status == -1)
                    {
                        neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#06F7E8"));
                    }
                }
                GetAccessRecalculation();
                TimeDifference();

                foreach (Neuron neuron in _neurons)
                {
                    if (neuron.Status == 1)
                        neuron.TimeToSpikeGeneration(WeightedAmount(neuron));
                    if (neuron.Status == -1)
                        neuron.TimeToRefractorinessOutput();
                }
                Iteration++;
                CurTimer = string.Format("{0}", Time);
            } while (!singleStep && _flag);
            Availability = true;
        }

        public void DoFewIterations()
        {

            Availability = false;
            for(int i = 0; i < 1000; ++i)
            {
                decimal timePause = GetListCandidates();               
                RecalculationOtherNeuron();
                RecalculationRefractorinessNeuron();
                RecalculationReceptivityNeuron();
                foreach (Neuron neuron in _neurons)
                {
                    if (neuron.Status == 1)
                    {
                        neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5DF706"));
                    }
                    if (neuron.Status == -1)
                    {
                        neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#06F7E8"));
                    }
                }
                GetAccessRecalculation();
                TimeDifference();

                foreach (Neuron neuron in _neurons)
                {
                    if (neuron.Status == 1)
                        neuron.TimeToSpikeGeneration(WeightedAmount(neuron));
                    if (neuron.Status == -1)
                        neuron.TimeToRefractorinessOutput();
                }
                Iteration++;
                CurTimer = string.Format("{0}", Time);
            }
            Availability = true;
        }

        public double WeightedAmount(Neuron receiver)
        {
            double sum = 0.0;
            int neuronCount = _neurons.Count;

            // Find the index of the receiver neuron
            int receiverIndex = _neurons.IndexOf(receiver);

            // Iterate over all neurons (i) and calculate the weighted sum
            for (int i = 0; i < neuronCount; i++)
            {
               
                sum+= matrixW[i, receiverIndex].Value * matrixM[i][receiverIndex].Value;
            }

            return sum;
        }


        
        public void GetStepBegin()
        {
            foreach (Neuron neuron in _neurons)
            {
                if (neuron.Status == 1)
                {
                    neuron.TimeToSpikeGeneration(WeightedAmount(neuron));
                    neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5DF706")); // зеленый - восп
                }
                if (neuron.Status == -1)
                {
                    neuron.TimeToRefractorinessOutput();
                    neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#06F7E8")); //бирюзовый - рефр
                }
            }
        }

        


       
        private decimal MakeNetworkStep()
        {
            // Обновляем веса, если статус нейрона = 1(восприимчивость)
            foreach (Neuron neuron in _neurons)
            {
                if (neuron.Status == 1)
                    neuron.TimeToSpikeGeneration(WeightedAmount(neuron));
                if (neuron.Status == -1)
                    neuron.TimeToRefractorinessOutput();
            }
            GetListCandidates(); //Составить список кандидатов до ближайшего события
            RecalculationOtherNeuron(); //Пересчет остальных элементов
            RecalculationRefractorinessNeuron(); // Пересчет элементов со статусом "рефрактерность"
            RecalculationReceptivityNeuron(); // Пересчет элементов со статусом "восприимчивость"
            GetAccessRecalculation();          
            return _delta;
        }

        public decimal GetListCandidates()
        {
           
            var validNeurons = _neurons.Where(neuron => neuron.TimeToEvent != -1).ToList();


            if (!validNeurons.Any())
                return -1;

            decimal min = validNeurons.Min(neuron => neuron.TimeToEvent);

            foreach (Neuron neuron in _neurons)
            {
                neuron.Active = neuron.TimeToEvent == min;
            }
           
            Delta = min;           
            return min;
        }


        public void RecalculationOtherNeuron()
        {
            foreach (Neuron neuron in _neurons)
            {
                if (neuron.Active != true && neuron.Status == 1 && neuron.RecalculatingAccess ==true)
                {
                    neuron.updateMembranePotential(_delta, WeightedAmount(neuron));
                    neuron.RecalculatingAccess = false;
                }
                if(neuron.Active !=true && neuron.Status == -1 && neuron.RecalculatingAccess == true)
                {
                    neuron.updateMembranePotentialR(_delta);
                    neuron.RecalculatingAccess = false;
                }
            }
        }

        public  void RecalculationReceptivityNeuron()
        {
            foreach(Neuron neuron in _neurons)
            {
                if(neuron.Active == true && neuron.Status == 1 && neuron.RecalculatingAccess == true)
                {
                    neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F83309"));//красный
                   // neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#06F7E8")); //бюр
                    neuron.Status = -1;
                    neuron.MembranePotential = -1;
                   // neuron.TimeToRefractorinessOutput();
                    GetImpulse(neuron);
                    neuron.RecalculatingAccess = false;
                }

            }
           
       
        }

        public  void RecalculationRefractorinessNeuron()
        {
            foreach(Neuron neuron in _neurons)
            {
                if(neuron.Active == true && neuron.Status == -1 && neuron.RecalculatingAccess == true)
                {
                    neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0951F8")); //син
                    neuron.Status = 1;
                    neuron.MembranePotential = 0;
                    //neuron.ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5DF706")); //зел
                   // neuron.TimeToSpikeGeneration(WeightedAmount(neuron));
                    MakeInsensitivity(neuron);
                    neuron.RecalculatingAccess = false;
                }
            }
            
           

        }

        public void GetAccessRecalculation()
        {
            foreach(Neuron neuron in _neurons)
            {
                neuron.RecalculatingAccess = true;
            }
        }

       


        public void GetImpulse(Neuron senderNeuron)
        {
            // Находим индекс нейрона отправителя
            int senderIndex = _neurons.IndexOf(senderNeuron);
            if (senderIndex == -1)
            {
                throw new ArgumentException("Sender neuron not found in the neuron list.");
            }

            // Обновляем матрицу
            for (int j = 0; j < _neurons.Count; j++)
            {

                if (matrixM[senderIndex][j].Receiver.Status == 1 && matrixM[senderIndex][j].Receiver.Active!=true)
                {
                    
                    matrixM[senderIndex][j].Value = 1;
                }
            }
        }

        public void MakeInsensitivity(Neuron neuron)
        {
            for(int i=0;i<_neurons.Count;i++)
            {
                for(int j = 0; j<_neurons.Count;j++)
                {
                    if (matrixM[i][j].Receiver == neuron)
                    {
                        matrixM[i][j].Value = 0;
                    }
                }
            }
        }


        public void TimeDifference()
        {
            Time = _time + _delta;
            _delta = 0;
            foreach (Neuron neuron in _neurons)
            {
                if(neuron.Active == true && neuron.Status == -1)
                {
                    neuron.ImpulseGenerationList.Add(Time);
                }
                

            }
        }

       

       


        public  void getSelectedNeuron(NeuronViewModel neuronVM)
        {
            foreach(Neuron neuron in _neurons)
            {
                if(neuron.Visualization == neuronVM)
                    SelectedNeuron = neuron;
            }
        }

        public void PerformOneIteration()
        {
            CurTimer = string.Format("{0}", Time);
            decimal timePause = MakeNetworkStep();
            TimeDifference();
        }


        private PlotModel _plotModel;
        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set
            {
                _plotModel = value;
                OnPropertyChanged(nameof(PlotModel));
            }
        }

        private ObservableCollection<PointData> _listOfPoints;
        public ObservableCollection<PointData> ListOfPoints
        {
            get { return _listOfPoints; }
            set
            {
                _listOfPoints = value;
                OnPropertyChanged(nameof(ListOfPoints));
                UpdatePlotModel();
            }
        }

        private void UpdatePlotModel()
        {
            if (_plotModel == null)
            {
                _plotModel = new PlotModel { Title = "Рассогласование элементов",
                Background = OxyColor.FromRgb(0xBB, 0xC1, 0xD1)
                };

                // Настройка осей
                _plotModel.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = "Time",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot
                });
                _plotModel.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Difference",
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot
                });
            }

            _plotModel.Series.Clear();
            var series = new LineSeries { Title = $"Первый элемент:{GraphNeuronFirst?.Visualization.Name}, Второй элемент:{GraphNeuronSecond?.Visualization.Name}" };

            foreach (var point in ListOfPoints)
            {
                series.Points.Add(new DataPoint(point.XValue, point.YValue));
            }

            _plotModel.Series.Add(series);
            _plotModel.ResetAllAxes();

            // Добавление аннотаций для подписи нейронов


            _plotModel.InvalidatePlot(true);
        }


        private bool _isSelectedNeuronVisible = false;

        public bool IsSelectedNeuronVisible
        {
            get { return _isSelectedNeuronVisible; }
            set
            {
                _isSelectedNeuronVisible = value;
                OnPropertyChanged(nameof(IsSelectedNeuronVisible));
            }
        }

        private int _counterForGraph;
        public int CounterForGraph
        {
            get { return _counterForGraph; }
            set
            {
                _counterForGraph = value;
                OnPropertyChanged(nameof(CounterForGraph));
            }
        }



    }
}


