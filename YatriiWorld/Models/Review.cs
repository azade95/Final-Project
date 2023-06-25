namespace YatriiWorld.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; } 
        
        

    }
}
