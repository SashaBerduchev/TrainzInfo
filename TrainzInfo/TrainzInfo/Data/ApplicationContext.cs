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
        public DbSet<TrainzType> TrainzTypes { get; set; }
        public DbSet<LocomotivesType> LocomotivesTypes { get; set; }
        public DbSet<CargoCarrieges> CargoCarrieges { get; set; }
        public DbSet<PassangerCarrieges> PassangerCarrieges { get; set; }
        public DbSet<Electic_locomotive> Electic_Locomotives { get; set; }
        public DbSet<Electrick_Lockomotive_Info> Electrick_Lockomotive_Infos { get; set; }
    }
}
