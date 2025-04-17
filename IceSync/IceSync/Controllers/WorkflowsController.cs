using IceSync.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IceSync.Controllers
{
    public class WorkflowsController(IUniversalLoaderService universalLoader) : Controller
    {
        /// <summary>
        /// Displays the list of available workflows.
        /// </summary>
        /// <returns>A list of workflow data from external Api.</returns>
        public async Task<IActionResult> Index()
        {
            var response = await universalLoader.GetWorkflowsAsync();

            return Ok(response.Value);
        }
    }
}
