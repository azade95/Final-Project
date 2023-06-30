using YatriiWorld.Models;

namespace YatriiWorld.ViewModels
{
    public class BookedTourVM
    {
        public decimal Price { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        

    }
}
