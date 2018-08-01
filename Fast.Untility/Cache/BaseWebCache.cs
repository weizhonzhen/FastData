using System;
using System.Collections.Generic;
using System.Web;

namespace Fast.Untility.Cache
{
    /// <summary>
    /// 标签：2015.7.13，魏中针
    /// 说明：缓存操作类
    /// </summary>
    public static class BaseWebCache
    {
        #region 获取值
        /// <summary>
        /// 获取值
        /// </summary>
        public static string Get(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var result = HttpRuntime.Cache.Get(key);
                return result == null ? "" : result.ToString();
            }
            else
                return null;
        }
        #endregion

        #region 设置值
        /// <summary>
        /// 设置值
        /// </summary>
        public static bool Set(string Name, string Value, int Hours = 24*30*12)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                Clear(Name);
                HttpRuntime.Cache.Insert(Name, Value, null, DateTime.Now.AddHours(Hours), System.Web.Caching.Cache.NoSlidingExpiration);
                return true;
            }
            else
                return false;
        }
        #endregion

        #region 增加值
        /// <summary>
        /// 增加值
        /// </summary>
        public static void Add<T>(string key, T Value) where T : class,new()
        {
            var list = Get<List<T>>(key);
            list.Add(Value);
            Set<List<T>>(key, list);
        }
        #endregion

        #region 删除值
        /// <summary>
        /// 删除值
        /// </summary>
        public static void Remove<T>(string key, T Value) where T : class,new()
        {
            var list = Get<List<T>>(key);
            list.Remove(Value);
            Set<List<T>>(key, list);
        }
        #endregion

        #region 获取值
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class,new()
        {
            try
            {
                var result = new T();
                object obj = HttpRuntime.Cache.Get(key);
                if (obj != null)
                    result = (T)((object)obj);
                return result;
            }
            catch
            {
                return new T();
            }
        }
        #endregion

        #region 设置值
        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public static bool Set<T>(string Name, T Value, int Hours = 24*30*12)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                Clear(Name);
                HttpRuntime.Cache.Insert(Name, Value, null, DateTime.Now.AddHours(Hours), System.Web.Caching.Cache.NoSlidingExpiration);
                return true;
            }
            else
                return false;
        }
        #endregion

        #region 是否存在
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return HttpRuntime.Cache.Get(key) != null;
            }
            else
                return false;
        }
        #endregion

        #region 清空
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="Name"></param>
        public static bool Clear(string key)
        {

            if (!string.IsNullOrEmpty(key))
            {
                HttpRuntime.Cache.Remove(key);
                return true;
            }
            else
                return false;
        }
        #endregion

        #region 清空所有缓存
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public static void ClearAll()
        {
            var cache = HttpRuntime.Cache.GetEnumerator();
            while (cache.MoveNext())
            {
                HttpRuntime.Cache.Remove(cache.Key.ToString());
            }
        }
        #endregion
    }
}
