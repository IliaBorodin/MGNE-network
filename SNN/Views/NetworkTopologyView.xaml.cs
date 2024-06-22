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
    /// Логика взаимодействия для NetworkTopologyView.xaml
    /// </summary>
    public partial class NetworkTopologyView : UserControl
    {
        
        private NetworkConfigurationViewModel mvvm;

        void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            NeuronViewModel neuronViewModel = (sender as FrameworkElement).DataContext as NeuronViewModel;
            mvvm.IsTab1Visible = true;
            mvvm.IsTab2Visible = false;
            mvvm.ChangeColor();
            //change SelectedPhone
            if(mvvm.SelectedNeuron != null)
            mvvm.SelectedNeuron.MembranePotential = -5;
           // mvvm.SelectedNeuron.
            mvvm.SelectedNeuron = neuronViewModel;
            
           
            //move mouse
            (sender as Ellipse).CaptureMouse();
           // offsetInEllipse = e.GetPosition((sender as Ellipse));
            mvvm.UpdateWeightsLine();

        }


        void line_MouseDowm(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            WeightViewModel weightViewModel = (sender as FrameworkElement).DataContext as WeightViewModel;
            mvvm.IsTab1Visible = false;
            mvvm.IsTab2Visible = true;
            mvvm.ChangeColor();
            mvvm.SelectedWeight = weightViewModel;
            
        }
        void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || !(sender as Ellipse).IsMouseCaptured)
                return;
            
            var pos = e.GetPosition(myCanvas);
            double left = Math.Max(0, Math.Min(myCanvas.ActualWidth - ((Ellipse)sender).ActualWidth, pos.X - ((Ellipse)sender).ActualWidth / 2));
            double top = Math.Max(0, Math.Min(myCanvas.ActualHeight - ((Ellipse)sender).ActualHeight, pos.Y - ((Ellipse)sender).ActualHeight / 2));

            Canvas.SetLeft((Ellipse)sender, left);
            Canvas.SetTop((Ellipse)sender, top);

            mvvm.SelectedNeuron.PointObj = new Point(left, top);
            mvvm.UpdateWeightsLine();
        }
        void ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender as Ellipse).IsMouseCaptured)
                return;
            (sender as Ellipse).ReleaseMouseCapture();
            
        }

        private void NetworkTopologyView_Loaded(object sender, RoutedEventArgs e)
        {
            // Получаем доступ к DataContext после загрузки элемента
            mvvm = DataContext as NetworkConfigurationViewModel;

            // Отключаем обработчик события Loaded после его выполнения
            Loaded -= NetworkTopologyView_Loaded;
        }


        public NetworkTopologyView()
        {
            InitializeComponent();
            Loaded += NetworkTopologyView_Loaded;

        }


      

       

       

    }
}
