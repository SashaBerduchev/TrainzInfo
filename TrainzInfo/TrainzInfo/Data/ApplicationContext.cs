﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using TrainzInfo.Models;

namespace TrainzInfo.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            string trace = "SERVER START!!";
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
            Trace.WriteLine(this);
            Trace.WriteLine(trace);
            try
            {
                FileStream fileStreamLog = new FileStream(@"Trace.log", FileMode.Append);
                for (int i = 0; i < trace.Length; i++)
                {
                    byte[] array = Encoding.Default.GetBytes(trace.ToString());
                    fileStreamLog.Write(array, 0, array.Length);
                }

                fileStreamLog.Close();
            }catch(Exception exp)
            {
                Trace.WriteLine(exp.ToString());
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
        public DbSet<LocomotiveBaseInfo> locomotiveBaseInfos { get; set; }
        public DbSet<StationInfo> stationInfos { get; set; }
        public DbSet<Plants> plants { get; set; }
        public DbSet<SuburbanTrainsInfo> SuburbanTrainsInfos { get; set; }
        public DbSet<IpAdresses> IpAdresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RailwayUsersPhoto> RailwayUsersPhotos { get; set; }
        public DbSet<MainImages> MainImages { get; set; }
        public DbSet<Metro> Metros { get; set; }
        public DbSet<MetroStation> MetroStations { get; set; }
        public DbSet<MetroLines> MetroLines { get; set; }
        public DbSet<TrainzInfo.Models.TrainsShadule> TrainsShadule { get; set; }
        public DbSet<TrainzInfo.Models.DieselTrains> DieselTrains { get; set; }

    }

}
