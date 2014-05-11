using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using Adform.Academy.DataTransfer.Core.DataTransfer.Actions;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.Core.DTO.Types;
using Adform.Academy.DataTransfer.Logger;
using Adform.Academy.DataTransfer.Logger.Events;

namespace Adform.Academy.DataTransfer.Core.DataTransfer
{
    public class ProjectRunner
    {
        private readonly int _projectId;
        private readonly ILogger _logger;
        private Thread _runnerThread;
        private CancelType _stopType = CancelType.ShutDown;

        private ExecutingProjectData _projectData;
        private int _executingTaskPosition;

        private readonly List<IAction> _executingTasksList = new List<IAction>
        {
            new FullAnalyze(),
            new CreateTables(),
            new CopyData(),
            new AppendAnalyze(),
            new Verify()
        };

        public bool CancelationPending { get; private set; }

        public bool IsRunning
        {
            get { return (_runnerThread != null && _runnerThread.ThreadState == ThreadState.Running); }
        }

        public ProjectRunner(int projectId, ILogger logger)
        {
            _projectId = projectId;
            _logger = logger;

            CancelationPending = false;
        }

        public void StartAsync()
        {
            if (_runnerThread != null)
                throw new InvalidOperationException("Same ProjectRunner cannot be ran twice");

            _runnerThread = new Thread(ExecuteProject);
            _runnerThread.Start();
        }

        private void ExecuteProject()
        {
            try
            {
                _projectData = new ExecutingProjectData();
                _projectData.Logger = _logger;
                _projectData.Session = SessionFactory.OpenSession();
                _projectData.Project = _projectData.Session.Load<Project>(_projectId);
                _projectData.SrcConnection = new SqlConnection(BuildConnectionString(_projectData.Project.DatabaseSource));
                _projectData.DesConnection = new SqlConnection(BuildConnectionString(_projectData.Project.DatabaseDestination));
                _projectData.SrcConnection.Open();
                _projectData.DesConnection.Open();
                _projectData.ProjectRunner = this;

                _projectData.Project.ProjectState = ProjectStateTypes.Running;
                _projectData.Session.Merge(_projectData.Project);
                _projectData.Session.Flush();

                while (
                    CancelationPending == false &&
                    _projectData.Project.ExecutionState != ExecutionStepsTypes.Completed && 
                    _projectData.Project.ProjectState != ProjectStateTypes.Error
                )
                {
                    ExecuteNextTask();
                }

                if (_projectData.Project.ExecutionState == ExecutionStepsTypes.Completed)
                    _projectData.Project.ProjectState = ProjectStateTypes.Stopped;

                if (CancelationPending)
                {
                    switch (_stopType)
                    {
                        case CancelType.Pause:
                            _projectData.Project.ProjectState = ProjectStateTypes.Paused;
                            break;
                        case CancelType.Stop:
                            _projectData.Project.ProjectState = ProjectStateTypes.Stopped;
                            _projectData.Project.ExecutionState = ExecutionStepsTypes.Canceled;
                            break;
                    }
                }
                _projectData.Session.Merge(_projectData.Project);
                _projectData.Session.Flush();
            }
            catch (Exception ex)
            {
                int? projectId = _projectData.Project != null ? (int?)_projectData.Project.ProjectId : null;
                _logger.LogError(new LogErrorEvent(ex, projectId));

                if (_projectData != null && _projectData.Project != null)
                {
                    _projectData.Project.ProjectState = ProjectStateTypes.Error;
                    _projectData.Session.Merge(_projectData.Project);
                    _projectData.Session.Flush();
                }
            }

        }

        private void ExecuteNextTask()
        {
            IAction executingAction = _executingTasksList[_executingTaskPosition];

            if (executingAction.ValidateStepExecution(_projectData.Project))
                executingAction.ExecuteAction(_projectData);

            _executingTaskPosition = (_executingTaskPosition + 1) % _executingTasksList.Count;
        }


        private string BuildConnectionString(Database database)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.DataSource = database.Host;
            connectionStringBuilder.InitialCatalog = database.DatabaseName;
            connectionStringBuilder.UserID = database.UserName;
            connectionStringBuilder.Password = database.Password;

            return connectionStringBuilder.ConnectionString;
        }

        public void StopExecution(CancelType cancelType)
        {
            _stopType = cancelType;
            CancelationPending = true;
            while (IsRunning)
            {
                Thread.Sleep(100);
            }
        }
    }

    public enum CancelType
    {
        Pause,
        Stop,
        ShutDown
    }
}
