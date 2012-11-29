﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Extensions;
using DotSpatial.Projections;
using HydroDesktop.Common;
using HydroDesktop.Configuration;
using HydroDesktop.Help;

namespace HydroDesktop.Main
{
    /// <summary>
    /// The welcome screen form shown on program startup
    /// </summary>
    public partial class WelcomeScreen : Form
    {
        /// <summary>
        /// Gets the list tools available.
        /// </summary>
        public IEnumerable<ISampleProject> SampleProjects { get; set; }
        
        #region Private Variables
        
        private List<ProjectFileInfo> _recentProjectFiles;
        private AppManager _app;
        private bool _newProjectCreated = false;

        private Extent _defaultMapExtent = new Extent(-170, -50, 170, 50);

        private ProjectManager myProjectManager;

        
        #endregion

        #region Constructor

        public WelcomeScreen(ProjectManager projManager)
        {
            InitializeComponent();

            myProjectManager = projManager;
            lblProductVersion.Text = "CUAHSI HydroDesktop " + AppContext.Instance.ProductVersion;
            
            _app = projManager.App;
            _recentProjectFiles = new List<ProjectFileInfo>();
            bsRecentFiles = new BindingSource(RecentProjectFiles, null);
            lstRecentProjects.DataSource = bsRecentFiles;
            lstRecentProjects.DoubleClick += lstRecentProjects_DoubleClick;

            lstProjectTemplates.DoubleClick += lstProjectTemplates_DoubleClick;

            if (lstProjectTemplates.Items.Count > 0)
            {
                lstProjectTemplates.SelectedIndex = 0;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// The list of recent project files
        /// </summary>
        public List<ProjectFileInfo> RecentProjectFiles
        {
            get { return _recentProjectFiles; }
        }

        /// <summary>
        /// Returns true, if a new project was created
        /// using template or using empty project
        /// </summary>
        public bool NewProjectCreated 
        {
            get { return _newProjectCreated; }
        }
        #endregion

        #region Methods

        private void CreateProjectFromTemplate()
        {
            Map mainMap = _app.Map as Map;
            if (mainMap != null)
            {
                if (lstProjectTemplates.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select a project template.");
                    DialogResult = DialogResult.None;
                    return;
                }
                var selectedTemplate = lstProjectTemplates.SelectedItem as ISampleProject;
                string projectFile = selectedTemplate.AbsolutePathToProjectFile;

                try
                {
                    string newProjectFile = CopyToDocumentsFolder(projectFile);
                    _app.SerializationManager.OpenProject(newProjectFile);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + @" File: " + projectFile);
                }
                    
                    
                lblProgress.Text = "Creating new Project.. ";
                this.Cursor = Cursors.WaitCursor;
                    
                panelStatus.Visible = true;

                _newProjectCreated = true;

                this.DialogResult = DialogResult.OK;
                    
                this.Close();
            }
        }

        private string CopyToDocumentsFolder(string projectFile)
        {
            string projDir = Path.GetDirectoryName(projectFile);
            string docsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string dotSpatialDir = Path.Combine(docsDir, "DotSpatial");
            if (!Directory.Exists(dotSpatialDir))
            {
                //todo check if the directory can be created
                Directory.CreateDirectory(dotSpatialDir);
            }

            string projName = Path.GetFileNameWithoutExtension(projectFile);
            string newProjDir = Path.Combine(dotSpatialDir, projName);
            if (!Directory.Exists(newProjDir))
            {
                Directory.CreateDirectory(newProjDir);
            }

            foreach (string file in Directory.GetFiles(projDir))
            {
                File.Copy(file, Path.Combine(newProjDir, Path.GetFileName(file)), true);
            }
            string newProjFile = Path.Combine(newProjDir, Path.GetFileName(projectFile));
            return newProjFile;
        }

        /// <summary>
        /// Creates a new empty project
        /// </summary>
        private void CreateEmptyProject()
        {
            panelStatus.Visible = true;
            myProjectManager.CreateEmptyProject();
            

            SetDefaultMapExtents();

            this.Cursor = Cursors.Default;

            _newProjectCreated = true;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void ReportProgress(int percent, string message)
        {
            //if (!progressBar1.Visible) progressBar1.Visible = true;
            //progressBar1.Value = percent;
            //lblProgress.Text = message;
        }

        #endregion

        #region Event Handlers

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbNewProjectTemplate.Checked)
            {
                CreateProjectFromTemplate();
            }
            else if (rbEmptyProject.Checked)
            {
                CreateEmptyProject();
            }
            else
            {
                OpenProject();
            }
            panelStatus.Visible = true;
            //_app.ProgressHandler.
        }

