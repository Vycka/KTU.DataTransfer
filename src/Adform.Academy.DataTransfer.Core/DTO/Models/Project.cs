using System;
using System.Collections;
using System.Collections.Generic;
using Adform.Academy.DataTransfer.Core.DTO.Types;

namespace Adform.Academy.DataTransfer.Core.DTO.Models
{
    public class Project : IEquatable<Project>
    {
        public virtual int ProjectId { get; set; }
        public virtual Database DatabaseSource { get; set; }
        public virtual Database DatabaseDestination { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual ProjectStateTypes ProjectState { get; set; }
        public virtual ExecutionStepsTypes ExecutionState { get; set; }
        //public virtual IList<ExecutionStep> ExecutionSteps { get; set; }

        //public virtual PendingUserAction PendingUserAction { get; set; }
        public virtual IList<Filter> Filters { get; set; }
        public virtual IList<Log> Logs { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Equals(Project other)
        {
            return ProjectId == other.ProjectId;
        }
    }
}
