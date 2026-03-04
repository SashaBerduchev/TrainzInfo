using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrainzInfoApplicationContext;
using TrainzInfoLog;
using TrainzInfoServices;
using TrainzInfoShared.DTO.GetDTO;

namespace TrainzInfo.Tools.BackgroundServices
{
    public class CacheWarmupService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NewsCacheService _newsCacheService;
        private readonly LocomotivesCacheService _locoCacheService;
        private readonly StationsCacheService _stationCacheService;
        private readonly ElectricsCacheService _electricsCacheService;
        private readonly DieselCacheService _dieselCacheService;
        public CacheWarmupService(IServiceProvider serviceProvider, NewsCacheService newsCacheService,
            LocomotivesCacheService locoCacheService,
            StationsCacheService stationsCacheService, ElectricsCacheService electricsCacheService,
            DieselCacheService dieselCacheService)
        {
            _serviceProvider = serviceProvider;
            _newsCacheService = newsCacheService;
            _locoCacheService = locoCacheService;
            _stationCacheService = stationsCacheService;
            _electricsCacheService = electricsCacheService;
            _dieselCacheService = dieselCacheService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Init("CacheWarmupService", nameof(StartAsync));
            Log.Wright("CacheWarmupService запущено");
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

            Log.Wright("Починаємо прогрів кешу...");
            Log.Wright("Прогрів кешу для новин...");
            await CacheNews(context, cache, cancellationToken);
            Log.Wright("Прогрів кешу для локомотивів...");
            await CacheLocomotives(context, cache, cancellationToken);
            Log.Wright("Прогрів кешу для станцій...");
            await CacheStations(context, cache, cancellationToken);
            await CacheElectrics(context, cache, cancellationToken);
            await CacheDiesels(context, cache, cancellationToken);
            Log.Wright("Прогрів кешу завершено");
            Log.Finish();
        }

        private async Task CacheDiesels(ApplicationContext context, IMemoryCache cache, CancellationToken cancellationToken)
        {
            string filia = null;
            string model = null;
            string depot = null;
            string oblast = null;
            int pageSize = 10;
            for (int page = 1; page <= 10; page++)
            {
                var filters = new
                {
                    filia = filia?.Trim().ToLower(),
                    depot = depot?.Trim().ToLower(),
                    model = model?.Trim().ToLower(),
                    oblast = oblast?.Trim().ToLower(),
                    page
                };
                string cacheKey = $"diesls_{JsonSerializer.Serialize(filters)}";

                var query = context.DieselTrains.AsNoTracking()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
                var data = await query.Select(dt => new DieselTrainsDTO
                {
                    Id = dt.Id,
                    Name = dt.SuburbanTrainsInfo.Model,
                    SuburbanTrainsInfo = dt.SuburbanTrainsInfo.BaseInfo,
                    NumberTrain = dt.NumberTrain,
                    DepotList = dt.DepotList.Name,
                    City = dt.DepotList.City.Name,
                    Oblast = dt.DepotList.City.Oblasts.Name,
                    Filia = dt.DepotList.UkrainsRailway.Name,
                    Image = dt.Image != null
                                        ? $"api/diesels/{dt.Id}/image?width=600" : null,
                    ImageMimeTypeOfData = dt.ImageMimeTypeOfData,
                    Station = dt.Stations.Name
                }).ToListAsync();

                var token = _dieselCacheService.GetToken();
                cache.Set(cacheKey, data,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                        .AddExpirationToken(token));
            }
        }

        private async Task CacheElectrics(ApplicationContext context, IMemoryCache cache, CancellationToken cancellationToken)
        {
            string filia = null;
            string name = null;
            string depo = null;
            int pageSize = 10;
            for (int page = 1; page <= 10; page++)
            {
                var filters = new
                {
                    filia = filia?.Trim().ToLower(),
                    name = name?.Trim().ToLower(),
                    depo = depo?.Trim().ToLower(),
                    page
                };

                string cacheKey = $"electrics_{JsonSerializer.Serialize(filters)}";
                var query = context.Electrics.AsNoTracking()
                    .OrderBy(x => x.Model)
                    .ThenBy(x => x.Trains.Model)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
                var data = await query.Select(x => new ElectricTrainDTO
                {
                    id = x.id,
                    Name = x.Name,
                    Model = x.Model,
                    MaxSpeed = x.MaxSpeed,
                    DepotTrain = x.DepotTrain,
                    DepotCity = x.DepotCity,
                    Image = x.Image != null
                                    ? $"api/electrictrains/{x.id}/image?width=600" : null,
                    ImageMimeTypeOfData = x.ImageMimeTypeOfData,
                    DepotList = x.DepotList.Name,
                    Oblast = x.City.Oblasts.Name,
                    UkrainsRailway = x.DepotList.UkrainsRailway.Name,
                    City = x.City.Name,
                    TrainsInfo = x.Trains.BaseInfo,
                    ElectrickTrainzInformation = x.ElectrickTrainzInformation.AllInformation,
                    Station = x.Stations.Name
                }).ToListAsync();
                IChangeToken token = _electricsCacheService.GetToken();
                cache.Set(cacheKey, data,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                        .AddExpirationToken(token));
            }

        }

