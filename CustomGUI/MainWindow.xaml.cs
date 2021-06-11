﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using Autodesk.Forge.BIM360.Serialization;
using BimProjectSetupAPI.Workflows;
using BimProjectSetupCommon;
using BimProjectSetupCommon.Helpers;
using BimProjectSetupCommon.Workflow;
using CustomGUI.Controls;
using File = System.IO.File;
using static CustomBIMFromCSV.Tools;
using Microsoft.WindowsAPICodePack.Dialogs;


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
            
            ProjectWorkflow projectProcess = new ProjectWorkflow(options);
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

            // load all existing projects from the BIM360 environment
            List<BimProject> projects = projectProcess.GetAllProjects();


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
            dialog.ShowDialog();
            try
            {
                var tmp = dialog.FileName;
            }
            catch (Exception ex)
            {
                statusbar.Text = ex.Message;
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


    }
}

