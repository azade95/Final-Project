using Microsoft.AspNetCore.Identity;

namespace YatriiWorld.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public bool IsReminded { get; set; }
        public List<Review> Reviews { get; set; }
        public Rating Rating { get; set; }
        public BookedTour BookedTour { get; set; }
    }
}
