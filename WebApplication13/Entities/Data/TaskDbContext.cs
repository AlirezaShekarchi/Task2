using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication13.Entities.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
          : base(options)
        {
        }
        public DbSet<User> Users { get; set; } 
        public DbSet<HighTempCity> highTempCities { get; set; }
    }
}
