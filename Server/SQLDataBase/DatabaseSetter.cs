using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class MissileLauncher
    {
        public int Id { get; set; }  // Primary Key
        public string? Code { get; set; }  // Unique Identifier
        public string? Location { get; set; }
        public string? MissileType { get; set; }
        public int MissileCount { get; set; }
        public int FailureCount { get; set; }
        public int FixedFailures { get; set; }
    }
}
