namespace YatriiWorld.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public double Point { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

    }
}
