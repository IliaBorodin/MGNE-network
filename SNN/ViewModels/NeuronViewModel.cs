using SNN.Models;
using SNN.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SNN.ViewModels
{


    
    public class NeuronViewModel: ViewModelBase
    {





        private static int instanceCount = 1;

        private string _name;
        public string Name {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private Brush colorRectangle = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9B42"));
        private Point pointObj;

        public Brush ColorRectangle
        {
            get { return colorRectangle; }
            set { colorRectangle = value; OnPropertyChanged(nameof(ColorRectangle)); }
        }
        public Point PointObj
        {
            get { return pointObj; }
            set { pointObj = value; OnPropertyChanged(nameof(PointObj)); }
        }

        public NeuronViewModel(string name, Point point, double rValue, double pValue)
        {
            _name = name;
            pointObj.X = point.X;
            pointObj.Y = point.Y;
            _id = instanceCount;
            instanceCount++;
            // MembranePotential = _random.Next(1, 56);
            InitialStatus = _random.Next(0, 2);
            UpdateInitialStatusTypes();
            ParameterPValue = pValue;
            ParameterRValue = rValue;
            _readyNotForExternal = false;
            _readyForExternal = true;
            
           
        }

        public NeuronViewModel(string name, Point point, double rValue, double pValue, double u, int status)
        {
            _name = name;
            pointObj.X = point.X;
            pointObj.Y = point.Y;
            _id = instanceCount;
            instanceCount++;
            InitialStatus = status;
            ParameterPValue = pValue;
            ParameterRValue = rValue;
            MembranePotential = u;
            _readyNotForExternal = false;
            _readyForExternal = true;
        }


        

        public static int GetInstanceCount()
        {
            return instanceCount;
        }

        private int _id;
        public int Id
        {
            get { return _id; }
        }
        private static Random _random = new Random();
        // Мембранный потенциал нейрона
        
        private double _membranePotential;
        
        public double MembranePotential
        {
            get { return _membranePotential; }
            set
            {
                if (ValidateMembranePotential(value))
                {
                    _membranePotential = value;
                    OnPropertyChanged(nameof(MembranePotential));
                }
            }
        }
        //********************************************************************
        private int _initialStatus;

        public int InitialStatus
        {
            get { return _initialStatus; }
            set
            {
                _initialStatus = value;
                OnPropertyChanged(nameof(InitialStatus));
            }
        }

        private ObservableCollection<InitialStatusType> _connectionTypes;
        private InitialStatusType _selectedConnectionType;
        private double tmp;
        public ObservableCollection<InitialStatusType> ConnectionTypes
        {
            get { return _connectionTypes; }
            set
            {
                _connectionTypes = value;
                OnPropertyChanged(nameof(ConnectionTypes));
            }
        }

        public InitialStatusType SelectedConnectionType
        {
            get { return _selectedConnectionType; }
            set
            {
                if (_selectedConnectionType != value)
                {
                    _selectedConnectionType = value;
                    OnPropertyChanged(nameof(SelectedConnectionType));
                    UpdateMembranePotentialRange();
                    SetInitialMembranePotential();

                }
            }
        }


        private void UpdateInitialStatusTypes()
        {
            ConnectionTypes = new ObservableCollection<InitialStatusType>
            {
                new InitialStatusType { Name = "Рефрактерность", Type = -1 },
                new InitialStatusType { Name = "Восприимчивость", Type = 1 }
            };

            if (SelectedConnectionType == null || !ConnectionTypes.Contains(SelectedConnectionType))
            {
                if(InitialStatus == 0)
                    SelectedConnectionType = ConnectionTypes.First();
                if(InitialStatus == 1)
                    SelectedConnectionType = ConnectionTypes.Last();
            }
        }


        private bool ValidateMembranePotential(double value)
        {
            if (SelectedConnectionType == null)
                return false;

            if (SelectedConnectionType.Type == -1)
            {
                return value >= -1 && value < 0;
            }
            else if (SelectedConnectionType.Type == 1)
            {
               // double minValue = Math.Min(17, 20);
                return value >= 0 && value < MinValue;
            }
            return false;
        }
        private double _parameterPValue;
        public double ParameterPValue
        {
            get { return _parameterPValue; }
            set
            {
                if (_parameterPValue != value)
                {
                    _parameterPValue = value;
                    OnPropertyChanged(nameof(ParameterPValue));
                    OnPropertyChanged(nameof(MinValue)); // Also notify that MinValue has changed
                    UpdateMembranePotentialRange();
                }
            }
        }

        private double _parameterRValue;
        public double ParameterRValue
        {
            get { return _parameterRValue; }
            set
            {
                if (_parameterRValue != value)
                {
                    _parameterRValue = value;
                    OnPropertyChanged(nameof(ParameterRValue));
                    OnPropertyChanged(nameof(MinValue)); // Also notify that MinValue has changed
                    UpdateMembranePotentialRange();
                }
            }
        }



        public double MinValue
        {
            get
            {
                return Math.Min(ParameterPValue, ParameterRValue);
            }
            set
            {
                OnPropertyChanged(nameof(MinValue));
            }
        }


        private string _membranePotentialRange;
        public string MembranePotentialRange
        {
            get { return _membranePotentialRange; }
            private set
            {
                if (_membranePotentialRange != value)
                {
                    _membranePotentialRange = value;
                    OnPropertyChanged(nameof(MembranePotentialRange));
                }
            }
        }

        private void UpdateMembranePotentialRange()
        {
            if (SelectedConnectionType == null)
            {
                MembranePotentialRange = string.Empty;
            }
            else if (SelectedConnectionType.Type == -1)
            {
                MembranePotentialRange = "[-1,0)";
            }
            else if (SelectedConnectionType.Type == 1)
            {
                MembranePotentialRange = $"[0,{MinValue})";
            }
        }



        public void SetInitialMembranePotential()
        {
            if (SelectedConnectionType.Type == 1)
            {
                double u = 1.0 - _random.NextDouble();
                MembranePotential = u * Math.Min(ParameterRValue, ParameterPValue);
                OnPropertyChanged(nameof(MembranePotential));
            }
            else
            {
                double u = 1.0 - _random.NextDouble();
                MembranePotential = - u;
                OnPropertyChanged(nameof(MembranePotential));
            }

        }

       


        private bool _externalInfluence  = false;
        public bool ExternalInfluence
        {
            get { return _externalInfluence; }
            set
            {
                _externalInfluence = value;
                OnPropertyChanged(nameof(ExternalInfluence));
            }
        }

        private bool _readyForExternal;
        public bool ReadyForExternal
        {
            get { return _readyForExternal; }
            set
            {
                _readyForExternal = value;
                OnPropertyChanged(nameof(ReadyForExternal));
            }
        }
        private bool _readyNotForExternal;
        public bool ReadyNotForExternal
        {
            get { return _readyNotForExternal; }
            set
            {
                _readyNotForExternal = value;
                OnPropertyChanged(nameof(ReadyNotForExternal));
            }
        }

    }


}
