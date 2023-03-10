namespace Ekinci.CMS.Business.Models.Responses.IntroResponses
{
    public class GetIntroResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public double SquareMeter { get; set; }
        public int YearCount { get; set; }
        public double CommercialAreaCount { get; set; }
    }
}