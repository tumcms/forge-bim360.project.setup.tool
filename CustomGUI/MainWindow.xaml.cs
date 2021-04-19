using CsvHelper;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        //Classes for Datagrids
        //one instand of a class contains data of a row in the CSV file
        //Global Data
        List<UserData> usermanag = new List<UserData>();
        StackPanel save_conf;
        
        string path_file = ".\\Config\\config.txt";

        public MainWindow()
        {
            //TODO: configfile handle -> on start read out configfile all id and set them
            InitializeComponent();
            


            usermanag.Add(new UserData());
            List<string> temp = new List<string>();
            usermanag[0]._sub_folder = temp;
            usermanag[0]._sub_folder.Add("test1");
            usermanag[0]._sub_folder.Add("test3");

            data.ItemsSource = usermanag;

        }



        //Event Handling

        
        //Tab Config

        //needs to be removed until (*)
        private void saveconf_Click(object sender, RoutedEventArgs e)
        {
            /* TODO: save config data in the Texboxes into the Config File 
               or create new if not exists            
             */
        }

        

        // Here (*)

        //UserManagement


        //CSVToolkit

        private void buttonimport_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(csvpath.Text)){
                statusbar.Text = "Import Failed! File not found!";
            }

            using (var readdata = new StreamReader(csvpath.Text))
            {
                var csv = new CsvReader(readdata, CultureInfo.CurrentCulture);
                //Maps the Header of the CSV Data to the class attributs
                csv.Context.RegisterClassMap<UserDataMap>();

                //Read all rows and safe in the usermanagment
                while (csv.Read())
                {
                    var tmp = csv.GetRecord<UserData>();
                    usermanag.Add(tmp);
                }

                
                
            }


            statusbar.Text="Import successful!";

        }

        //Remove input if use click on it
        private void csvpath_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

            if (csvpath.Text.Equals("C:\\Users\\Example1\\example.csv"))
            {
                csvpath.Text = "";
            }
        }

        private void buttonexport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            // ToDo: Use Controls -> Config.XAML instead of re-implementing the same UI again

            //var window = new Window();
            //window.Owner = this;
            ////Add config input boxes
            //save_conf = new StackPanel { Orientation = Orientation.Vertical };
            //save_conf.Children.Add(new TextBlock 
            //{ 
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Margin = new Thickness(40,40,0,0),
            //    Text = "Forge_Client_ID",
            //    VerticalAlignment = VerticalAlignment.Top,
            //    FontSize = 16,
            //    FontWeight=FontWeights.Bold
            //});
            //save_conf.Children.Add(new TextBox 
            //{ 
            //    Name = "Forge_Client_ID",
            //    Text = conf_clientid,
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Margin = new Thickness(200,40,0,0),
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Width = 300,
            //});

            //save_conf.Children.Add(new TextBlock
            //{
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Margin = new Thickness(40, 80, 0, 0),
            //    Text = "Forge_Client_Secret",
            //    VerticalAlignment = VerticalAlignment.Top,
            //    FontSize = 16,
            //    FontWeight = FontWeights.Bold
            //});
            //save_conf.Children.Add(new TextBox
            //{
            //    Name = "clientsecret",
            //    Text = conf_clientsecret,
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Margin = new Thickness(200, 80, 0, 0),
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Width = 300,
            //});

            //save_conf.Children.Add(new TextBlock
            //{
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Margin = new Thickness(40, 120, 0, 0),
            //    Text = "Forge_Bim_Account_ID",
            //    VerticalAlignment = VerticalAlignment.Top,
            //    FontSize = 16,
            //    FontWeight = FontWeights.Bold
            //});
            //save_conf.Children.Add(new TextBox
            //{
            //    Name = "bimid",
            //    Text = conf_bimid,
            //    HorizontalAlignment = HorizontalAlignment.Left,
            //    Margin = new Thickness(200, 120, 0, 0),
            //    VerticalAlignment = VerticalAlignment.Top,
            //    Width = 300,
            //    //add on change event handle
            //}) ;

            //save_conf.Children.Add(new Button
            //{
            //    Content="Save Config",
            //    HorizontalAlignment= HorizontalAlignment.Left,
            //    Margin = new Thickness(604,159,0,0),
            //    VerticalAlignment= VerticalAlignment.Top,
            //});

            //window.Closed += Window_Closed;
            //window.Content = save_conf;
            //window.ShowDialog();


        }

        private void Window_Closed(object sender, EventArgs e)
        {

            string path_folder = ".\\Config";

            //quick and dirty solution -> mainly for testing
            var temp = save_conf.Children.OfType<TextBox>().ToList();

            // ToDo: delegate store operation to dedicated control in Controls->Config.xaml

            //conf_clientid = temp[0].Text;
            //conf_clientsecret = temp[1].Text;
            //conf_bimid = temp[2].Text;



            ////Create Config Folder
            //if (!Directory.Exists(path_folder))
            //{
            //    Directory.CreateDirectory(path_folder);
            //}
            ////Check if Config already exists
            //if (File.Exists(path_file))
            //{
            //    File.Delete(path_file);
            //}

            ////Write config into txt
            //using(FileStream fs = File.Create(path_file))
            //{
            //    using(var sr= new StreamWriter(fs))
            //    {
            //        sr.WriteLine(conf_clientid);
            //        sr.WriteLine(conf_clientsecret);
            //        sr.WriteLine(conf_bimid);
            //    }
            //}

        }
    }
}
