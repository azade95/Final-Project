using Microsoft.AspNetCore.Identity;
using YatriiWorld.Models;

namespace YatriiWorld.ViewModels
{
    public class UserModel
    {
       public AppUser User { get; set; }
       public List<BookedTour> BookedTours { get; set; }
    }
}
