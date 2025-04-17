using IceSync.Interfaces;
using IceSync.Models;
using Quartz;

namespace IceSync.Jobs
{
    public class WorkflowSyncJob(IUniversalLoaderService universalLoaderService, IWorkflowRepository workflowRepository) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var apiResult = await universalLoaderService.GetWorkflowsAsync();

            if (apiResult.IsFailed)
                return;

            var apiWorkflows = apiResult.Value;
            var dbWorkflowsResult = await workflowRepository.GetAllAsync();

            if (dbWorkflowsResult.IsFailed)
                return;

            var dbWorkflows = dbWorkflowsResult.Value;

            var apiDict = apiWorkflows.ToDictionary(w => w.Id);
            var dbDict = dbWorkflows.ToDictionary(w => w.Id);

            var toAdd = new List<Workflow>();
            var toUpdate = new List<Workflow>();
            var toDelete = new List<Workflow>();

            foreach (var apiWorkflow in apiWorkflows)
            {
                if (dbDict.TryGetValue(apiWorkflow.Id, out var existing))
                {
                    existing.Name = apiWorkflow.Name;
                    existing.IsActive = apiWorkflow.IsActive;
                    existing.MultiExecBehavior = apiWorkflow.MultiExecBehavior;

                    toUpdate.Add(existing);
                }
                else
                {
                    toAdd.Add(apiWorkflow);
                }
            }

            foreach (var dbWorkflow in dbWorkflows)
            {
                if (!apiDict.ContainsKey(dbWorkflow.Id))
                    toDelete.Add(dbWorkflow);
            }

            await workflowRepository.SyncWorkflowsAsync(toAdd, toUpdate, toDelete);
        }
    }
}
