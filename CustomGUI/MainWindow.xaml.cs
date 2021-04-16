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
        List<UserData> usermanag = new List<UserData>();


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
        private void saveconf_Click(object sender, RoutedEventArgs e)
        {
            /* TODO: save config data in the Texboxes into the Config File 
               or create new if not exists            
             */
        }

        //Remove input if use click on it
        private void clientid_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (clientid.Text.Equals("Forge_Client_ID"))
            {
                clientid.Text = "";
            }
        }
        private void bimid_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (bimid.Text.Equals("Forge_Bim_Account_ID"))
            {
                bimid.Text = "";
            }
        }
        private void clientsecret_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (clientsecret.Text.Equals("Forge_Client_Secret"))
            {
                clientsecret.Text = "";
            }
        }



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


            statusbar.Text="Import succesfull!";

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
            var window = new Window();
            window.Owner = this;
            //Add the right Config 
            var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            stackPanel.Children.Add(new TextBlock { HorizontalAlignment = HorizontalAlignment.Left,
                           Margin = new Thickness(40,40,0,0),
                           Text = "Forge_Client_ID",
                           VerticalAlignment = VerticalAlignment.Top,
                           FontSize = 16
            });
            stackPanel.Children.Add(new TextBox {  });
            window.Content = stackPanel;
            window.ShowDialog();
            
        }
    }
}
