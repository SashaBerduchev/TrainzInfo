using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainzInfo.Migrations
{
    public partial class Startup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargoCarrieges",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarriegeType = table.Column<string>(nullable: false),
                    MaxSpeed = table.Column<int>(nullable: false),
                    CargoType = table.Column<string>(nullable: false),
                    CargoWeight = table.Column<int>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoCarrieges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CargoCarriegesInfos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false),
                    Info = table.Column<string>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoCarriegesInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(nullable: false),
                    Link = table.Column<string>(nullable: false),
                    IsUpdate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Depots",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    UkrainsRailways = table.Column<string>(nullable: false),
                    Addres = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depots", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Diesel_Train_Infos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    AllInfo = table.Column<string>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diesel_Train_Infos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Diesel_Trinzs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    VagonCount = table.Column<int>(nullable: false),
                    Power = table.Column<int>(nullable: false),
                    ImgSrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diesel_Trinzs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DieselLocomoives",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    MaxSpeed = table.Column<int>(nullable: false),
                    SectionCount = table.Column<int>(nullable: false),
                    DiseslPower = table.Column<int>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieselLocomoives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DieselLocomotiveInfos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Diesel_Type = table.Column<string>(nullable: true),
                    Power = table.Column<int>(nullable: false),
                    Baseinfo = table.Column<string>(nullable: true),
                    AllInfo = table.Column<string>(nullable: true),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieselLocomotiveInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DieselTrainzLists",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    NumberTrain = table.Column<int>(nullable: false),
                    Depo = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieselTrainzLists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Electic_Locomotives",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Speed = table.Column<int>(nullable: false),
                    SectionCount = table.Column<int>(nullable: false),
                    ALlPowerP = table.Column<int>(nullable: false),
                    LocomotiveImg = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Electic_Locomotives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Electrick_Lockomotive_Infos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Electric_Type = table.Column<string>(nullable: true),
                    Power = table.Column<int>(nullable: false),
                    Baseinfo = table.Column<string>(nullable: true),
                    AllInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Electrick_Lockomotive_Infos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ElectrickTrainsList",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    NumberTrain = table.Column<int>(nullable: false),
                    Depo = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectrickTrainsList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ElectrickTrainzInformation",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    AllInformation = table.Column<string>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectrickTrainzInformation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Electrics",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    VagonsCountP = table.Column<string>(nullable: false),
                    MaxSpeed = table.Column<int>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Electrics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ListRollingStones",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Depot = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListRollingStones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LocomotivesTypes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocomotivesTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NewsComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsInfos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameNews = table.Column<string>(nullable: false),
                    BaseNewsInfo = table.Column<string>(nullable: false),
                    NewsInfoAll = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false),
                    user = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Oblasts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    OblCenter = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oblasts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PassangerCarriegesInfos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false),
                    Info = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassangerCarriegesInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PassangerCarrieres",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calss = table.Column<string>(nullable: false),
                    CountPlace = table.Column<int>(nullable: false),
                    PlaceType = table.Column<string>(nullable: false),
                    ImgsrcOutside = table.Column<string>(nullable: false),
                    ImgsrcInside = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassangerCarrieres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    Railway = table.Column<string>(nullable: false),
                    Oblast = table.Column<string>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StationsShadules",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Station = table.Column<string>(nullable: false),
                    UzFilia = table.Column<string>(nullable: false),
                    TimeOfArrive = table.Column<DateTime>(nullable: false),
                    TimeOfDepet = table.Column<DateTime>(nullable: false),
                    TrainInfo = table.Column<string>(nullable: false),
                    ImgTrain = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationsShadules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status_namr = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationFrom = table.Column<string>(nullable: false),
                    StationTo = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    NameOfTrain = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TrainzTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainzTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfPassTrains",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfPassTrains", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UkrainsRailways",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Information = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UkrainsRailways", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserLocomotivePhotos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameLocomotive = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    UserSername = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    BaseInfo = table.Column<string>(nullable: false),
                    AllInfo = table.Column<string>(nullable: false),
                    PhotoLink = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocomotivePhotos", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 50, nullable: false),
            //        Age = table.Column<int>(nullable: false),
            //        Email = table.Column<string>(nullable: false),
            //        Password = table.Column<string>(nullable: false),
            //        Status = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Id);
            //    });

            migrationBuilder.CreateTable(
                name: "UserTrainzPhotos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    LocmotiveName = table.Column<string>(nullable: true),
                    Marshrute = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    BaseInfo = table.Column<string>(nullable: false),
                    Imgsrc = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainzPhotos", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargoCarrieges");

            migrationBuilder.DropTable(
                name: "CargoCarriegesInfos");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "Diesel_Train_Infos");

            migrationBuilder.DropTable(
                name: "Diesel_Trinzs");

            migrationBuilder.DropTable(
                name: "DieselLocomoives");

            migrationBuilder.DropTable(
                name: "DieselLocomotiveInfos");

            migrationBuilder.DropTable(
                name: "DieselTrainzLists");

            migrationBuilder.DropTable(
                name: "Electic_Locomotives");

            migrationBuilder.DropTable(
                name: "Electrick_Lockomotive_Infos");

            migrationBuilder.DropTable(
                name: "ElectrickTrainsList");

            migrationBuilder.DropTable(
                name: "ElectrickTrainzInformation");

            migrationBuilder.DropTable(
                name: "Electrics");

            migrationBuilder.DropTable(
                name: "ListRollingStones");

            migrationBuilder.DropTable(
                name: "LocomotivesTypes");

            migrationBuilder.DropTable(
                name: "NewsComments");

            migrationBuilder.DropTable(
                name: "NewsInfos");

            migrationBuilder.DropTable(
                name: "Oblasts");

            migrationBuilder.DropTable(
                name: "PassangerCarriegesInfos");

            migrationBuilder.DropTable(
                name: "PassangerCarrieres");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "StationsShadules");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Trains");

            migrationBuilder.DropTable(
                name: "TrainzTypes");

            migrationBuilder.DropTable(
                name: "TypeOfPassTrains");

            migrationBuilder.DropTable(
                name: "UkrainsRailways");

            migrationBuilder.DropTable(
                name: "UserLocomotivePhotos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTrainzPhotos");
        }
    }
}
