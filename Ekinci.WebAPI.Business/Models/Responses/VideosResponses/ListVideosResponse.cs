namespace Ekinci.WebAPI.Business.Models.Responses.VideosResponse
{
    public class ListVideosResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public string VideoUrl { get; set; }
    }
}