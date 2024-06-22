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
    /// Логика взаимодействия для NeuronInfoDynamicsView.xaml
    /// </summary>
    public partial class NeuronInfoDynamicsView : UserControl
    {
        public NeuronInfoDynamicsView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ActivateForGraphCommandProperty =
            DependencyProperty.Register("ActivateForGraphCommand", typeof(ICommand), typeof(NeuronInfoDynamicsView),
                new PropertyMetadata(null));

        public ICommand ActivateForGraphCommand
        {
            get { return (ICommand)GetValue(ActivateForGraphCommandProperty); }
            set { SetValue(ActivateForGraphCommandProperty, value); }
        }

        public static readonly DependencyProperty DeactivateForGraphCommandProperty =
            DependencyProperty.Register("DeactivateForGraphCommand", typeof(ICommand), typeof(NeuronInfoDynamicsView),
                new PropertyMetadata(null));

        public ICommand DeactivateForGraphCommand
        {
            get { return (ICommand)GetValue(DeactivateForGraphCommandProperty); }
            set { SetValue(DeactivateForGraphCommandProperty, value); }
        }



    }
}
