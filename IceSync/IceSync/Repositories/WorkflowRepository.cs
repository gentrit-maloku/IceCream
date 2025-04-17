using FluentResults;
using IceSync.Data;
using IceSync.Interfaces;
using IceSync.Models;
using Microsoft.EntityFrameworkCore;

namespace IceSync.Repositories
{
    public class WorkflowRepository(ApplicationDbContext context) : IWorkflowRepository
    {
        public async Task<Result<List<Workflow>>> GetAllAsync()
        {
            var workflows = await context.Workflows.ToListAsync();

            return Result.Ok(workflows);
        }

        public async Task<Result> SyncWorkflowsAsync(List<Workflow> toAdd, List<Workflow> toUpdate, List<Workflow> toDelete)
        {
            if (toAdd.Count > 0)
                await context.BulkInsertAsync(toAdd);

            if (toUpdate.Count > 0)
                await context.BulkUpdateAsync(toUpdate);

            if (toDelete.Count > 0)
                await context.BulkDeleteAsync(toDelete);

            await context.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
