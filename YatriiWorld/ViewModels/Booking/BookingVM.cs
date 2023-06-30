namespace YatriiWorld.ViewModels
{
    public class BookingVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PeopleCount { get; set; }
        public BookedTourVM BookedTour { get; set; }
    }
}
