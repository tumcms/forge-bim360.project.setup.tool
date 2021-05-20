using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
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
using AdskConstructionCloudBreakdown;
using CsvHelper;
using CsvHelper.Configuration;

namespace CustomGUI.Controls
{
    /// <summary>
    /// Interaction logic for AccProjectConfig.xaml
    /// </summary>
    public partial class AccProjectConfig : UserControl 
    {




        protected List<Bim360Project> projects { get; set; }
        private string csvpath { get; set; }

        private Bim360Project activeProject { set; get; }

        private Folder activeFolder { set; get; }

        public AccProjectConfig()
        {
            InitializeComponent();


            //combobox Value assignment
            FolderUserPermissionComboBox.ItemsSource = Enum.GetValues(typeof(AdskConstructionCloudBreakdown.AccessPermissionEnum));
            FolderRolePermissionComboBox.ItemsSource = Enum.GetValues(typeof(AdskConstructionCloudBreakdown.AccessPermissionEnum));
            
            //transform Enum to real Name as string
            var tmp = Enum.GetValues((typeof(AdskConstructionCloudBreakdown.ProjectTypeEnum)));
            List<string> tobeadded = new List<string>();
            foreach (var iter in tmp)
            {
                tobeadded.Add(Selection.SelectProjectType((ProjectTypeEnum)iter));
            }
            ProjectTypeComboBox.ItemsSource = tobeadded;

            tmp = Enum.GetValues((typeof(AdskConstructionCloudBreakdown.TradeEnum)));
            tobeadded = new List<string>();
            foreach (var iter in tmp)
            {
                tobeadded.Add(Selection.SelectTrade((TradeEnum)iter));
            }
            tobeadded.Add(string.Empty);
            TradeComboBox.ItemsSource = tobeadded;


        }

        private void AccProjectConfig_OnInitialized(object? sender, EventArgs e)
        {
            //read last Path
            string path_last = @".\Config\last.txt";
            if (File.Exists(path_last))
            {
                using (FileStream fs = File.OpenRead(path_last))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        csvpath = reader.ReadLine();
                    }
                }
            }

