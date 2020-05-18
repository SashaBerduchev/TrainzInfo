using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TrainzInfo.Models;

namespace TrainzInfo.Data
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
            Trace.WriteLine(this);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TrainzType> trainzTypes { get; set; }
    }
}
