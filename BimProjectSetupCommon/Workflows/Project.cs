/////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved
// Written by Forge Partner Development
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using Autodesk.Forge.BIM360.Serialization;
using BimProjectSetupCommon.Helpers;

namespace BimProjectSetupCommon.Workflow
{
    public class ProjectWorkflow : BaseWorkflow
    {
        public static bool includeActiveServicesToCsv = false;

        public ProjectWorkflow(AppOptions options) : base(options)
        {
            DataController.InitializeAllProjects();
        }
        /// <summary>
        /// Loads the data provided in the custom CSV template into a DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable CustomGetDataFromCsv()
        {
            return CsvReader.CustomReadDataFromCSV();
        }

        public static DataTable CustomGetDataFromCsv_stream(Stream input)
        {
            return CsvReader.ReadFile(input);
            
        }

        /// <summary>
        /// Creates a project from custom DataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <param name="projectName"></param>
        /// <param name="projectProcess"></param>
        /// <returns></returns>
        public List<BimProject> CustomCreateProject(DataTable table, int row, string projectName, ProjectWorkflow projectProcess)
        {
            Util.LogInfo("\nCreating project: " + projectName + "\n");

            BimProject newProject = DataController.CustomGetBimProject(table, row, projectName);
            string response = DataController.AddProject(newProject);
            if (response == "error")
            {
                Util.LogError($"There was a problem creating project with name: {projectName}. The program has been stopped.");
                throw new ApplicationException($"Stopping the program... You can see the log file for more information.");
            }

            List <BimProject> updatedProjects = projectProcess.GetAllProjects();

            return updatedProjects;
        }
        /// <summary>
        /// 
        /// </summary>
        public void CreateProjectsProcess()
        {
            try
            {
                CsvReader.ReadDataFromProjectCSV();
                if (false == _options.TrialRun)
                {
                    CreateProjects();
                }
                else
                {
                    Log.Info("Trial run finished. No further processing");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateProjectsProcess()
        {
            try
            {
                CsvReader.ReadDataFromProjectCSV();
                if (false == _options.TrialRun)
                {
                    UpdateProjects();
                }
                else
                {
                    Log.Info("Trial run (-r option is true) - no further processing");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projects"></param>
        public void ArchiveProjectsProcess(List<BimProject> projects)
        {
            try
            {
                this.ArchiveProjects(projects);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeService"></param>
        /// <param name="bimProjects"></param>
        public void ExportProjectsToCsv(bool includeService, List<BimProject> bimProjects = null)
        {
            if (includeService == true)
            {
                List<BimProject> projWithSvc = new List<BimProject>();
                foreach (BimProject proj in bimProjects)
                {
                    string services = GetProjectServiceTypes(proj.id);
                    proj.service_types = services;
                    projWithSvc.Add(proj);
                }
                CsvExporter.ExportProjectsCsv(projWithSvc);
            }
            else if (includeService == false)
            {
                CsvExporter.ExportProjectsCsv(bimProjects);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BimProject> GetAllProjects()
        {
            return DataController.AllProjects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetIndustryRoles()
        {
            return DataController.GetIndustryRoles();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Hub> GetHubs()
        {
            return DataController.Hubs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projId"></param>
        /// <returns></returns>
        public string GetProjectServiceTypes(string projId)
        {
            BimProject project = DataController.GetProjectWithServiceById(projId);
            string _serviceTypes = "";
            if (project.service_types.Count() > 0)
            {
                _serviceTypes = project.service_types.Replace(',', DefaultConfig.secondDelimiter);
            }
            return _serviceTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateProjects()
        {
            Log.Info("");
            Log.Info("Creating projects...");

            List<BimProject> _projectsToCreate = DataController.GetBimProjects();

            if (_projectsToCreate == null || _projectsToCreate.Count < 1)
            {
                Log.Info("Project creation was unable to start due to insufficient input data");
                return;
            }

            for(int i = 0; i < DataController._projectTable.Rows.Count; i++)
            {
                string name = Convert.ToString(DataController._projectTable.Rows[i]["name"]);
                Log.Info($"- processing row {i + 1} - project name: {name}");

                BimProject project = _projectsToCreate.FirstOrDefault(p => p.name != null && p.name.Equals(name));
                if (project == null || false == CheckRequiredParams(project, DataController._projectTable.Rows[i]))
                {
                    Log.Error($"There's an incomplete input data");
                    DataController._projectTable.Rows[i]["result"] = ResultCodes.IncompleteInputData;
                    continue;
                }

                DataController.AddProject(project, null, i);
            }
            CsvExporter.WriteResults(DataController._projectTable, _options, _options.FilePath);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateProjects()
        {
            Log.Info("");
            Log.Info("Updating projects..");

            List<BimProject> _projectsToCreate = DataController.GetBimProjects();

            for (int i = 0; i < DataController._projectTable.Rows.Count; i++)
            {
                string name = Convert.ToString(DataController._projectTable.Rows[i]["name"]);
                Log.Info($"Processing row {i + 1} - project name: {name}");

                BimProject project = _projectsToCreate.FirstOrDefault(p => p.name != null && p.name.Equals(name));
                if (project == null || false == CheckRequiredParams(project, DataController._projectTable.Rows[i]))
                {
                    Log.Error($"Incomplete input data");
                    DataController._projectTable.Rows[i]["result"] = ResultCodes.IncompleteInputData;
                    continue;
                }

                if (project.id != null)
                {
                    // the dynamic exclusion for project name during serialization
                    BimProject originalProject = DataController.AllProjects.First(p => p.id.Equals(project.id));
                    if (originalProject.name != project.name)
                    {
                        project.include_name_to_request_body = true;
                    }
                    else if (originalProject.name == project.name)
                    {
                        project.include_name_to_request_body = false;
                    }

                    DataController.UpdateProject(project, i);
                }
                else if (project.id == null)
                {
                    Log.Error($"Project ID is not given. Cannot update.");
                }
            }
            CsvExporter.WriteResults(DataController._projectTable, _options, _options.FilePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projects"></param>
        private void ArchiveProjects(List<BimProject> projects)
        {
            Log.Info("");
            Log.Info("Start archiving selected projects.");
            foreach (BimProject project in projects)
            {
                Log.Info($"Processing project name: {project.name}");
                if (project == null || false == CheckRequiredParams(project))
                {
                    Log.Error($"Incomplete input data");
                    continue;
                }
                DataController.ArchiveProject(project);
            }
        }

        /// <summary>
        /// Validates the given input data to prevent insufficient API calls afterwards
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        private bool CheckRequiredParams(BimProject proj)
        {
            bool isNull = string.IsNullOrEmpty(proj.name) || string.IsNullOrEmpty(proj.project_type) || string.IsNullOrEmpty(proj.value)
                || string.IsNullOrEmpty(proj.currency);

            return !isNull;
        }

        /// <summary>
        /// Validates the given input data to prevent insufficient API calls afterwards
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool CheckRequiredParams(BimProject proj, DataRow row)
        {
            bool isNull = string.IsNullOrEmpty(proj.name) || string.IsNullOrEmpty(proj.project_type) || string.IsNullOrEmpty(proj.value)
                || string.IsNullOrEmpty(proj.currency);
            if (isNull)
            {
                string msg = $"One of the required parameters is null or empty. Required are: name, proj_type,value,currency,start_date,end_date";
                row["result"] = "Invalid Parameters";
                row["result_message"] = msg;
                Log.Error(msg);
            }
            return !isNull;
        }
    }
}
