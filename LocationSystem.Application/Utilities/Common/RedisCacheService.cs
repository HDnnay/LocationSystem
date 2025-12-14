using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LocationSystem.Application.Utilities.Common
{
    public class RedisCacheService:ICacheService
    {
        private readonly IDistributedCache _Cache;
        public RedisCacheService(IDistributedCache distributedCache)
        {
            _Cache = distributedCache;
        }

        public TResult? GetOrCreate<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, TResult?> valueFactory, int expireSeconds = 60)
        {
            string? jsonStr = _Cache.GetString(cacheKey);
            //缓存不存在
            if (string.IsNullOrEmpty(jsonStr))
            {
                var options = CreateCacheOption(expireSeconds);
                TResult? result = valueFactory(options);
                //如果数据源中也没有查到，可能会返回null,null会被json序列化为字符串"null"，所以可以防范“缓存穿透”
                var jsonString = JsonSerializer.Serialize(result, typeof(TResult));
                _Cache.SetString(cacheKey, jsonString, options);
                return result;
            }
            else
            {
                _Cache.Refresh(cacheKey);//刷新，以便于滑动过期时间延期
                return JsonSerializer.Deserialize<TResult>(jsonStr)!;
            }
        }

        private DistributedCacheEntryOptions CreateCacheOption(int expireSeconds)
        {
            var seconds = Random.Shared.Next(expireSeconds, expireSeconds + 5);
            TimeSpan exTimeSpan = TimeSpan.FromSeconds(seconds);
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = exTimeSpan;
            return options;
        }

        public async Task<TResult?> GetOrCreateAsync<TResult>(string cacheKey, Func<DistributedCacheEntryOptions, Task<TResult?>> valueFactory, int expireSeconds = 60)
        {
            string? jsonStr = await _Cache.GetStringAsync(cacheKey);
            //缓存不存在
            if (string.IsNullOrEmpty(jsonStr))
            {
                var options = CreateCacheOption(expireSeconds);
                TResult? result = await valueFactory(options);
                //如果数据源中也没有查到，可能会返回null,null会被json序列化为字符串"null"，所以可以防范“缓存穿透”
                var jsonString = JsonSerializer.Serialize(result, typeof(TResult));
                await _Cache.SetStringAsync(cacheKey, jsonString, options);
                return result;
            }
            else
            {
                await _Cache.RefreshAsync(cacheKey);//刷新，以便于滑动过期时间延期
                return JsonSerializer.Deserialize<TResult>(jsonStr)!;
            }
        }

        public void Remove(string cacheKey)
        {
            _Cache.Remove(cacheKey);
        }
    }
}
