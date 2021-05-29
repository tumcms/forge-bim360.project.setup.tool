﻿using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
using Autodesk.Forge.BIM360.Serialization;
using BimProjectSetupAPI.Workflows;
using BimProjectSetupCommon;
using BimProjectSetupCommon.Helpers;
using BimProjectSetupCommon.Workflow;
using CustomGUI.Controls;
using File = System.IO.File;
using static CustomBIMFromCSV.Tools;


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
            window_config.Height = 150;
            window_config.ResizeMode = ResizeMode.NoResize;
            window_config.ShowDialog();

            BimId = ((ForgeConfig) window_config.Content).BimId_Box.Text.ToString();
            ClientSecret = ((ForgeConfig) window_config.Content).ClientSecret_Box.Text.ToString();
            ClientId = ((ForgeConfig) window_config.Content).ClientId_Box.Text.ToString();

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
                statusbar.Text = "Import failed! Unknown CSV Config or File not found!";
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
            AccProjectConfig.ExportBim360Projects(csvpathexp.Text+ filename);

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
                    }
                }
            }

        }
        private void Csvpathexp_OnInitialized(object? sender, EventArgs e)
        {
            csvpathexp.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

     
        private void Upload_OnClick(object sender, RoutedEventArgs e)
        {


            if (!Directory.Exists((".\\sample")))
            {
                var tmp = ".\\sample";

                Directory.CreateDirectory((".\\sample"));
            }

            //Hardcoded Name in here maybe user should be able to change
            string filename = ".\\sample\\BIM360_Custom_Template.csv";

            //export the Projects
            AccProjectConfig.ExportBim360Projects(filename);

            //maybe change
            string[] input = new string[] {"-c", ClientId ,"-s", ClientSecret, "-a" ,BimId , "-p" ,
                                           filename,"-h"," stefan.1995.huber@tum.de","-f"," .\\sample","-t",",",
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

            // load data from custom CSV file. Filepath was set in constructor of projectProcess by pushing the options instance
            statusbar.Text = "Read in Data from ProjectConfig";
            DataTable csvData = projectProcess.CustomGetDataFromCsv();

            // load all existing projects from the BIM360 environment
            statusbar.Text = "Checking projects";
            List<BimProject> projects = projectProcess.GetAllProjects();


            List<BimCompany> companies = null;
            BimProject currentProject = null;
            List<HqUser> projectUsers = null;
            List<NestedFolder> folders = null;
            NestedFolder currentFolder = null;


            statusbar.Text = "Formatting Data for Upload";
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
                    statusbar.Text = "Uploading...";
                    UploadFilesFromFolder(csvData, row, folderProcess, currentFolder, currentProject.id,
                        options.LocalFoldersPath);
                }
            }
            catch
            {
                statusbar.Text = "Error on Formatting Data and Uploading";
                return;
            }

            statusbar.Text = "Upload successful";
            File.Delete(filename);
        }


    }
}
