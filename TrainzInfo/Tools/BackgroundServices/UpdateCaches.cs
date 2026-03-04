using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrainzInfo.Data;
using TrainzInfo.Services;
using TrainzInfoLog;
using TrainzInfoShared.DTO.GetDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrainzInfo.Tools.BackgroundServices
{
    public class UpdateCaches : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateCaches(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ForceUpdate();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        private async Task ForceUpdate()
        {
            Log.Init(nameof(UpdateCaches), nameof(ExecuteAsync));
            Log.Wright("Updatecache start.");
            Log.Wright("Updatecache triggered.");
            try
            {
                Log.Wright("Updatecache: Starting Updatecache process.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
                    var newstoken = scope.ServiceProvider.GetRequiredService<NewsCacheService>();
                    var locotoken = scope.ServiceProvider.GetRequiredService<LocomotivesCacheService>();
                    var stationstoken = scope.ServiceProvider.GetRequiredService<StationsCacheService>();

                    ClearCaches(cache, newstoken, locotoken, stationstoken);
                    Log.Wright("UpdateCache: Starting cache update process.");
                    Log.Wright("UpdateCache: Caching stations.");
                    await CacheStations(context, cache, stationstoken);
                    Log.Wright("UpdateCache: Caching locomotives.");
                    await CacheLocomotives(context, cache, locotoken);
                    Log.Wright("UpdateCache: Caching news.");
                    await CacheNews(context, cache, newstoken);
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

        private void ClearCaches(IMemoryCache cache, NewsCacheService newsCache, LocomotivesCacheService locomotivesCache, StationsCacheService stationsCache)
        {
            newsCache.Clear(); // очищаємо кеш новин
            locomotivesCache.Clear(); // очищаємо кеш новин
            stationsCache.Clear(); // очищаємо кеш новин
        }



        private async Task CacheStations(ApplicationContext context, IMemoryCache cache, StationsCacheService cacheService)
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
                        StationImages = s.StationImages.Image != null ? $"api/stations/{s.StationImages.id}/image?width=300" : null
                    })
                    .ToListAsync();

                IChangeToken token = cacheService.GetToken();
                cache.Set(cacheKey, stations,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                        .AddExpirationToken(token));
            }
        }

        private async Task CacheLocomotives(ApplicationContext context, IMemoryCache cache, LocomotivesCacheService cacheService)
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
                        ImgSrc = n.Image != null ? $"api/locomotives/{n.id}/image?width=300" : null
                    }).ToListAsync();

                IChangeToken token = cacheService.GetToken();
                cache.Set(cacheKey, data,
                   new MemoryCacheEntryOptions()
                       .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                       .AddExpirationToken(token));
            }
        }

        private async Task CacheNews(ApplicationContext context, IMemoryCache cache, NewsCacheService cacheService)
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
                                ? $"api/news/{n.NewsImages.id}/image?width=300" : null
                        })
                        .ToListAsync();
                var token = cacheService.GetToken();
                cache.Set(cacheKey, data,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                        .AddExpirationToken(token));
            }

            List<NewsDTO> newsDTOs = await context.NewsInfos
                .Include(n => n.NewsComments)
                .Include(n => n.User)
                .Include(n => n.NewsImages)
                .OrderByDescending(n => n.DateTime)
                .Take(5) // беремо перші 20 новин для детального кешування
                .Select(n => new NewsDTO
                {
                    id = n.id,
                    NameNews = n.NameNews,
                    BaseNewsInfo = n.BaseNewsInfo,
                    NewsInfoAll = n.NewsInfoAll,
                    DateTime = n.DateTime.ToString("yyyy-MM-dd"),
                    ImgSrc = n.NewsImages.Image != null
                        ? $"api/news/{n.NewsImages.id}/image?width=300" : null
                })
                .ToListAsync();
            foreach (NewsDTO dto in newsDTOs) { 
                string cacheKey = $"news_detail_{dto.id}";
                var token = cacheService.GetToken();
                cache.Set(cacheKey, dto,
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                        .AddExpirationToken(token));
            }
        }
    }
}
