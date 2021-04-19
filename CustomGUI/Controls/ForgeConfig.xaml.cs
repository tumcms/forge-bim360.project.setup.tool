using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
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
using CustomGUI.Annotations;

namespace CustomGUI.Controls
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class ForgeConfig : UserControl
    {
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string BimId { get; set; }
        private string ConfigFilePath { get; set; }

        public ForgeConfig()
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
            

            // set path to CSV
            var path = @"C:\dev\BIM360config.txt";

            // run the save method
            this.SaveConfigToFile(path);
        }

        /// <summary>
        /// OnLoaded event should try to load a config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Config_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.LoadConfigFromFile(this.ConfigFilePath);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Couldn't find config file on specified path. ");
            }
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

        /// <summary>
        /// Saves the config data to a specified config file
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveConfigToFile(string filePath)
        {
            // query current values from textboxes
            // ToDo: Implement INotifyPropertyChanged Interface to dynamically bind the class property to the text box and simultaneously update both bidirectional
            this.ClientId = ClientId_Box.Text;
            this.ClientSecret = ClientSecret_Box.Text;
            this.BimId = BimId_Box.Text;

            //Check if Config already exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath); // ToDo: do we really need a delete operation or couldn't we use a simple overwrite in the Streamwriter if the file already exists?
            }

            //Write config into txt
            using (FileStream fs = File.OpenWrite(filePath))
            {
                using (var sr = new StreamWriter(fs))
                {
                    sr.WriteLine(this.ClientId);
                    sr.WriteLine(this.ClientSecret);
                    sr.WriteLine(this.BimId);
                }

                
            }
        }


       
    }
}
