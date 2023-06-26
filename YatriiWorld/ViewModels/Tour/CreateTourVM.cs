namespace YatriiWorld.ViewModels
{
    public class CreateTourVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public double Rating { get; set; }  
        public int VisitingPlaces { get; set; }
        public string Destination { get; set; }
        public int Day { get; set; }
        public int Night { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
    }
}
