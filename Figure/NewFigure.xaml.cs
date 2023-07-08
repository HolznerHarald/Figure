using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaktionslogik für NewFigure.xaml
    /// </summary>
    public partial class NewFigure : Window
    {
        public NewFigure()
        {
            InitializeComponent();
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
