using FluentResults;
using IceSync.Models;

namespace IceSync.Interfaces
{
    public interface IUniversalLoaderService
    {
        Task<Result<List<Workflow>>> GetWorkflowsAsync();

        Task<Result<bool>> RunWorkflowAsync(int workflowId);
    }
}
