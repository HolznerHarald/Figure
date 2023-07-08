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
using System.Windows.Shapes;

namespace Figure
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        internal void Init(Options options)
        {
            if (options.WelcheTeile[0])
                CH1.IsChecked = true;
            else
                CH1.IsChecked = false;
            if (options.WelcheTeile[1])
                CH2.IsChecked = true;
            else
                CH2.IsChecked = false;
            if (options.WelcheTeile[2])
                CH3.IsChecked = true;
            else
                CH3.IsChecked = false;
            if (options.WelcheTeile[3])
                CH4.IsChecked = true;
            else
                CH4.IsChecked = false;
            if (options.WelcheTeile[4])
                CH5.IsChecked = true;
            else
                CH5.IsChecked = false;
            if (options.WelcheTeile[5])
                CH6.IsChecked = true;
            else
                CH6.IsChecked = false;
            //Schnitt
            if (options.scnhitt0)
                Schnitt0.IsChecked = true;
            else
                Schnitt0.IsChecked = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
