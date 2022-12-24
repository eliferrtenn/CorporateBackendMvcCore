﻿namespace Ekinci.WebAPI.Business.Models.Responses.AnnouncementResponses
{
    public class GetAnnouncementResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AnnouncementDate { get; set; }
        public List<AnnouncementResponse> AnnouncementPhotos { get; set; }
    }
    public class AnnouncementResponse
    {
        public int ID { get; set; }
        public string PhotoUrl { get; set; }
    }
}