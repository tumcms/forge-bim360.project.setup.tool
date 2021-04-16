using System;
using System.Collections.Generic;
using System.IO;
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
        UserData test = new UserData();
        

        public MainWindow()
        {
            //TODO: configfile handle -> on start read out configfile all id and set them
            InitializeComponent();
            List<UserData> usermanag = new List<UserData>();
            usermanag.Add(new UserData { _projectname = "Test1",_projecttype="mehrTest"  });
            data.Items.Add(usermanag[0]);

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
    }
}
