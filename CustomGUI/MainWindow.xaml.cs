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
        StackPanel save_conf;
        private ObservableCollection<Bim360Project> projects;
        
        string path_file =@".\Config\config.txt";

        public MainWindow()
        {
            //TODO: configfile handle -> on start read out configfile all id and set them
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

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            var window_config = new Window();
            window_config.Owner = this;
            window_config.Content = new ForgeConfig();
            window_config.Width = 710;
            window_config.Height = 150;
            window_config.ShowDialog();

            // ToDo: Use Controls -> Config.XAML instead of re-implementing the same UI again


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



        private void Window_Closed(object sender, EventArgs e)
        {
            // ToDo: create global var for all necessary paths and set appropriate protection layers
            var filePath = @".\Config\config.txt";
            
            // call method from user control
            //ForgeConfig.SaveConfigToFile(filePath);
            
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


            project1.Plans.Add(new Folder("Level1"));
            project1.ProjectFiles.Add(new Folder("Level1"));
            project1.ProjectFiles[0].Subfolders.Add(new Folder("Level1"));

            var project2 = new Bim360Project("MyProject02")
            {
                ProjectType = ProjectTypeEnum.Office
            };


            project2.Plans.Add(new Folder("Level2"));
            project2.ProjectFiles.Add(new Folder("Level3"));
            project2.ProjectFiles[0].Subfolders.Add(new Folder("Level4"));

            return new ObservableCollection<Bim360Project>() {project1, project2};

        }

    }
}