        private void WelcomeScreen_Load(object sender, EventArgs e)
        {
            SampleProjectInstaller spi = new SampleProjectInstaller();
            List<SampleProjectInfo> sampleProjects1 = spi.FindSampleProjectFiles();
            IEnumerable<ISampleProject> sampleProjects2 = spi.SetupInstalledSampleProjects(sampleProjects1);

            SampleProjects = sampleProjects2;

            lstProjectTemplates.DataSource = SampleProjects;
            lstProjectTemplates.DisplayMember = "Name";

            FindRecentProjectFiles();
        }

        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FeatureSet fs = new FeatureSet();
            //fs.Features[0].Coordinates[0].X
        }

        private void btnBrowseProject_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "HydroDesktop Project File|*.dspx";
            fileDialog.Title = "Select the Project File to Open";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenExistingProject(fileDialog.FileName);
            }
        }

        void lstRecentProjects_DoubleClick(object sender, EventArgs e)
        {
            OpenProject();
        }

        void lstProjectTemplates_DoubleClick(object sender, EventArgs e)
        {
            CreateProjectFromTemplate();
        }

        #endregion

        #region Methods

        private void OpenProject()
        {
            ProjectFileInfo selected = lstRecentProjects.SelectedValue as ProjectFileInfo;
            if (selected != null)
            {
                panelStatus.Visible = true;
                OpenExistingProject(selected.FullPath);
            }
        }

        private void OpenExistingProject(string projectFileName)
        {
            lblProgress.Text = "Opening Project " + Path.GetFileNameWithoutExtension(projectFileName) + "...";
            this.Cursor = Cursors.WaitCursor;

            myProjectManager.OpenProject(projectFileName);

            this.Cursor = Cursors.Default;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SetDefaultMapExtents()
        {
            double[] xy = new double[4];
            xy[0] = _defaultMapExtent.MinX;
            xy[1] = _defaultMapExtent.MinY;
            xy[2] = _defaultMapExtent.MaxX;
            xy[3] = _defaultMapExtent.MaxY;
            double[] z = new double[] { 0, 0 };
            ProjectionInfo wgs84 = KnownCoordinateSystems.Geographic.World.WGS1984;
            Reproject.ReprojectPoints(xy, z, wgs84, _app.Map.Projection, 0, 2);

            _app.Map.ViewExtents = new Extent(xy);
        }
        
        private void FindRecentProjectFiles()
        {
            this.RecentProjectFiles.Clear();

            List<string> existingRecentFiles = new List<string>();
                
            foreach (string recentFile in Settings.Instance.RecentProjectFiles)
            {              
                if (File.Exists(recentFile))
                {
                    if (!existingRecentFiles.Contains(recentFile)) //add to list only if not exists
                    {
                        existingRecentFiles.Add(recentFile);
                    }
                }
            }

            Settings.Instance.RecentProjectFiles.Clear();
            foreach (string recentFile in existingRecentFiles)
            {
                Settings.Instance.RecentProjectFiles.Add(recentFile);
                RecentProjectFiles.Add(new ProjectFileInfo(recentFile));
            }

            //also adds the installed 'sample projects' to the directory
            //SetupSampleProjects();

            bsRecentFiles.ResetBindings(false);
            lstRecentProjects.SelectedIndex = -1;
        }




        #endregion

        private void linkLabelQuickStart_click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string quickStartGuideFile = Properties.Settings.Default.QuickStartGuideName;
            LocalHelp.OpenHelpFile(quickStartGuideFile);
        }

        private void linkLabelHelp_click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LocalHelp.OpenHelpFile("welcome.html");
        }

        private void lstRecentProjects_Click(object sender, EventArgs e)
        {
            rbOpenExistingProject.Checked = true;
        }      
    } 

    public class ProjectFileInfo
    {
        public ProjectFileInfo(string fullPath)
        {
            FullPath = fullPath;
        }

        public string FullPath { get; private set; }
        public string Name 
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FullPath);

            }
        }
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectFileInfo);
        }

        public bool Equals(ProjectFileInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