            if (!string.IsNullOrEmpty(csvpath))
            {
                LoadBim360Projects(csvpath);
            }
        }

        public Boolean LoadBim360Projects(string filepath)
        {
            if (File.Exists(filepath))
            {
                for (int i = 0; i < 2; i++)
                {
                    using (var streamReader = new StreamReader(filepath))
                    {
                        //try different configs
                        CsvConfiguration csvconfig;
                        if (i == 0)
                        {
                            //CSV with current date config
                            csvconfig = new CsvConfiguration(CultureInfo.CurrentCulture)
                            {
                                HeaderValidated = null,
                                MissingFieldFound = null
                            };
                        }
                        else
                        {
                            //CSV with Invariant Config
                            csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                            {
                                HeaderValidated = null,
                                MissingFieldFound = null
                            };
                        }

                        //try each configuration
                        using (var csv = new CsvReader(streamReader, csvconfig))
                        {
                            //Maps the Header of the CSV Data to the class attributes
                            csv.Context.RegisterClassMap<UserDataMap>();

                            //call the import
                            var output = SerializationParser.LoadBim360ProjectsFromCsv(csv);

                            //Sort in Data if read was successful
                            if (output != null)
                            {
                                //ToDo: sort data into Frontend
                                projects = output;
                                ProjectsView.ItemsSource = projects;
                                return true;
                            }
                        }

                    }
                }
            }

            return false;
        }

        public void ExportBim360Projects(string filepath)
        {
            using (var streamWriter = new StreamWriter(filepath))
            {
                //maybe the CultureInfo needs to be changed
                using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    //Maps the Header of the CSV Data to the class attributes
                    var tmp = SerializationParser.ExportBim360ToCSV(projects);
                    csv.Context.RegisterClassMap<UserDataExport>();
                    csv.WriteRecords(tmp);
                }
            }

        }
        //change active project
        private void ProjectsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //should be right i guess
            activeProject=(Bim360Project)ProjectsView.SelectedCells[0].Item;
            TreeViewPlans.ItemsSource = activeProject.Plans.Subfolders;
            TreeViewProjects.ItemsSource = activeProject.ProjectFiles.Subfolders;
            ProjectTypeComboBox.SelectedItem = Selection.SelectProjectType(activeProject.ProjectType);

        }

        private void ProjectTypeComboBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProjectTypeComboBox.Text.ToString()))
            {
                activeProject.ProjectType = Selection.SelectProjectType(ProjectTypeComboBox.Text.ToString());
            }
        }

        private void Button_AddProject(object sender, RoutedEventArgs e)
        {
            projects.Add(new Bim360Project(Namenewproject.Text));
            activeProject=projects.Last();
            ProjectsView.Items.Refresh();
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //set active Folder 
            if (TreeViewFolder.SelectedItem.GetType()==typeof(Folder))
            {
                activeFolder = (Folder)TreeViewFolder.SelectedItem;
            }
            else if(activeProject!=null)
            {
                //this is needed because of the Static implimentation of the Rootfolders
                if (((TreeViewItem) TreeViewFolder.SelectedItem).Header.ToString() == "Plans")
                {
                    activeFolder = activeProject.Plans;
                }
                else if(((TreeViewItem)TreeViewFolder.SelectedItem).Header.ToString() == "Project Files")
                {
                    activeFolder = activeProject.ProjectFiles;
                }
            }

        }

        private void Button_AddUserPermission(object sender, RoutedEventArgs e)
        {
            //error handling
            if (activeProject == null)
            {
                MessageBox.Show("Please select a Project", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            if (activeFolder == null)
            {
                MessageBox.Show("Please select a Folder", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (FolderUserPermissionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a Userpermission", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var tobeadd = InputUserEmail.Text.Split(';');
            if (tobeadd.Any(iter => string.IsNullOrWhiteSpace(iter)))
            {
                MessageBox.Show("Please enter a valid Name.\nMaybe an ; to much?", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var inputindsrole = InputIndustryRole.Text.Split(';');
            var roletoadd = inputindsrole.Where(iterrole => 
                !string.IsNullOrWhiteSpace(iterrole)).ToList();


            //add user
            foreach (var iteruser in tobeadd)
            {
                var tmp = new UserPermission(iteruser, (AccessPermissionEnum) 
                    FolderUserPermissionComboBox.SelectedItem);
                activeFolder.UserPermissions.Add(tmp);
                tmp.AssignedUsers.IndustryRoles=roletoadd;

                //if no company was added
                if ((string.IsNullOrWhiteSpace(InputCompanyName.Text)) ||
                    (InputCompanyName.Text.Equals("add company name here"))) continue;

                tmp.AssignedUsers.AssignedCompany = new Company(InputCompanyName.Text);
                //assign Trade to Company
                if (TradeComboBox.SelectionBoxItem.ToString() != "")
                {
                    tmp.AssignedUsers.AssignedCompany.Trade = Selection.SelectTrade(
                        (string) TradeComboBox.SelectionBoxItem);
                }

            }

        }

        private void Button_AddRolePermission(object sender, RoutedEventArgs e)
        {
            //error handling
            if (activeProject == null)
            {
                MessageBox.Show("Please select a Project", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            if (activeFolder == null)
            {
                MessageBox.Show("Please select a Folder", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (FolderRolePermissionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a Rolepermission", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(InputRole.Text))
            {
                MessageBox.Show("Please enter a Role", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //add role to Folder
            var tobeadd=new RolePermission(InputRole.Text, (AccessPermissionEnum)
                FolderRolePermissionComboBox.SelectedItem);
            activeFolder.RolePermissions.Add(tobeadd);


        }

        private void MenuItem_FolderChild(object sender, RoutedEventArgs e)
        {
            var tmp = (MenuItem) e.Source;
            var folder= (Folder) tmp.DataContext;
            if (folder == null)
            {
                MessageBox.Show("Please select a Project", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var dialog = new InputDialog("Please enter the folder name:","Example Folder");
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog(); 
            folder.AddSubFolder(dialog.Answer);
            dialog.Close();
            //refresh layout
            TreeViewFolder.Items.Refresh();
            TreeViewPlans.Items.Refresh();
            TreeViewProjects.Items.Refresh();
        }

        private void MenuItem_FolderNeighbor(object sender, RoutedEventArgs e)
        {
            var tmp = (MenuItem)e.Source;
            var folder = (Folder)tmp.DataContext;
            if (folder == null)
            {
                MessageBox.Show("Please select a Project", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var dialog = new InputDialog("Please enter the folder name:", "Example Folder");
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
            folder.RootFolder.AddSubFolder(dialog.Answer);
            dialog.Close();
            //Refresh layout
            TreeViewFolder.Items.Refresh();
            TreeViewPlans.Items.Refresh();
            TreeViewProjects.Items.Refresh();
        }
        
    }
}
