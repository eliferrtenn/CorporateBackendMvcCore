﻿namespace Ekinci.WebAPI.Business.Models.Responses.AnnouncementResponses
{
    public class ListAnnouncementsResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEnabled { get; set; }
    }
}