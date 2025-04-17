using IceSync.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IceSync.Controllers
{
    public class WorkflowsController(IUniversalLoaderService universalLoader) : Controller
    {
        /// <summary>
        /// Displays the list of available workflows.
        /// </summary>
        /// <returns>A view displaying workflow data.</returns>
        public async Task<IActionResult> Index()
        {
            var response = await universalLoader.GetWorkflowsAsync();

            return View(response.Value);
        }
    }
}