        private async Task CacheStations(ApplicationContext context, IMemoryCache cache, CancellationToken cancellationToken)
        {
            string filia = null;
            string name = null;
            string oblast = null;
            int pageSize = 10;
            for (int page = 1; page <= 10; page++)
            {
                var filters = new
                {
                    filia = filia?.Trim().ToLower(),
                    name = name?.Trim().ToLower(),
                    oblast = oblast?.Trim().ToLower(),
                    page
                };
                string cacheKey = $"stations_{JsonSerializer.Serialize(filters)}";

                var query = context.Stations;


                var stations = await query
                    .OrderBy(s => s.id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new StationsDTO
                    {
                        id = s.id,
                        Name = s.Name,
                        DopImgSrc = s.DopImgSrc,
                        DopImgSrcSec = s.DopImgSrcSec,
                        DopImgSrcThd = s.DopImgSrcThd,
                        ImageMimeTypeOfData = s.ImageMimeTypeOfData,
                        // EF Core сам зробить потрібні JOIN, Include не треба писати вручну
                        UkrainsRailways = s.UkrainsRailways.Name,
                        Oblasts = s.Oblasts.Name,
                        Citys = s.Citys.Name,
                        StationInfo = s.StationInfo.BaseInfo,
                        Metro = s.Metro.Name,
                        StationImages = s.StationImages.Image != null ? $"api/stations/{s.StationImages.id}/image?width=600" : null
                    })
                    .ToListAsync();

                IChangeToken token = _stationCacheService.GetToken();
                cache.Set(cacheKey, stations,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                        .AddExpirationToken(token));
            }
        }

        private async Task CacheLocomotives(ApplicationContext context, IMemoryCache cache, CancellationToken cancellationToken)
        {
            string filia = null;
            string name = null;
            string oblast = null;
            int pageSize = 10;


            for (int page = 1; page <= 10; page++)
            {
                var filters = new
                {
                    filia = (string)null,
                    depot = (string)null,
                    seria = (string)null,
                    oblast = (string)null,
                    page
                };

                string cacheKey = $"locomotives_{JsonSerializer.Serialize(filters)}";

                var query = context.Locomotives.AsNoTracking()
                    .OrderBy(x => x.Locomotive_Series.Seria)
                    .ThenBy(x => x.Number)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                var data = await query
                    .Select(n => new LocomotiveDTO
                    {
                        Id = n.id,
                        Number = n.Number,
                        Speed = n.Speed,
                        Depot = n.DepotList.Name,
                        City = n.DepotList.City.Name,
                        Oblast = n.DepotList.City.Oblasts.Name,
                        Filia = n.DepotList.UkrainsRailway.Name,
                        Seria = n.Locomotive_Series.Seria,
                        Station = n.Stations.Name,
                        ImgSrc = n.Image != null ? $"api/locomotives/{n.id}/image?width=600" : null
                    }).ToListAsync();

                var token = _locoCacheService.GetToken();
                cache.Set(cacheKey, data,
                   new MemoryCacheEntryOptions()
                       .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                       .AddExpirationToken(token));
            }
        }

        private async Task CacheNews(ApplicationContext context, IMemoryCache cache, CancellationToken cancellationToken)
        {
            int pageSize = 6;

            for (int page = 1; page <= 5; page++) // прогріваємо перші 3 сторінки
            {
                string cacheKey = $"news_page_{page}";

                var data = await context.NewsInfos
                        .Include(n => n.NewsComments)
                        .Include(n => n.User)
                        .Include(n => n.NewsImages)
                        .OrderByDescending(n => n.DateTime)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(n => new NewsDTO
                        {
                            id = n.id,
                            NameNews = n.NameNews,
                            BaseNewsInfo = n.BaseNewsInfo,
                            NewsInfoAll = n.NewsInfoAll,
                            DateTime = n.DateTime.ToString("yyyy-MM-dd"),
                            ImgSrc = n.NewsImages.Image != null
                                ? $"api/news/{n.NewsImages.id}/image?width=600" : null
                        })
                        .ToListAsync();
                var token = _newsCacheService.GetToken();
                cache.Set(cacheKey, data,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                        .AddExpirationToken(token));
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
    }
}
