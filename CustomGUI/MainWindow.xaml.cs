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
using Microsoft.Win32;
using Folder = AdskConstructionCloudBreakdown.Folder;


namespace CustomGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //Global Data
        List<UserData> usermanag = new List<UserData>();

        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string BimId { get; set; }
        private string AdminMail { get; set; }

        private string path_file =@".\Config\config.txt";
        private string path_last =@".\Config\last.txt";

        
        /// <summary>
        /// Initializer for main window
        /// </summary>
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
            AdminMail = ((ForgeConfig) window_config.Content).AdminMail_Box.Text.ToString();
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
                        AdminMail = reader.ReadLine();
                    }
                }
            }

        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccProjectConfig.ProjectsView.Items.Refresh();
        }

        #region Buttons and userinteractions

        /// <summary>
        /// runs the upload to the BIM360 environment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                                        "-h", AdminMail ,"-f"," .\\sample","-t",",",
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
                        if (ServiceTab.CheckBoxservices.IsChecked == true)
                        {
                            #region Add Services
                            var listname = new List<string>();
                            listname.Add("admin");
                            listname.Add("doc_manage");
                            if (ServiceTab.CheckBoxpm.IsChecked == true)
                            {
                                listname.Add("pm");
                            }
                            if (ServiceTab.CheckBoxfng.IsChecked == true)
                            {
                                listname.Add("fng");
                            }
                            if (ServiceTab.CheckBoxcollab.IsChecked == true)
                            {
                                listname.Add("collab");
                            }
                            if (ServiceTab.CheckBoxcost.IsChecked == true)
                            {
                                listname.Add("cost");
                            }
                            if (ServiceTab.CheckBoxgng.IsChecked == true)
                            {
                                listname.Add("gng");
                            }
                            if (ServiceTab.CheckBoxglue.IsChecked == true)
                            {
                                listname.Add("glue");
                            }
                            if (ServiceTab.CheckBoxplan.IsChecked == true)
                            {
                                listname.Add("plan");
                            }
                            if (ServiceTab.CheckBoxfield.IsChecked == true)
                            {
                                listname.Add("field");
                            }
                            if (ServiceTab.CheckBoxassete.IsChecked == true)
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
                                serviceList.Last().company = ServiceTab.Company.Text.Trim();
                                serviceList.Last().email = AdminMail;
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
            this.statusbar.Text = "check out www.github.com/tumcms for more information!";
        }

        /// <summary>
        /// exposes a saveFileDialog window to the user to store the current configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            var browser = new SaveFileDialog
            {
                FileName = "BIM360_Custom_Template.csv",
                RestoreDirectory = true,
                Title = "Saving configuration file...",
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            browser.ShowDialog();

            // specify path
            var path = Path.GetFullPath(browser.FileName);

            //export the Projects
            try
            {
                AccProjectConfig.ExportBim360Projects(path);
            }
            catch (Exception ex)
            {
                statusbar.Text = ex.Message;
                return;
            }
            statusbar.Text = "Export successful";

        }

        /// <summary>
        /// provides a user dialog to load a csv file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Import_OnClick(object sender, RoutedEventArgs e)
        {
            var browser = new OpenFileDialog
            {
                Multiselect = false, 
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
        };

            // show browser
            browser.ShowDialog();

            var fileName = browser.FileName;

            //Load CSV into Mainwindow
            if (AccProjectConfig.LoadBim360Projects(fileName))
            {
                statusbar.Text = "Import successful!";
                using (FileStream fs = File.OpenWrite(path_last))
                {
                    using (var sr = new StreamWriter(fs))
                    {
                        //Delete the content of the file
                        sr.Write(string.Empty);
                        sr.WriteLine(fileName);
                    }
                }
            }
            else
            {
                statusbar.Text = "Import failed! Unknown CSV Config or File not found!";
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetTemplate_OnClick(object sender, RoutedEventArgs e)
        {
            var progress = new updates_upload();
            progress.Show();

            //Updates
            progress.pgb.Value = 10;
            progress.ProgressLabel.Content = "Build Connection";
            // ProgressBar "refresh"
            CallDispatch(progress);

            string[] input = new string[] {"-c", ClientId ,"-s", ClientSecret, "-a" ,BimId ,
                "-h", AdminMail ,"-f"," .\\sample","-t",",",
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
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

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roots"></param>
        /// <returns></returns>
        private Folder getNestedFolder(NestedFolder roots)
        {
            var currentFolder = new Folder(roots.name);
            
            foreach (var iterfolder in roots.childrenFolders)
            {
                currentFolder.AddSubFolder(getNestedFolder(iterfolder));

            }

            return currentFolder;
        }

    }
}

