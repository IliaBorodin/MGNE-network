using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNN.Validation
{
  public class ValidationParametersMP: DependencyObject
    {
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(ValidationParametersMP), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty SelectedConnectionTypeProperty =
            DependencyProperty.Register("SelectedConnectionType", typeof(int), typeof(ValidationParametersMP), new PropertyMetadata(default(int)));

        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public int SelectedConnectionType
        {
            get { return (int)GetValue(SelectedConnectionTypeProperty); }
            set { SetValue(SelectedConnectionTypeProperty, value); }
        }
    }
}
