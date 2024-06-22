using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;
using System.Windows.Media;

namespace SNN.Models
{
    public class Neuron : ViewModelBase
    {
        //Ссылка на ViewModel(для отображения)
        private NeuronViewModel _visualization;
        public NeuronViewModel Visualization
        {
            get { return _visualization; }
            set {
                _visualization = value;
                OnPropertyChanged(nameof(Visualization));
            }
        }
        

        //Переменные, относящиеся к сети в целом
        private double _r;
        public double R { get { return _r; } }
        private double _p;
        public double P { get { return _p; } }
        private double _alpha;
        public double Alpha { get { return _alpha; } }
        private double _tr;
        public double Tr { get { return _tr; } }


        //----------------------------------------------------

        //----------------------------------------------------

        private bool _limbo;
        public bool Limbo
        {
            get { return _limbo; }
            set
            {
                _limbo = value;
                OnPropertyChanged(nameof(Limbo));
            }
        }

        //Динамические параметры
        private int _status;
        public int Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private bool _active;
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                OnPropertyChanged(nameof(Active));
            }
        }
        private static Random _random = new Random();
       // private static Random _randomReceptivity = new Random();
        private decimal _membranePotential;
        public decimal MembranePotential
        {
            get { return _membranePotential; }
            set
            {
                _membranePotential = value;
                OnPropertyChanged(nameof(MembranePotential));
            }
        }
        private string _timeToEventStr;
        public string TimeToEventStr
        {
            get { return _timeToEventStr; }
            set
            {
                _timeToEventStr = value;
                OnPropertyChanged(nameof(TimeToEventStr));
            }
        }
        private decimal _timeToEvent;
       public decimal TimeToEvent {
            get { return  _timeToEvent; }
            set
            {
                _timeToEvent = value;
                _timeToEventStr = value.ToString();
                OnPropertyChanged(nameof(TimeToEvent));
                OnPropertyChanged(nameof(TimeToEventStr));
                
            }
        }
       public int[] RelationshipIndicator {  get; set; }

        public Neuron(NeuronViewModel visualization, int status, double p, double r, double alpha, double tr, bool exteranalInfluence, double q)
        {
            _p = p; // Пороговое значение
            _r = r; // Равновесное значение
            _alpha = alpha; // Скоростной параметр
            _tr = tr; // Период рефрактерности
            _visualization = visualization;
            _status = status;
            _impulseGenerationList = new List<decimal>();
            InitialColorStatus();
            _readyForGraph = true;
            _readyNotForGraph = false;
            _limbo = false;
            _exteranalInfluence = exteranalInfluence;
            _qValue = q;


        }

        private double _qValue;
        public double QValue
        {
            get { return _qValue; }
        }
        private bool _exteranalInfluence;
        public bool ExteranalInfluence
        {
            get { return _exteranalInfluence; }
        }
        private void InitialColorStatus()
        {
            if(Status == 1)
            {

                ColorStatus =  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5DF706"));
            }
            if(Status == -1)
            {
                ColorStatus = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#06F7E8"));
            }
        }

        public double GetRandomDoubleInRange(double minValue, double maxValue)
        {
            return _random.NextDouble() * (maxValue - minValue) + minValue;
        }


        public void TimeToRefractorinessOutput()
        {
            TimeToEvent =  -_membranePotential * (decimal)_tr;
        }

        public void TimeToSpikeGeneration(double sum)
        { 
            if (_exteranalInfluence == true)
            {
                if(ImpulseGenerationList.Count > 0) {
                    _exteranalInfluence = false;
                    if (_p < _r + sum)
                        TimeToEvent = (decimal)(1 / _alpha * Math.Log((double)((_membranePotential - (decimal)_r - (decimal)sum) / ((decimal)_p - (decimal)_r - (decimal)sum))));
                    else
                    {
                        Limbo = true;
                        TimeToEvent = -1;
                        TimeToEventStr = "Неопределенность(нужно внешнее воздействие)";
                    }
                    return;
                }
                

                if (_p < _r + QValue)
                    TimeToEvent = (decimal)(1 / _alpha * Math.Log((double)((_membranePotential - (decimal)_r - (decimal)QValue) / ((decimal)_p - (decimal)_r - (decimal)QValue))));
                else
                {
                    Limbo = true;
                    TimeToEvent = -1;
                    TimeToEventStr = "Неопределенность(нужно внешнее воздействие)";
                }
                return;
            }
            else if (_exteranalInfluence == false)
            {
                if (_p < _r + sum)
                    TimeToEvent =(decimal)( 1 / _alpha * Math.Log((double)((_membranePotential - (decimal)_r - (decimal)sum) / ((decimal)_p - (decimal)_r - (decimal)sum))));
                else
                {
                    Limbo = true;
                    TimeToEvent = -1;
                    TimeToEventStr = "Неопределенность(нужно внешнее воздействие)";
                }
            }
        }



        public void updateMembranePotential(decimal delta, double sum)
        {
            MembranePotential = (decimal)((decimal)_r + (decimal)sum +
                (decimal)Math.Exp((double)(-(decimal)_alpha * delta)) *
                (_membranePotential - (decimal)_r - (decimal)sum));
           
        }

        public void updateMembranePotentialR(decimal delta)
        {
            MembranePotential = _membranePotential + delta / (decimal)_tr;
           
        }

        private Brush _colorStatus;

        public Brush ColorStatus
        {
            get { return _colorStatus; }
            set {
                _colorStatus = value;
                Visualization.ColorRectangle = value;
                OnPropertyChanged(nameof(ColorStatus));

            }
        }
        private bool _recalculatingAccess =true;
        public bool RecalculatingAccess
        {
            get { return _recalculatingAccess; }
            set
            {
                _recalculatingAccess = value;
            }
        }

        private List<decimal> _impulseGenerationList;
        public List<decimal> ImpulseGenerationList
        {
            get { return _impulseGenerationList; }
            set
            {
                _impulseGenerationList = value;
                OnPropertyChanged(nameof(ImpulseGenerationList));
            }
        }

        private bool _readyForGraph;
        public bool ReadyForGraph
        {
            get { return _readyForGraph; }
            set
            {
                _readyForGraph = value;
                OnPropertyChanged(nameof(ReadyForGraph));
            }
        }
        private bool _readyNotForGraph;
        public bool ReadyNotForGraph
        {
            get { return _readyNotForGraph; }
            set
            {
                _readyNotForGraph = value;
                OnPropertyChanged(nameof(ReadyNotForGraph));
            }
        }



    }
}
