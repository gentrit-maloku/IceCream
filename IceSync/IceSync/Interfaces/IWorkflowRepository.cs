using FluentResults;
using IceSync.Models;

namespace IceSync.Interfaces
{
    public interface IWorkflowRepository
    {
        Task<Result<List<Workflow>>> GetAllAsync();

        Task<Result> SyncWorkflowsAsync(
            List<Workflow> toAdd,
            List<Workflow> toUpdate,
            List<Workflow> toDelete);
    }
}
