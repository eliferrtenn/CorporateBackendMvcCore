namespace Ekinci.WebAPI.Business.Models.Responses.ProjectResponse
{
    public class GetProjectResponse
    {
        public int ID { get; set; }
        public long StatusID { get; set; }
        public string StatusName { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public string ProjectDate { get; set; }
        public string DeliveryDate { get; set; }
        public int ApartmentCount { get; set; }
        public int SquareMeter { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public List<ProjectPhotosResponse> ProjectPhotos { get; set; }
    }
    public class ProjectPhotosResponse
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
    }
}