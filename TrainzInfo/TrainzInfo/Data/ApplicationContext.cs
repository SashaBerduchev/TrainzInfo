using Microsoft.EntityFrameworkCore;
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
        public DbSet<TrainzType> TrainzTypes { get; set; }
        public DbSet<LocomotivesType> LocomotivesTypes { get; set; }
        public DbSet<CargoCarrieges> CargoCarrieges { get; set; }
        public DbSet<Electic_locomotive> Electic_Locomotives { get; set; }
        public DbSet<Electrick_Lockomotive_Info> Electrick_Lockomotive_Infos { get; set; }
        public DbSet<DieselLocomoives> DieselLocomoives { get; set; }
        public DbSet<DieselLocomotiveInfo> DieselLocomotiveInfos { get; set; }
        public DbSet<PassangerCarriere> PassangerCarrieres { get; set; }
        public DbSet<CargoCarriegesInfo> CargoCarriegesInfos { get; set; }
        public DbSet<PassangerCarriegesInfo> PassangerCarriegesInfos { get; set; }
        public DbSet<NewsInfo> NewsInfos { get; set; }
        public DbSet<ListRollingStone> ListRollingStones {get;set;}
        public DbSet<NewsComments> NewsComments { get; set; }
        public DbSet<UserLocomotivePhotos> UserLocomotivePhotos { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ElectricTrain> Electrics { get; set; }
        public DbSet<Diesel_trainz> Diesel_Trinzs { get; set; }
        public DbSet<ElectrickTrainsList> ElectrickTrainsList { get; set; }
        public DbSet<UserTrainzPhoto> UserTrainzPhotos { get; set; }
        public DbSet<ElectrickTrainzInformation> ElectrickTrainzInformation { get; set; }
        public DbSet<DieselTrainzList> DieselTrainzLists { get; set; }
        public DbSet<UkrainsRailways> UkrainsRailways { get; set; }
        public DbSet<DepotList> Depots { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Oblast> Oblasts { get; set; }
        public DbSet<Stations> Stations { get; set; }
        public DbSet<TypeOfPassTrain> TypeOfPassTrains { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<StationsShadule> StationsShadules { get; set; }
        public DbSet<Diesel_train_info> Diesel_Train_Infos { get; set; }
        public DbSet<Locomotive_series> Locomotive_Series { get; set; }
        public DbSet<LocomotiveBaseInfo> locomotiveBaseInfos { get; set; }
        public DbSet<StationInfo> stationInfos { get; set; }
        public DbSet<Plants> plants { get; set; }
        public DbSet<SuburbanTrainsInfo> SuburbanTrainsInfos { get; set; }
        public DbSet<IpAdresses> IpAdresses { get; set; }
    }
}
