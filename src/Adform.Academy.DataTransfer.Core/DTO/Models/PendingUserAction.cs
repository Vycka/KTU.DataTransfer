using Adform.Academy.DataTransfer.Core.DTO.Types;

namespace Adform.Academy.DataTransfer.Core.DTO.Models
{
    public class PendingUserAction
    {
        public virtual int ProjectId { get; set; }
        public virtual PendingUserActionTypes PendingAction { get; set; }
    }
}
