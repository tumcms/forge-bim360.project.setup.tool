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


        private string path_file =@".\Config\config.txt";
        private string path_last =@".\Config\last.txt";


        public MainWindow()
        {
            InitializeComponent();
            
            // sample data from recently introduced class structure mirroring key aspects of the BIM360 environment
            projects = CreateSampleStructure();


            //AccProjectConfig.ProjectsView.ItemsSource = projects;

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

        //Remove input if use click on it
        private void csvpath_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

            if (csvpath.Text.Equals("C:\\Users\\Example1\\example.csv"))
            {
                csvpath.Text = "";
            }
        }


        private void buttonimport_Click(object sender, RoutedEventArgs e)
        {
            //move to conifg
            if (!File.Exists(csvpath.Text)){
                statusbar.Text = "Import failed! File not found!";
                return;
            }

            //Load CSV into Mainwindow
            if (AccProjectConfig.LoadBim360Projects(csvpath.Text))
            {
                statusbar.Text = "Import successful!";
                //Write config into txt
                using (FileStream fs = File.OpenWrite(path_last))
                {
                    using (var sr = new StreamWriter(fs))
                    {
                        //Delete the content of the file
                        sr.Write(string.Empty);
                        sr.WriteLine(csvpath.Text);
                    }
                }
            }
            else
            {
                statusbar.Text = "Import failed! Unknown CSV Config!";
            }

        }



        private void buttonexport_Click(object sender, RoutedEventArgs e)
        {
            //move to conifg
            if (File.Exists(csvpathexp.Text))
            {
                statusbar.Text = "Export Failed! File already exists!";
            }

            if (!Directory.Exists((csvpathexp.Text)))
            {
                var tmp = csvpathexp.Text;

                Directory.CreateDirectory((csvpathexp.Text));
            }

            //Hardcoded Name in here maybe user should be able to change
            string filename = "\\BIM360_Custom_Template.csv";

            //export the Projects
            AccProjectConfig.ExportBim360Projects(csvpathexp.Text+ filename, (List<Bim360Project>) AccProjectConfig.ProjectsView.ItemsSource);

            statusbar.Text = "Export successful";

        }

        private void MainWindow_OnInitialized(object? sender, EventArgs e)
        {
            //create history
            if (!File.Exists(path_last))
            {
                //Create Directory and File for the Config
                Directory.CreateDirectory(path_last.Remove(path_last.LastIndexOf("\\")));
                var tmp = File.Create(path_last);
                tmp.Close();
            }
            else
            {
                if (File.Exists(path_last))
                {
                    using (FileStream fs = File.OpenRead(path_last))
                    {
                        using (StreamReader reader = new StreamReader(fs))
                        {
                            csvpath.Text = reader.ReadLine();
                        }
                    }
                }
            }



        }
        private void Csvpathexp_OnInitialized(object? sender, EventArgs e)
        {
            csvpathexp.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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
