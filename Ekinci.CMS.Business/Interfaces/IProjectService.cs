using Ekinci.CMS.Business.Models.Requests.ProjectRequests;
using Ekinci.CMS.Business.Models.Responses.ProjectResponses;
using Ekinci.Common.Business;
using Microsoft.AspNetCore.Http;

namespace Ekinci.CMS.Business.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult> AddProject(AddProjectRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl);
        Task<ServiceResult<UpdateProjectRequest>> UpdateProject(int projectID);
        Task<ServiceResult> UpdateProject(UpdateProjectRequest request, IEnumerable<IFormFile> PhotoUrls, IFormFile PhotoUrl);
        Task<ServiceResult<List<ListProjectResponses>>> GetAll();
        Task<ServiceResult<List<ListProjectResponses>>> GetAllProjectStatus(int ProjectStatusID);
        Task<ServiceResult<GetProjectResponse>> GetProject(int projectID);
        Task<ServiceResult> DeleteProjectPhoto(int projectPhotoID);
        Task<ServiceResult> DeletProject(int projecttID);
    }
}