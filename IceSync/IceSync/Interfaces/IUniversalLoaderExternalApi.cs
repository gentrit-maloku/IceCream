using IceSync.Models;
using IceSync.Models.Requests;
using IceSync.Models.Responses;
using Refit;

namespace IceSync.Interfaces
{
    public interface IUniversalLoaderExternalApi
    {
        [Post("/v2/authenticate")]
        Task<ApiResponse<AuthenticationResponse>> AuthenticateAsync([Body] AuthenticationRequest request);

        [Get("/workflows")]
        Task<ApiResponse<List<Workflow>>> GetWorkflowsAsync([Header("Authorization")] string bearerToken);

        [Post("/workflows/{workflowId}/run")]
        Task<ApiResponse<string>> RunWorkflowAsync(int workflowId, [Header("Authorization")] string bearerToken);
    }
}
