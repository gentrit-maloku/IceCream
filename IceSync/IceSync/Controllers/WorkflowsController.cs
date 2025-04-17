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

        /// <summary>
        /// Executes the specified workflow by its ID.
        /// </summary>
        /// <param name="id">The ID of the workflow to run.</param>
        /// <returns>
        /// Returns HTTP 200 OK with `true` if successful, otherwise HTTP 400 Bad Request with the first error message.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Run(int id)
        {
            var result = await universalLoader.RunWorkflowAsync(id);

            if (result.IsSuccess)
                return Ok(true);

            return BadRequest(result.Errors.First().Message);
        }
    }
}
