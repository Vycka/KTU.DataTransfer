namespace Adform.Academy.DataTransfer.WebApi.Contracts.Projects.Types
{
    //WARNING: This enum is also defined in CORE project
    public enum ExecutionStepsTypes
    {
        NotStarted = 0,
        FullAnalyze = 1,
        AppendAnalyze = 2,
        Copy = 3,
        Verify = 4,
        Delete = 5,
        Completed = 6,
        Canceled = 7
    }
}
