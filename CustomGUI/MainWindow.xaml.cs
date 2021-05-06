using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdskConstructionCloudBreakdown;
using CustomGUI.Controls;

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
        private ObservableCollection<Bim360Project> projects;
        
        string path_file =@".\Config\config.txt";


        public MainWindow()
        {
            InitializeComponent();
            
            // sample data from recently introduced class structure mirroring key aspects of the BIM360 environment
            projects = CreateSampleStructure();


            AccProjectConfig.ProjectsView.ItemsSource = projects;


            usermanag.Add(new UserData());
            List<string> temp = new List<string>();
            usermanag[0]._sub_folder = temp;
            usermanag[0]._sub_folder.Add("test1");
            usermanag[0]._sub_folder.Add("test3");

            data.ItemsSource = usermanag;

        }



        //Event Handling

        //Config subwindow
        private void Config_Click(object sender, RoutedEventArgs e)
        {
            var window_config = new Window();
            window_config.Owner = this;
            window_config.Content = new ForgeConfig();
            window_config.Width = 710;
            window_config.Height = 150;
            window_config.ResizeMode = ResizeMode.NoResize;
            window_config.ShowDialog();


        }


        //UserManagement


        //CSVToolkit

        private void buttonimport_Click(object sender, RoutedEventArgs e)
        {
            //move to conifg
            if (!File.Exists(csvpath.Text)){
                statusbar.Text = "Import Failed! File not found!";
            }

            AccProjectConfig.LoadBim360Projects(csvpath.Text);

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
            //create a csv file with 10 + subfolder height columns 

            


        }



        /// <summary>
        /// Loading some test data for test purposes. To be deleted before deploying!
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Bim360Project> CreateSampleStructure()
        {
            var project1 = new Bim360Project("MyProject01")
            {
                ProjectType = ProjectTypeEnum.Office
            };


            project1.Plans=new Folder("Level1");
            project1.ProjectFiles=(new Folder("Level1"));
            project1.ProjectFiles.Subfolders.Add(new Folder("Level1"));

            var project2 = new Bim360Project("MyProject02")
            {
                ProjectType = ProjectTypeEnum.Office
            };


            project2.Plans=(new Folder("Level2"));
            project2.ProjectFiles=(new Folder("Level3"));
            project2.ProjectFiles.Subfolders.Add(new Folder("Level4"));

            return new ObservableCollection<Bim360Project>() {project1, project2};

        }


    }
}
