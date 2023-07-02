using System.ComponentModel.DataAnnotations;

namespace YatriiWorld.ViewModels
{
    public class UserVM
    {
        public string Name { get; set; }
        
        public string Surname { get; set; }
       
        public string Username { get; set; }
      
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public string Gender { get; set; }
       
    }
}
