namespace YatriiWorld.ViewModels
{
    public class CreateSlideVM
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int Order { get; set; }
        public IFormFile Photo { get; set; }
    }
}
