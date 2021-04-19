using System;
using System.Collections.Generic;
using System.IO;
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

namespace CustomGUI.Controls
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : UserControl
    {
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string BimId { get; set; }

        public Config()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #region Callback functions from Frontend

        //Remove input if use click on it
        private void ClientId_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (ClientId_Box.Text.Equals("Forge_Client_ID"))
            {
                ClientId_Box.Text = "";
            }
        }
        private void BimId_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (BimId_Box.Text.Equals("Forge_Bim_Account_ID"))
            {
                BimId_Box.Text = "";
            }
        }
        private void ClientSecret_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (ClientSecret_Box.Text.Equals("Forge_Client_Secret"))
            {
                ClientSecret_Box.Text = "";
            }
        }
        private void SaveConfig_click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion


        /// <summary>
        /// Loads the config data from a specified config file
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadConfigFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        ClientId = reader.ReadLine();
                        ClientSecret = reader.ReadLine();
                        BimId = reader.ReadLine();
                    }
                }
            }
        }

        
    }
}
