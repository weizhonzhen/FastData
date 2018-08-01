using System.Runtime.Caching;
using System;

namespace Fast.Untility.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    public static class BaseCache
    {
        public static ObjectCache cache = new MemoryCache(Guid.NewGuid().ToString());

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="regionName">区域名</param>
        /// <param name="Hours">过期小时</param>
        public static void Set(string key, string value, string regionName, int Hours = 24 * 30 * 12)
        {
            if (!string.IsNullOrEmpty(key))
            {
                cache.Remove(key, regionName);
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now.AddHours(Hours);
                cache.Set(key, value, policy, regionName);
            }
        }
                
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="regionName">区域名</param>
        /// <param name="Hours">过期小时</param>
        public static void Set<T>(string key,T value,string regionName, int Hours = 24 * 30 * 12) where T : class, new()
        {
            if (!string.IsNullOrEmpty(key))
            {
                cache.Remove(key, regionName);
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now.AddHours(Hours);
                cache.Set(key, value, policy, regionName);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="regionName">区域名</param>
        public static object Get(string key, string regionName)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                   return cache.Get(key, regionName);
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="regionName">区域名</param>
        public static T Get<T>(string key, string regionName) where T : class, new()
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var result = new T();
                    var obj = cache.Get(key, regionName);
                    if (obj != null)
                        result = (T)obj;
                    return result;
                }
                else
                    return new T();
            }
            catch
            {
                return new T();
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="regionName">区域名</param>
        public static void Remove(string key, string regionName)
        {
            if (!string.IsNullOrEmpty(key))
                cache.Remove(key, regionName);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="regionName">区域名</param>
        public static bool Exists(string key,string regionName)
        {
            if (!string.IsNullOrEmpty(key))
                return cache.Contains(key, regionName);
            else
                return false;
        }
    }
}
