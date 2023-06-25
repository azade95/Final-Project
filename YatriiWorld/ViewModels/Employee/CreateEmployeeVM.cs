using System.ComponentModel.DataAnnotations;

namespace YatriiWorld.ViewModels
{
    public class CreateEmployeeVM
    {

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Surname { get; set; }
        public int PositionId { get; set; }
        public IFormFile Photo { get; set; }
    }
}
