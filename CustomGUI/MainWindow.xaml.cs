using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AdskConstructionCloudBreakdown;
using Autodesk.Forge.BIM360.Serialization;
using BimProjectSetupAPI.Workflows;
using BimProjectSetupCommon;
using BimProjectSetupCommon.Helpers;
using BimProjectSetupCommon.Workflow;
using CustomGUI.Controls;
using File = System.IO.File;
using static CustomBIMFromCSV.Tools;
using Microsoft.WindowsAPICodePack.Dialogs;
using CustomGUI.Service;
using Folder = AdskConstructionCloudBreakdown.Folder;


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

        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string BimId { get; set; }
        private string Adminmail { get; set; }

        private string path_file =@".\Config\config.txt";
        private string path_last =@".\Config\last.txt";




        public MainWindow()
        {
            InitializeComponent();

        }

        

        //Event Handling

        //Config subwindow
        private void Config_Click(object sender, RoutedEventArgs e)
        {
            var window_config = new Window();
            window_config.Owner = this;
            window_config.Content = new ForgeConfig();
            window_config.Width = 710;
            window_config.SizeToContent = SizeToContent.Height;
            window_config.ResizeMode = ResizeMode.NoResize;
            window_config.ShowDialog();

            BimId = ((ForgeConfig) window_config.Content).BimId_Box.Text.ToString();
            ClientSecret = ((ForgeConfig) window_config.Content).ClientSecret_Box.Text.ToString();
            ClientId = ((ForgeConfig) window_config.Content).ClientId_Box.Text.ToString();
            Adminmail = ((ForgeConfig) window_config.Content).AdminMail_Box.Text.ToString();

        }


        //UserManagement

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

            var filePath =@".\Config\config.txt";
            //Get Last Config from Login Data
            if (File.Exists(@".\Config\config.txt"))
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        ClientId = reader.ReadLine();
                        ClientSecret = reader.ReadLine();
                        BimId = reader.ReadLine();
                        Adminmail = reader.ReadLine();
                    }
                }
            }

        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccProjectConfig.ProjectsView.Items.Refresh();
        }


        //Upload
        private void Upload_OnClick(object sender, RoutedEventArgs e)
        {
            var progress = new updates_upload();
            progress.Show();
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(AccProjectConfig.ExportBim360Projects());
            MemoryStream dataset = new MemoryStream(byteArray);
            //dataset.Position = 0;


            //Updates
            progress.pgb.Value = 10;
            progress.ProgressLabel.Content = "Build Connection";
            // ProgressBar "refresh"
            CallDispatch(progress);

            //maybe change
            string[] input = new string[] {"-c", ClientId ,"-s", ClientSecret, "-a" ,BimId , 
                                        "-h", Adminmail ,"-f"," .\\sample","-t",",",
                                           "-z",",","-e","UTF-8","-d","yyyy-MM-dd"} ;

            
            // Delete previous versions of log.txt
            System.IO.File.Delete("Log/logInfo.txt");
            System.IO.File.Delete("Log/logImportant.txt");

            AppOptions options = AppOptions.Parse(input);
            // options.AccountRegion = "EU"; 
            options.AdminRole = "Project Manager";

            ProjectWorkflow projectProcess = new ProjectWorkflow(options);
            System.Threading.Thread.Sleep(1000);
            // load all existing projects from the BIM360 environment
            List<BimProject> projects = projectProcess.GetAllProjects();

            FolderWorkflow folderProcess = new FolderWorkflow(options);
            ProjectUserWorkflow projectUserProcess = new ProjectUserWorkflow(options);
            AccountWorkflow accountProcess = new AccountWorkflow(options);

            //Updates
            progress.pgb.Value = 25;
            progress.ProgressLabel.Content = "Convert Data from GUI";
            // ProgressBar "refresh"
            CallDispatch(progress);


            // load data from custom CSV file. Filepath was set in constructor of projectProcess by pushing the options instance
            DataTable csvData = new DataTable();
            csvData=ProjectWorkflow.CustomGetDataFromCsv_stream(dataset);
       

            //Updates
            progress.pgb.Value = 40;
            progress.ProgressLabel.Content = "Uploading";
            // ProgressBar "refresh"
            CallDispatch(progress);

            


            List<BimCompany> companies = null;
            BimProject currentProject = null;
            List<HqUser> projectUsers = null;
            List<NestedFolder> folders = null;
            NestedFolder currentFolder = null;


            //Uploading
            try
            {
                for (int row = 0; row < csvData.Rows.Count; row++)
                {
                    string projectName = csvData.Rows[row]["project_name"].ToString();

                    // check if the current row defines the start point for another project (project_name column must be set)
                    if (!string.IsNullOrEmpty(projectName))
                    {
                        Util.LogImportant($"\nCurrent project: {projectName}");

                        // check if the requested project name is already present inside BIM360
                        currentProject = projects.Find(x => x.name == projectName);

                        if (currentProject == null)
                        {
                            // create the project
                            projects = projectProcess.CustomCreateProject(csvData, row, projectName, projectProcess);

                            // set the current project variable to the recently created project
                            currentProject = projects.Find(x => x.name == projectName);

                            // verify the initialization of the new project
                            CheckProjectCreated(currentProject, projectName);
                        }

                        

                        // create the folder structure
                        folders = folderProcess.CustomGetFolderStructure(currentProject);

                        // add the companies
                        companies = accountProcess.CustomUpdateCompanies(csvData, row, accountProcess);

                        // create or update the project users
                        projectUsers = projectUserProcess.CustomUpdateProjectUsers(csvData, row, companies,
                            currentProject, projectUserProcess);

                        //activate all services
                        ServiceWorkflow serviceProcess = new ServiceWorkflow(options);

                        //check what servers needs to be activated
                        if (Servicetab.CheckBoxservices.IsChecked == true)
                        {
                            #region Add Services
                            var listname = new List<string>();
                            listname.Add("admin");
                            listname.Add("doc_manage");
                            if (Servicetab.CheckBoxpm.IsChecked == true)
                            {
                                listname.Add("pm");
                            }
                            if (Servicetab.CheckBoxfng.IsChecked == true)
                            {
                                listname.Add("fng");
                            }
                            if (Servicetab.CheckBoxcollab.IsChecked == true)
                            {
                                listname.Add("collab");
                            }
                            if (Servicetab.CheckBoxcost.IsChecked == true)
                            {
                                listname.Add("cost");
                            }
                            if (Servicetab.CheckBoxgng.IsChecked == true)
                            {
                                listname.Add("gng");
                            }
                            if (Servicetab.CheckBoxglue.IsChecked == true)
                            {
                                listname.Add("glue");
                            }
                            if (Servicetab.CheckBoxplan.IsChecked == true)
                            {
                                listname.Add("plan");
                            }
                            if (Servicetab.CheckBoxfield.IsChecked == true)
                            {
                                listname.Add("field");
                            }
                            if (Servicetab.CheckBoxassete.IsChecked == true)
                            {
                                listname.Add("assets");
                            }
                            #endregion

                            var serviceList = new List<ServiceActivation>();
                            foreach (var iter in listname)
                            {
                                serviceList.Add(new ServiceActivation());
                                serviceList.Last().service_type = iter;
                                serviceList.Last().project_name = projectName;
                                //test hardcoded Test company name needs to be enter or find out
                                serviceList.Last().company = Servicetab.Company.Text.Trim();
                                serviceList.Last().email = Adminmail;
                            }

                            serviceProcess.ActivateServicesProcess(
                                new List<BimProject>(new BimProject[] {currentProject}), serviceList);

                        }


                    }

                    // assign permissions
                    currentFolder = CreateFoldersAndAssignPermissions(csvData, row, projectUsers, folderProcess,
                        folders, currentFolder, currentProject, projectUserProcess);

                    // run the file upload if requested
                    UploadFilesFromFolder(csvData, row, folderProcess, currentFolder, currentProject.id,
                        options.LocalFoldersPath);
                    //Updates
                    progress.pgb.Value = 50+(50/ csvData.Rows.Count*row);
                    progress.ProgressLabel.Content = "Uploading";
                    // ProgressBar "refresh"
                    CallDispatch(progress);

                }
            }
            catch(Exception ex)
            {
                //show the error
                statusbar.Text = ex.Message;
                progress.Close();
                return;
            }

            statusbar.Text = "Upload successful";
            progress.Close();
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException("Just look into the git");
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            //maybe change
            string filename = "\\BIM360_Custom_Template.csv";

            //window for user to enter data
            var dialog = new InputDialog("Please enter the path for exporting the CSV:",
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ShowDialog();
            if (dialog.DialogResult != true)
            {
                return;
            }
            var exportpath = dialog.Answer;
            //move to conifg
            if (File.Exists(exportpath + filename))
            {
                statusbar.Text = "Export Failed! File already exists!";
                return;
            }

            if (!Directory.Exists((exportpath)))
            {
                try
                {
                    Directory.CreateDirectory(exportpath);
                }
                catch (Exception ex)
                {
                    statusbar.Text = ex.Message;
                    return;
                }
            }

            //Hardcoded Name in here maybe user should be able to change


            //export the Projects
            try
            {
                AccProjectConfig.ExportBim360Projects(exportpath + filename);
            }
            catch (Exception ex)
            {
                statusbar.Text = ex.Message;
                return;
            }
            statusbar.Text = "Export successful";

            dialog.Close();
        }

        private void Import_OnClick(object sender, RoutedEventArgs e)
        {
            //Folderbrowser
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = false;
            dialog.Multiselect = false;
            //if use canceled the selection
            if (Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogResult.Cancel ==
                dialog.ShowDialog())
            {
                statusbar.Text = "canceled";
                return;
            }

            //Load CSV into Mainwindow
            if (AccProjectConfig.LoadBim360Projects(dialog.FileName))
            {
                statusbar.Text = "Import successful!";
                using (FileStream fs = File.OpenWrite(path_last))
                {
                    using (var sr = new StreamWriter(fs))
                    {
                        //Delete the content of the file
                        sr.Write(string.Empty);
                        sr.WriteLine(dialog.FileName);
                    }
                }
            }
            else
            {
                statusbar.Text = "Import failed! Unknown CSV Config or File not found!";
            }


        }

        private void GetTemplete_OnClick(object sender, RoutedEventArgs e)
        {
            var progress = new updates_upload();
            progress.Show();

            //Updates
            progress.pgb.Value = 10;
            progress.ProgressLabel.Content = "Build Connection";
            // ProgressBar "refresh"
            CallDispatch(progress);

            string[] input = new string[] {"-c", ClientId ,"-s", ClientSecret, "-a" ,BimId ,
                "-h", Adminmail ,"-f"," .\\sample","-t",",",
                "-z",",","-e","UTF-8","-d","yyyy-MM-dd"};

            AppOptions options = AppOptions.Parse(input);
            // options.AccountRegion = "EU"; 
            options.AdminRole = "Project Manager";

            ProjectWorkflow projectProcess = new ProjectWorkflow(options);
            FolderWorkflow folderProcess = new FolderWorkflow(options);
            System.Threading.Thread.Sleep(1000);
            // load all existing projects from the BIM360 environment
            List<BimProject> projects = projectProcess.GetAllProjects();

            //Updates
            progress.pgb.Value = 30;
            progress.ProgressLabel.Content = "Download Project";
            // ProgressBar "refresh"
            CallDispatch(progress);

            BimProject currentProject = null;

            // get project from bim360
            try
            {
                currentProject = projects.Find(x => x.name == TextBoxTemplate.Text.Trim());
            }
            catch (Exception)
            {
                statusbar.Text = "No Project with the name:" + TextBoxTemplate.Text.Trim() + " found!";
                return;
            }
            if (currentProject == null)
            {
                return;
            }

            var nestedfolders = folderProcess.CustomGetFolderStructure(currentProject);
            foreach (var iterfolder in nestedfolders)
            {
               

            }

            progress.pgb.Value = 50;
            progress.ProgressLabel.Content = "Converte to local Project 0/2";
            // ProgressBar "refresh"
            CallDispatch(progress);


            var newproject = new Bim360Project(TextBoxTemplate.Text.Trim());
            newproject.ProjectType = Selection.SelectProjectType(currentProject.project_type);
            var roots = new List<Folder>();
            //hier rekursive alle ordner finden
            foreach (var iterfolder in nestedfolders)
            {
                roots.Add(getNestedFolder(iterfolder));
                progress.pgb.Value = 70;
                progress.ProgressLabel.Content = "Converte to local Project 1/2";
                // ProgressBar "refresh"
                CallDispatch(progress);
            }
            //here hardcoded plan and projectfiles need to be changed in the furtuer if  ACC comes
            if (roots[0].Name == "Plans")
            {
                newproject.Plans = roots[0];
                newproject.ProjectFiles = roots[1];
            }
            else
            {
                newproject.Plans = roots[1];
                newproject.ProjectFiles = roots[0];
            }

            AccProjectConfig.ProjectsView.ItemsSource = AccProjectConfig.projects;
            AccProjectConfig.projects.Add(newproject);
            AccProjectConfig.ProjectsView.Items.Refresh();
            progress.Close();
            statusbar.Text = "Import successful";

        }


        //Progress updates
        Action EmptyDelegate = delegate () { };

        void CallDispatch(updates_upload input)
        {
            input.pgb.Dispatcher.Invoke(
                EmptyDelegate,
                System.Windows.Threading.DispatcherPriority.Background
            );
            input.ProgressLabel.Dispatcher.Invoke(
                EmptyDelegate,
                System.Windows.Threading.DispatcherPriority.Background
            );

        }

        //translate Nestedfolder into Folder of C# Code
        private Folder getNestedFolder(NestedFolder roots)
        {
            var currentfolder = new Folder(roots.name);
            var subfolder = new List<Folder>();
            foreach (var iterfolder in roots.childrenFolders)
            {
                currentfolder.AddSubFolder(getNestedFolder(iterfolder));

            }

            return currentfolder;
        }


        
    }
}

