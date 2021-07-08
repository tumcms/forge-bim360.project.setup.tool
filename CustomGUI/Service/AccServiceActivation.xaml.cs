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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomGUI.Controls;

namespace CustomGUI.Service
{
    /// <summary>
    /// Interaktionslogik für AccServiceActivation.xaml
    /// </summary>
    public partial class Service : UserControl
    {
        public Service()
        {
            InitializeComponent();
        }

        private void CheckBoxservices_OnChecked(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Please enter the Companyname:", "Example Company");
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                Company.Text = dialog.Answer;
            }
            else
            {
                CheckBoxservices.IsChecked = false;
            }
            dialog.Close();
        }
    }
}
