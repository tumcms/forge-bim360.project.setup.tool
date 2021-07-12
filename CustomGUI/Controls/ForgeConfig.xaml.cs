﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomGUI.Controls
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class ForgeConfig : UserControl , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            this.ClientId = ClientId_Box.Text;
            this.ClientSecret = ClientSecret_Box.Text;
            this.BimId = BimId_Box.Text;
            this.AdminMail = AdminMail_Box.Text;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string BimId { get; set; }
        private string AdminMail { get; set; }
        private string ConfigFilePath { get; set; }

        public ForgeConfig()
        {
            InitializeComponent();
            this.DataContext = this;
            ConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Bim360Interface\Config\config.txt";
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

        private void AdminMail_Box_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (AdminMail_Box.Text.Equals("AdminMail"))
            {
                AdminMail_Box.Text = "";
            }
        }

        private void SaveConfig_click(object sender, RoutedEventArgs e)
        {
            // run the save method
            this.SaveConfigToFile(ConfigFilePath);
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


        #region Config handling
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
                        ClientId_Box.Text = ClientId;
                        ClientSecret = reader.ReadLine();
                        ClientSecret_Box.Text = ClientSecret;
                        BimId = reader.ReadLine();
                        BimId_Box.Text = BimId;
                        AdminMail = reader.ReadLine();
                        AdminMail_Box.Text = AdminMail;
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

            System.Threading.Thread.Sleep(1000);
            // query current values from textboxes
            this.ClientId = ClientId_Box.Text;
            this.ClientSecret = ClientSecret_Box.Text;
            this.BimId = BimId_Box.Text;
            this.AdminMail = AdminMail_Box.Text;

            if (!File.Exists(filePath))
            {
                //Create Directory and File for the Config
                Directory.CreateDirectory(filePath.Remove(filePath.LastIndexOf("\\")));
                var tmp = File.Create(filePath);
                tmp.Close();
            }

            //Write config into txt
            using (FileStream fs = File.OpenWrite(filePath))
            {
                using (var sr = new StreamWriter(fs))
                {
                    //Delete the content of the file
                    sr.Write(string.Empty);

                    //Write the new data into it
                    sr.WriteLine(this.ClientId);
                    sr.WriteLine(this.ClientSecret);
                    sr.WriteLine(this.BimId);
                    sr.WriteLine(this.AdminMail);
                }

                
            }
        }

        private void ConfigControl_Initialized(object sender, EventArgs e)
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

    }
}
