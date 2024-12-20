﻿using Microsoft.EntityFrameworkCore;
using TrainzInfo.Models;
using TrainzInfo.Tools;

namespace TrainzInfo.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Startup.GetConfig() == true)
            {
                optionsBuilder.LogTo(LoggingExceptions.EFLog);
            }
            
        }
        public DbSet<Users> User { get; set; }
        public DbSet<Locomotive> Locomotives { get; set; }
        public DbSet<NewsInfo> NewsInfos { get; set; }
        public DbSet<NewsComments> NewsComments { get; set; }
        public DbSet<UserLocomotivePhotos> UserLocomotivePhotos { get; set; }
        public DbSet<ElectricTrain> Electrics { get; set; }
        public DbSet<UserTrainzPhoto> UserTrainzPhotos { get; set; }
        public DbSet<ElectrickTrainzInformation> ElectrickTrainzInformation { get; set; }
        public DbSet<UkrainsRailways> UkrainsRailways { get; set; }
        public DbSet<DepotList> Depots { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Oblast> Oblasts { get; set; }
        public DbSet<Stations> Stations { get; set; }
        public DbSet<TypeOfPassTrain> TypeOfPassTrains { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<StationsShadule> StationsShadules { get; set; }
        public DbSet<Locomotive_series> Locomotive_Series { get; set; }
        public DbSet<LocomotiveBaseInfo> LocomotiveBaseInfos { get; set; }
        public DbSet<StationInfo> StationInfos { get; set; }
        public DbSet<Plants> Plants { get; set; }
        public DbSet<SuburbanTrainsInfo> SuburbanTrainsInfos { get; set; }
        public DbSet<IpAdresses> IpAdresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RailwayUsersPhoto> RailwayUsersPhotos { get; set; }
        public DbSet<MainImages> MainImages { get; set; }
        public DbSet<Metro> Metros { get; set; }
        public DbSet<MetroStation> MetroStations { get; set; }
        public DbSet<MetroLines> MetroLines { get; set; }
        public DbSet<TrainsShadule> TrainsShadule { get; set; }
        public DbSet<DieselTrains> DieselTrains { get; set; }

    }

}
