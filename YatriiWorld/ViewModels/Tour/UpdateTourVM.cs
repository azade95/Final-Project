using System.ComponentModel.DataAnnotations;

namespace YatriiWorld.ViewModels
{
    public class UpdateTourVM
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public double Rating { get; set; }
        public int VisitingPlaces { get; set; }
        [Required]
        public string Destination { get; set; }
        public int Day { get; set; }
        public int Night { get; set; }
        [Required]
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
    }
}
