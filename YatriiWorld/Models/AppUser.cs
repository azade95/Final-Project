﻿using Microsoft.AspNetCore.Identity;

namespace YatriiWorld.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Gender { get; set; }
        public bool IsReminded { get; set; }
    }
}
