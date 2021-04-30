﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
using AdskConstructionCloudBreakdown;
using CsvHelper;

namespace CustomGUI.Controls
{
    /// <summary>
    /// Interaction logic for AccProjectConfig.xaml
    /// </summary>
    public partial class AccProjectConfig : UserControl
    {
        private List<Bim360Project> projects { get; set; }
        private string csvpath { get; set; }
        public AccProjectConfig()
        {
            InitializeComponent();

        }

        private void AccProjectConfig_OnInitialized(object? sender, EventArgs e)
        {
            try
            {
                //TOdo: add last path to csvpath to load last config
                // projects = SerializationParser.LoadBim360ProjectsFromCsv("csvpath");
            }
            catch{}

        }

        public void LoadBim360Projekts(string filepath)
        {
            using (var readdata = new StreamReader(filepath))
            { 
                using (var csv = new CsvReader(readdata, CultureInfo.CurrentCulture))
                {

                    //Maps the Header of the CSV Data to the class attributs
                    csv.Context.RegisterClassMap<UserDataMap>();

                    //call the import for new class def
                    var output = SerializationParser.LoadBim360ProjectsFromCsv(csv);

                    //ToDo: sort data into Frontend

                    ProjectsView.ItemsSource = output;

                }
            }


        }
    }
}
