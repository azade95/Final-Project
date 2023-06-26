using AutoMapper;
using YatriiWorld.Models;
using YatriiWorld.ViewModels;

namespace YatriiWorld.Services
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<AppUser, RegisterVM>();
            CreateMap<RegisterVM, AppUser>();
            CreateMap<Slide, CreateSlideVM>();
            CreateMap<CreateSlideVM, Slide>();
            CreateMap<Slide, UpdateSlideVM>();
            CreateMap<UpdateSlideVM, Slide>();
            CreateMap<Tour,CreateTourVM>();
            CreateMap<CreateTourVM,Tour>();
        }
    }
}
