using System.Collections.Generic;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.NHibernate;
using Adform.Academy.DataTransfer.Core.DTO.Types;
using Adform.Academy.DataTransfer.Logger;
using Adform.Academy.DataTransfer.Logger.Events;
using NHibernate.Criterion;

namespace Adform.Academy.DataTransfer.Core.DataTransfer
{
    public class DataTransferServiceRunner
    {
        private readonly ILogger _logger;

        private Dictionary<int, ProjectRunner> _activeProjects = new Dictionary<int, ProjectRunner>(); 

        public DataTransferServiceRunner(ILogger logger)
        {
            _logger = logger;
            _activeProjects = new Dictionary<int, ProjectRunner>();
            _logger.Log(new LogEvent("Execution service created"));      
        }

        public void StartService()
        {
            _logger.Log(new LogEvent("Execution service starting..."));
            using (var session = SessionFactory.OpenSession())
            {
                var projectList =
                    session.CreateCriteria(typeof (Project))
                    .Add(Restrictions.Eq("ProjectState", ProjectStateTypes.Running)
                ).List<Project>();

                foreach (var project in projectList)
                {
                    StartProject(project.ProjectId);
                }
            }
            _logger.Log(new LogEvent("Execution service started")); 
        }

        public void StopService()
        {
            _logger.Log(new LogEvent("Execution service stopping..."));

            foreach (var runningProject in _activeProjects.Values)
            {
                if (runningProject.IsRunning)
                    runningProject.StopExecution(CancelType.ShutDown);
            }

            _logger.Log(new LogEvent("Execution service stopped")); 
        }

        public void StartProject(int projectId)
        {
            StopProjectSilent(projectId);

            var projectRunner = new ProjectRunner(projectId, _logger);
            projectRunner.StartAsync();
            _activeProjects.Add(projectId, projectRunner);
        }

        private void StopProjectSilent(int projectId)
        {
            if (_activeProjects.ContainsKey(projectId))
            {
                var project = _activeProjects[projectId];
                if (project.IsRunning)
                    project.StopExecution(CancelType.ShutDown);

                _activeProjects.Remove(projectId);
            }
        }
    }
}
