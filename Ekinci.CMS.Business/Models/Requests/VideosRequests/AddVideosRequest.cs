﻿namespace Ekinci.CMS.Business.Models.Requests.VideosRequests
{
    public class AddVideosRequest
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
    }
}