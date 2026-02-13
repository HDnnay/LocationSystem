﻿using Microsoft.Extensions.Caching.Distributed;
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

        public async Task RemoveByPatternAsync(string pattern)
        {
            // 注意：由于使用的是 IDistributedCache，没有直接的方法根据模式删除缓存
            // 在实际项目中，如果使用的是 StackExchange.Redis，可以通过以下方式实现：
            // 1. 获取 Redis 连接
            // 2. 使用 KEYS 或 SCAN 命令查找匹配的键
            // 3. 使用 DEL 命令删除这些键
            // 这里暂时留空，实际项目中需要根据具体的缓存实现来修改
            await Task.CompletedTask;
        }

        public async Task ClearAllAsync()
        {
            // 注意：由于使用的是 IDistributedCache，没有直接的方法清除所有缓存
            // 在实际项目中，如果使用的是 StackExchange.Redis，可以通过 FLUSHDB 命令实现
            // 这里暂时留空，实际项目中需要根据具体的缓存实现来修改
            await Task.CompletedTask;
        }
    }
}
