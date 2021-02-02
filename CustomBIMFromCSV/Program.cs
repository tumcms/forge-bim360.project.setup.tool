using System.Data;
using System.Collections.Generic;

using static CustomBIMFromCSV.Tools;

using BimProjectSetupCommon;
using BimProjectSetupCommon.Workflow;
using BimProjectSetupCommon.Helpers;

using Autodesk.Forge.BIM360.Serialization;
using BimProjectSetupAPI.Workflows;

namespace CustomBIMFromCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            // Delete previous versions of log.txt
            System.IO.File.Delete("Log/logInfo.txt");
            System.IO.File.Delete("Log/logImportant.txt");
            
            AppOptions options = AppOptions.Parse(args);
            // options.AccountRegion = "EU";

            ProjectWorkflow projectProcess = new ProjectWorkflow(options);
            FolderWorkflow folderProcess = new FolderWorkflow(options);
            ProjectUserWorkflow projectUserProcess = new ProjectUserWorkflow(options);
            AccountWorkflow accountProcess = new AccountWorkflow(options);

            // load data from custom CSV file. Filepath was set in constructor of projectProcess by pushing the options instance
            DataTable csvData = projectProcess.CustomGetDataFromCsv();

            // load all existing projects from the BIM360 environment
            List<BimProject> projects = projectProcess.GetAllProjects();

            List<BimCompany> companies = null;
            BimProject currentProject = null;
            List<HqUser> projectUsers = null;
            List<NestedFolder> folders = null;
            NestedFolder currentFolder = null;

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
                    projectUsers = projectUserProcess.CustomUpdateProjectUsers(csvData, row, companies, currentProject, projectUserProcess);
                }

                // assign permissions
                currentFolder = CreateFoldersAndAssignPermissions(csvData, row, projectUsers, folderProcess, folders, currentFolder, currentProject, projectUserProcess);

                // run the file upload if requested
                UploadFilesFromFolder(csvData, row, folderProcess, currentFolder, currentProject.id, options.LocalFoldersPath);
            }
        }
    }
}
