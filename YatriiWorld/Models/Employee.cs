﻿namespace YatriiWorld.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
