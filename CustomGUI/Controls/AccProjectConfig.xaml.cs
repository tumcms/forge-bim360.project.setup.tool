using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        public List<Bim360Project> projects { get; set; }
        private string csvpath { get; set; }

        private Bim360Project activeProject { set; get; }

        private Folder activeFolder { set; get; }

        private UserPermission activePermission { set; get; }



        public AccProjectConfig()
        {
            InitializeComponent();
            projects = new List<Bim360Project>();

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

        #region Import / Export
        //Export / Import of Projects 
        /// <summary>
        /// Export currently only supports 10 Subfolder
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
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
                                MissingFieldFound = null,
                            };
                        }
                        else
                        {
                            //CSV with Invariant Config
                            csvconfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                            {
                                HeaderValidated = null,
                                MissingFieldFound = null,
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
                    if (tmp != null)
                    {
                        csv.WriteRecords(tmp);
                    }
                }
            }

        }

        public string ExportBim360Projects()
        {
            
            string retu=new string("");
            MemoryStream stream = new MemoryStream();
            using (var streamWriter = new StreamWriter(stream))
            {
                //maybe the CultureInfo needs to be changed
                //change that everything should get written
                //CsvConfiguration test = new CsvConfiguration()
                using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    //Maps the Header of the CSV Data to the class attributes
                    var tmp = SerializationParser.ExportBim360ToCSV(projects);
                    if (tmp == null)
                    {
                        return retu;
                    }
                    //this is really dirty but it works for now
                    for (int i=0;i<50;i++)
                    {
                        tmp.Add(new UserData());
                    }
                    csv.Context.RegisterClassMap<UserDataExport>();
                    //csv.WriteHeader<UserData>();
                    csv.WriteRecords(tmp);
                    // convert stream to string
                    stream.Position = 0;
                    StreamReader reader = new StreamReader(stream);
                    retu = reader.ReadToEnd().ToString();
                }
            }

            return retu;
        }
        #endregion

        #region Userinteraction functions
        //GUI User Interactions
        private void ProjectsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //should be right i guess
            if (ProjectsView.SelectedCells[0].Item.GetType() == typeof(Bim360Project))
            {
                activeProject = (Bim360Project) ProjectsView.SelectedCells[0].Item;
                TreeViewPlans.ItemsSource = activeProject.Plans.Subfolders;
                TreeViewProjects.ItemsSource = activeProject.ProjectFiles.Subfolders;
                ProjectTypeComboBox.SelectedItem = Selection.SelectProjectType(activeProject.ProjectType);
            }
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (activeProject == null)
            {
                MessageBox.Show("No Project selected", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //set active Folder 
            if (TreeViewFolder.SelectedItem.GetType() == typeof(Folder))
            {
                activeFolder = (Folder)TreeViewFolder.SelectedItem;
            }
            else if (activeProject != null)
            {
                //this is needed because of the Static implimentation of the Rootfolders
                if (((TreeViewItem)TreeViewFolder.SelectedItem).Header.ToString() == "Plans")
                {
                    activeFolder = activeProject.Plans;
                }
                else if (((TreeViewItem)TreeViewFolder.SelectedItem).Header.ToString() == "Project Files")
                {
                    activeFolder = activeProject.ProjectFiles;
                }
            }
            
            UserPermissionView.ItemsSource = activeFolder.UserPermissions;
            RolePermissionView.ItemsSource = activeFolder.RolePermissions;
            RoleView.ItemsSource = null;

        }

        private void UserPermissionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            activePermission = (UserPermission)UserPermissionView.SelectedItem;
            if (activePermission != null)
            {
                RoleView.ItemsSource = activePermission.AssignedUsers.IndustryRoles;
            }

        }

        private void ProjectTypeComboBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (activeProject != null)
            {
                if (string.IsNullOrEmpty(ProjectTypeComboBox.Text.ToString()))
                {
                    activeProject.ProjectType = Selection.SelectProjectType(ProjectTypeComboBox.Text.ToString());
                }
            }
        }

        private void Button_AddProject(object sender, RoutedEventArgs e)
        {
            projects.Add(new Bim360Project(Namenewproject.Text));
            ProjectsView.ItemsSource = projects;
            activeProject =projects.Last();
            ProjectsView.Items.Refresh();
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
                var tmp = new UserPermission(iteruser.Trim(), (AccessPermissionEnum) 
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
            //refresh view
            UserPermissionView.Items.Refresh();
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
            //refresh view
            RolePermissionView.Items.Refresh();

        }

        private void MenuItem_FolderChild(object sender, RoutedEventArgs e)
        {
            //get the selected Item
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
            if (dialog.DialogResult==true)
            {
                folder.AddSubFolder(dialog.Answer);
            }
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
            if (dialog.DialogResult == true)
            {
                folder.RootFolder.AddSubFolder(dialog.Answer);
            }
            dialog.Close();
            //Refresh layout
            TreeViewFolder.Items.Refresh();
            TreeViewPlans.Items.Refresh();
            TreeViewProjects.Items.Refresh();
        }
        
        private void MenuItem_ProjectDelete(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;
            //Get the underlying item
            var toDeleteFromBindedList = (Bim360Project)item.SelectedCells[0].Item;
            if (toDeleteFromBindedList == null)
            {
                return;
            }
            //reorgenize
            if (toDeleteFromBindedList == activeProject)
            {
                activeProject = null;
            }
            activeFolder = null;
            activePermission = null;
            projects.Remove(toDeleteFromBindedList);
            //If last Project is deletet remove all other reference to it
            if (projects.Count==0)
            {
                activeProject = null;
                activeFolder = null;
                activePermission = null;
                TreeViewFolder.ItemsSource = null;
                TreeViewFolder.Items.Refresh();
                TreeViewPlans.ItemsSource = null;
                TreeViewPlans.Items.Refresh();
                TreeViewProjects.ItemsSource = null;
                TreeViewProjects.Items.Refresh();
                RolePermissionView.ItemsSource = null;
                RolePermissionView.Items.Refresh();
            }
            ProjectsView.Items.Refresh();


        }
        
        private void MenuItem_ProjectDuplicate(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;
            //Copy Itme
            var toduplicate = (Bim360Project)item.SelectedCells[0].Item;
            if (toduplicate == null)
            {
                return;
            }
            var toadd = new Bim360Project(toduplicate);
            projects.Add(toadd);
            ProjectsView.Items.Refresh();

        }

        private void MenuItem_UserDelete(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;
            //errorhandling
            if (!item.SelectedCells.Any())
            {
                MessageBox.Show("No User to delete", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Get the underlying item
            var toDeleteFromList = (UserPermission)item.SelectedCells[0].Item;
            //reorgenize
            if (toDeleteFromList == activePermission)
            {
                activePermission = null;
            }
            activeFolder.UserPermissions.Remove(toDeleteFromList);
            UserPermissionView.Items.Refresh();

        }

        private void MenuItem_UserModify(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;
            if (!item.SelectedCells.Any())
            {
                MessageBox.Show("No User to modify", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Get the underlying item
            //if(item.SelectedCells[0].Item.GetType().Equals())
            var toModifyFromList = (UserPermission)item.SelectedCells[0].Item;

            //reorgenize
            InputModifyUser dialog;
            //inputdialog for user to modify 
            if (toModifyFromList.AssignedUsers.AssignedCompany != null)
            {
                dialog = new InputModifyUser(toModifyFromList.AssignedUsers.MailAddress,
                    toModifyFromList.AccessPermission, toModifyFromList.AssignedUsers.AssignedCompany.Name);
            }
            else
            {
                dialog = new InputModifyUser(toModifyFromList.AssignedUsers.MailAddress,
                    toModifyFromList.AccessPermission);
            }

            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
            //set values + refesh view
            toModifyFromList.AssignedUsers.MailAddress = dialog.UserRet;
            toModifyFromList.AccessPermission = dialog.AccessRet;
            //if no Company was set -> create a new one
            if (toModifyFromList.AssignedUsers.AssignedCompany == null && 
                !string.IsNullOrWhiteSpace(dialog.CompanyRet))
            {
                toModifyFromList.AssignedUsers.AssignedCompany =new Company(dialog.CompanyRet);
            }
            else if (toModifyFromList.AssignedUsers.AssignedCompany != null)
            {
                toModifyFromList.AssignedUsers.AssignedCompany.Name = dialog.CompanyRet;
            }
            
            dialog.Close();
            UserPermissionView.Items.Refresh();

        }

        private void MenuItem_AddRole(object sender, RoutedEventArgs e)
        {
            if (activePermission == null)
            {
                return;
            }
            //ask user about the role
            var dialog = new InputDialog("Please enter the Role name:", "Example Role");
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                //remove empty Role
                foreach (var iter in activePermission.AssignedUsers.IndustryRoles.ToList())
                {
                    if (string.IsNullOrWhiteSpace(iter))
                    {
                        activePermission.AssignedUsers.IndustryRoles.Remove(iter);
                    }
                }

                activePermission.AssignedUsers.IndustryRoles.Add(dialog.Answer);
            }

            dialog.Close();
            //refresh layout
            RoleView.Items.Refresh();

        }

        private void MenuItem_RoleDelete(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;
            //errorhandling
            if (!item.SelectedCells.Any())
            {
                MessageBox.Show("No Role to delete", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Get the underlying item
            var toDeleteFromList = (string)item.SelectedCells[0].Item;
            if (activePermission == null)
            {
                return;
            }
            //reorgenize
            activePermission.AssignedUsers.IndustryRoles.Remove(toDeleteFromList);
            RoleView.Items.Refresh();
        }

        private void MenuItem_RolePermissionDelete(object sender, RoutedEventArgs e)
        {
            
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;
            //if no Role is in the view
            if (!item.SelectedCells.Any())
            {
                MessageBox.Show("No Rule to delete", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Get the underlying item
            var toDeleteFromList = (RolePermission)item.SelectedCells[0].Item;
            if (activeFolder == null)
            {
                return;
            }
            //reorgenize
            activeFolder.RolePermissions.Remove(toDeleteFromList);
            RolePermissionView.Items.Refresh();
        }

        private void MenuItem_RolePermissionModify(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;
            //if no Role is in the view
            if (!item.SelectedCells.Any())
            {
                MessageBox.Show("No Rule to modify", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Get the underlying item
            var toModifyFromList = (RolePermission)item.SelectedCells[0].Item;
            if (activeFolder == null)
            {
                return;
            }
            //reorgenize
            var dialog = new InputModifyRole(toModifyFromList.Role, toModifyFromList.AccessPermission)
            {
                ResizeMode = ResizeMode.NoResize
            };
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                //set values + refesh view
                toModifyFromList.Role = dialog.RoleRet;
                toModifyFromList.AccessPermission = dialog.AccessRet;
            }

            dialog.Close();
            RolePermissionView.Items.Refresh();

        }

        private void MenuItem_ProjectRename(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;
            //Get the underlying item
            //if(item.SelectedCells[0].Item.GetType().Equals())
            var toModifyFromList = (Bim360Project)item.SelectedCells[0].Item;
            if (toModifyFromList == null)
            {
                MessageBox.Show("Please select a Project", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var dialog = new InputDialog("Please enter the new Project name:", "Example Project");
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                toModifyFromList.ProjectName = dialog.Answer;
            }
            dialog.Close();
            //refresh layout
            ProjectsView.Items.Refresh();

        }

        private void MenuItem_RootFolderChild(object sender, RoutedEventArgs e)
        {
            if (activeProject == null)
            {
                MessageBox.Show("Please select a Project", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var menuItem = (MenuItem)sender;
            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;
            //Find the placementTarget
            var item = (TreeViewItem)contextMenu.PlacementTarget;
            //Get the underlying item
            Folder folder;
            if (item.Header.ToString() == "Plans")
            {
                folder = activeProject.Plans;
            }else if (item.Header.ToString() == "Project Files")
            {
                folder = activeProject.ProjectFiles;
            }
            else
            {
                //this should never happen
                return;
            }
            
            var dialog = new InputDialog("Please enter the folder name:", "Example Folder");
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                folder.AddSubFolder(dialog.Answer);
            }

            dialog.Close();
            //refresh layout
            TreeViewFolder.Items.Refresh();
            TreeViewPlans.Items.Refresh();
            TreeViewProjects.Items.Refresh();
        }

        private void MenuItem_RemoveFolder(object sender, RoutedEventArgs e)
        {
            var tmp = (MenuItem)e.Source;
            var folder = (Folder)tmp.DataContext;
            if (folder == null)
            {
                MessageBox.Show("Please select a Project", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //just to make sure
            if (MessageBox.Show("Are you sure?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }

            folder.RootFolder.Subfolders.Remove(folder);
            //Refresh layout
            TreeViewFolder.Items.Refresh();
            TreeViewPlans.Items.Refresh();
            TreeViewProjects.Items.Refresh();
        }
        private void MenuItem_RenameFolder(object sender, RoutedEventArgs e)
        {
            //get data form context
            var tmp = (MenuItem)e.Source;
            var folder = (Folder)tmp.DataContext;
            if (folder == null)
            {
                MessageBox.Show("Please select a Project", "Error"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Userinteraction
            var dialog = new InputDialog("Please enter the folder name:", "Example Folder");
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
            {
                folder.Name = dialog.Answer.Trim();
            }
            dialog.Close();
            //Refresh layout
            TreeViewFolder.Items.Refresh();
            TreeViewPlans.Items.Refresh();
            TreeViewProjects.Items.Refresh();
        }

        #endregion


    }
}
