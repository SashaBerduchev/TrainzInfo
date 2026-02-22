using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfoModel.Models.Information.Main;
using TrainzInfoModel.Models.System;
using TrainzInfoModel.Models.Trains;

namespace TrainzInfo.Tools.BackgroundServices
{

    public class SearchIndexingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public SearchIndexingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Init(nameof(SearchIndexingService), nameof(ExecuteAsync));
            Log.Wright("SearchIndexingService start.");
            while (!stoppingToken.IsCancellationRequested)
            {
                // 1. Ждем "таймаут" (например, 5 минут)
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                Log.Wright("SearchIndexingService triggered.");
                try
                {
                    Log.Wright("SearchIndexingService: Starting indexing process.");
                    // 2. Создаем область видимости для работы с БД
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                        // 3. Выполняем логику индексации
                        await SyncIndexAsync(context);
                        await UpdateTables(context);
                    }
                    Log.Wright("SearchIndexingService: Indexing process completed successfully.");
                }
                catch (Exception ex)
                {
                    Log.Wright("SearchIndexingService: An error occurred during the indexing process.");
                    Log.Exceptions($"SearchIndexingService: Error during indexing process - {ex.ToString()}");
                }
                finally
                {
                    Log.Wright("SearchIndexingService: Waiting for the next trigger.");
                }
            }
        }

        private async Task UpdateTables(ApplicationContext context)
        {
            List<Train> trains = await context.Trains
                .Include(t => t.From)
                .Include(t => t.To)
                .ToListAsync();
            foreach (var train in trains)
            {
                if(train.From is null)
                {
                    if(train.StationFrom == "Київ")
                    {
                        train.StationFrom = "Київ-Пасажирський";
                    }
                    Stations stationFrom = await context.Stations.Where(s => s.Name == train.StationFrom).FirstOrDefaultAsync();
                    Stations stationTo = await context.Stations.Where(s => s.Name == train.StationTo).FirstOrDefaultAsync();
                    if (stationFrom != null)
                    {
                        train.From = stationFrom;
                    }
                    if (stationTo != null)
                    {
                        train.To = stationTo;
                    }
                    context.Trains.Update(train);
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task SyncIndexAsync(ApplicationContext context)
        {

            await IndexLoco(context);
            await IndexStations(context);
            await IndexDieselTrains(context);
            await ElectricTrains(context);
            await Trains(context);
            await TrainsSheduller(context);
            await StationsSheduller(context);
            await News(context);
        }

        private async Task News(ApplicationContext context)
        {
            var indexedIds = await context.DocumentToIndex
           .Include(d => d.NewsInfo)
           .Where(d => d.NewsInfo != null)
           .Select(d => d.NewsInfo.id)
           .ToListAsync();

            var newIndex = await context.NewsInfos
            .Where(l => !indexedIds.Contains(l.id))
            .Take(500) // Берем пачками по 500 штук, чтобы не грузить память
            .ToListAsync();

            foreach (var item in newIndex)
            {
                if (item.ObjectName == null)
                {
                    item.ObjectName = $"{nameof(NewsInfo).ToUpper()} - {item.id}";
                }
                var doc = new DocumentToIndex
                {
                    NameObject = item.ObjectName,
                    Path = nameof(NewsInfo),
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    NewsInfo = item // Привязываем по ID (или объектом)
                };
                context.DocumentToIndex.Add(doc);
            }

            if (newIndex.Any())
            {
                await context.SaveChangesAsync();
            }

        }

        private async Task StationsSheduller(ApplicationContext context)
        {
            var indexedIds = await context.DocumentToIndex
            .Include(d => d.StationsShadule)
            .Where(d => d.StationsShadule != null)
            .Select(d => d.StationsShadule.id)
            .ToListAsync();

            var newIndex = await context.StationsShadules
            .Where(l => !indexedIds.Contains(l.id))
            .Take(500) // Берем пачками по 100 штук, чтобы не грузить память
            .ToListAsync();

            foreach (var item in newIndex)
            {
                if (item.ObjectName == null)
                {
                    item.ObjectName = $"{nameof(StationsShadule).ToUpper()} - {item.id}";
                }
                var doc = new DocumentToIndex
                {
                    NameObject = item.ObjectName,
                    Path = nameof(StationsShadule),
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    StationsShadule = item // Привязываем по ID (или объектом)
                };
                context.DocumentToIndex.Add(doc);
            }

            if (newIndex.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        private async Task TrainsSheduller(ApplicationContext context)
        {
            var indexedIds = await context.DocumentToIndex
            .Include(d => d.TrainsShadule)
            .Where(d => d.TrainsShadule != null)
            .Select(d => d.TrainsShadule.id)
            .ToListAsync();

            var newIndex = await context.TrainsShadule
            .Where(l => !indexedIds.Contains(l.id))
            .Take(500) // Берем пачками по 100 штук, чтобы не грузить память
            .ToListAsync();

            foreach (var item in newIndex)
            {
                if (item.ObjectName == null)
                {
                    item.ObjectName = $"{nameof(TrainsShadule).ToUpper()} - {item.id}";
                }
                var doc = new DocumentToIndex
                {
                    NameObject = item.ObjectName,
                    Path = nameof(TrainsShadule),
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    TrainsShadule = item // Привязываем по ID (или объектом)
                };
                context.DocumentToIndex.Add(doc);
            }

            if (newIndex.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        private async Task Trains(ApplicationContext context)
        {
            var indexedIds = await context.DocumentToIndex
            .Include(d => d.Trains)
            .Where(d => d.Trains != null)
            .Select(d => d.Trains.id)
            .ToListAsync();

            var newIndex = await context.Trains
            .Where(l => !indexedIds.Contains(l.id))
            .Take(500) // Берем пачками по 100 штук, чтобы не грузить память
            .ToListAsync();

            foreach (var item in newIndex)
            {
                if (item.ObjectName == null)
                {
                    item.ObjectName = $"{nameof(Train).ToUpper()} - {item.id}";
                }
                var doc = new DocumentToIndex
                {
                    NameObject = item.ObjectName,
                    Path = nameof(Train),
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    Trains = item // Привязываем по ID (или объектом)
                };
                context.DocumentToIndex.Add(doc);
            }

            if (newIndex.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        private async Task ElectricTrains(ApplicationContext context)
        {
            var indexedIds = await context.DocumentToIndex
            .Include(d => d.Electric)
            .Where(d => d.Electric != null)
            .Select(d => d.Electric.id)
            .ToListAsync();

            var newIndex = await context.Electrics
            .Where(l => !indexedIds.Contains(l.id))
            .Take(500) // Берем пачками по 100 штук, чтобы не грузить память
            .ToListAsync();

            foreach (var item in newIndex)
            {
                if (item.ObjectName == null)
                {
                    item.ObjectName = $"{nameof(ElectricTrain).ToUpper()} - {item.id}";
                }
                var doc = new DocumentToIndex
                {
                    NameObject = item.ObjectName,
                    Path = nameof(ElectricTrain),
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    Electric = item // Привязываем по ID (или объектом)
                };
                context.DocumentToIndex.Add(doc);
            }

            if (newIndex.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        private async Task IndexDieselTrains(ApplicationContext context)
        {
            var indexedIds = await context.DocumentToIndex
            .Include(d => d.DieselTrains)
            .Where(d => d.DieselTrains != null)
            .Select(d => d.DieselTrains.Id)
            .ToListAsync();

            var newIndex = await context.DieselTrains
            .Where(l => !indexedIds.Contains(l.Id))
            .Take(500) // Берем пачками по 100 штук, чтобы не грузить память
            .ToListAsync();

            foreach (var item in newIndex)
            {
                if (item.ObjectName == null)
                {
                    item.ObjectName = $"{nameof(DieselTrains).ToUpper()} - {item.Id}";
                }
                var doc = new DocumentToIndex
                {
                    NameObject = item.ObjectName,
                    Path = nameof(DieselTrains),
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    DieselTrains = item // Привязываем по ID (или объектом)
                };
                context.DocumentToIndex.Add(doc);
            }

            if (newIndex.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        private async Task IndexStations(ApplicationContext context)
        {
            var indexedIds = await context.DocumentToIndex
            .Include(d => d.Stations)
            .Where(d => d.Stations != null)
            .Select(d => d.Stations.id)
            .ToListAsync();

            var newIndex = await context.Stations
            .Where(l => !indexedIds.Contains(l.id))
            .Take(500) // Берем пачками по 100 штук, чтобы не грузить память
            .ToListAsync();

            foreach (var item in newIndex)
            {
                if (item.ObjectName == null)
                {
                    item.ObjectName = $"{nameof(Stations).ToUpper()} - {item.id}";
                }
                var doc = new DocumentToIndex
                {
                    NameObject = item.ObjectName,
                    Path = nameof(Stations),
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    Stations = item // Привязываем по ID (или объектом)
                };
                context.DocumentToIndex.Add(doc);
            }

            if (newIndex.Any())
            {
                await context.SaveChangesAsync();
            }
        }

        private async Task IndexLoco(ApplicationContext context)
        {
            var indexedIds = await context.DocumentToIndex
            .Include(d => d.Locomotive)
            .Where(d => d.Locomotive != null)
            .Select(d => d.Locomotive.id)
            .ToListAsync();

            var newIndex = await context.Locomotives
            .Where(l => !indexedIds.Contains(l.id))
            .Take(500) // Берем пачками по 100 штук, чтобы не грузить память
            .ToListAsync();

            foreach (var item in newIndex)
            {
                if (item.ObjectName == null)
                {
                    item.ObjectName = $"{nameof(Locomotive).ToUpper()} - {item.id}";
                }
                var doc = new DocumentToIndex
                {
                    NameObject = item.ObjectName,
                    Path = nameof(Locomotive),
                    DateCreate = DateTime.Now,
                    DateUpdate = DateTime.Now,
                    Locomotive = item // Привязываем по ID (или объектом)
                };
                context.DocumentToIndex.Add(doc);
            }

            if (newIndex.Any())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
