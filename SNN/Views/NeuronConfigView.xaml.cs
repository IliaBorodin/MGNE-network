using SNN.Commands;
using SNN.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SNN.Views
{
    /// <summary>
    /// Логика взаимодействия для NeuronConfigView.xaml
    /// </summary>
    public partial class NeuronConfigView : UserControl
    {
        
        public NeuronConfigView()
        {
            InitializeComponent();
          

        }

       

        public static readonly DependencyProperty NeuronRemoveCommandProperty =
            DependencyProperty.Register("NeuronDeleteCommand", typeof(ICommand), typeof(NeuronConfigView),
                new PropertyMetadata(null));

        public ICommand NeuronDeleteCommand
        {
            get { return (ICommand)GetValue(NeuronRemoveCommandProperty); }
            set { SetValue(NeuronRemoveCommandProperty, value); }
        }

        public static readonly DependencyProperty WeightCreateCommandProperty =
            DependencyProperty.Register("WeightCreateCommand", typeof(ICommand), typeof(NeuronConfigView),
                new PropertyMetadata(null));

        public ICommand WeightCreateCommand
        {
            get { return (ICommand)GetValue(WeightCreateCommandProperty); }
            set { SetValue(WeightCreateCommandProperty, value); }
        }

        public static readonly DependencyProperty ParameterPValueProperty =
            DependencyProperty.Register("ParameterPValue", typeof(double), typeof(NeuronConfigView),
                new PropertyMetadata(0.0));
        public double ParameterPValue
        {
            get { return (double)GetValue(ParameterPValueProperty); }
            set { SetValue(ParameterPValueProperty, value); }
        }

        public static readonly DependencyProperty ParameterRValueProperty =
            DependencyProperty.Register("ParameterRValue", typeof(double), typeof(NeuronConfigView),
                new PropertyMetadata(0.0));
        public double ParameterRValue
        {
            get { return (double)GetValue(ParameterRValueProperty); }
            set { SetValue(ParameterRValueProperty, value); }
        }

        public static readonly DependencyProperty ActivateForExternalCommandProperty =
            DependencyProperty.Register("ActivateForExternalCommand", typeof(ICommand), typeof(NeuronConfigView),
                new PropertyMetadata(null));

        public ICommand ActivateForExternalCommand
        {
            get { return (ICommand)GetValue(ActivateForExternalCommandProperty); }
            set { SetValue(ActivateForExternalCommandProperty, value); }
        }

        public static readonly DependencyProperty DeactivateForExternalCommandProperty =
            DependencyProperty.Register("DeactivateForExternalCommand", typeof(ICommand), typeof(NeuronConfigView),
                new PropertyMetadata(null));

        public ICommand DeactivateForExternalCommand
        {
            get { return (ICommand)GetValue(DeactivateForExternalCommandProperty); }
            set { SetValue(DeactivateForExternalCommandProperty, value); }
        }

        public static readonly DependencyProperty ParameterQValueProperty =
      DependencyProperty.Register(
          "ParameterQValue",
          typeof(bool),
          typeof(NeuronConfigView),
          new PropertyMetadata(false)
      );

        public bool ParameterQValue
        {
            get { return (bool)GetValue(ParameterQValueProperty); }
            set { SetValue(ParameterQValueProperty, value); }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            // Действия при входе курсора в область UserControl (если необходимо)
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            // Перемещение фокуса с TextBox при выходе курсора из области UserControl
            if (membranePotentialTextBox.IsFocused)
            {
                deleteButton.Focus();
                membranePotentialTextBox.Focus();
            }
        }




    }
}
