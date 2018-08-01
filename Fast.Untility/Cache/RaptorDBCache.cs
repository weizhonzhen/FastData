using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaptorDB;
using Fast.Untility.Base;
using System.Web;
using System.Runtime.Remoting.Messaging;

namespace Fast.Untility.Cache
{
    /// <summary>
    /// 持久化缓存
    /// </summary>
    public static class RaptorDBCache
    {
        private static string path = string.Format("{0}/App_Data/RaptorDB", AppDomain.CurrentDomain.BaseDirectory);

        public static RaptorDBString GetContext(string fileName)
        {
            var key = string.Format("cache{0}", fileName);
            var context = CallContext.GetData(key) as RaptorDBString;

            if (context == null)
            {
                context = new RaptorDBString(string.Format("{0}/{1}", path, fileName), false);
                CallContext.SetData(key, context);
            }

            return context;
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="fileName">文件名</param>
        public static void Remove(string key, string fileName)
        {
            if (!string.IsNullOrEmpty(key))
            {
                GetContext(fileName).RemoveKey(key);
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="fileName">文件名</param>
        public static void Set(string key,string value, string fileName)
        {
            if (!string.IsNullOrEmpty(key))
            {
                GetContext(fileName).RemoveKey(key);
                GetContext(fileName).Set(key, value);
            }
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="fileName">文件名</param>
        public static void Set<T>(string key, T value, string fileName) where T : class, new()
        {
            if (!string.IsNullOrEmpty(key))
            {
                GetContext(fileName).RemoveKey(key);
                GetContext(fileName).Set(key, value.ToByte());
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="fileName">文件名</param>
        public static T Get<T>(string key, string fileName) where T : class, new()
        {
            if (!string.IsNullOrEmpty(key))
            {
                byte[] val;
                if (GetContext(fileName).Get(key, out val))
                    return val.ToModel<T>();
                else
                    return new T();
            }
            else
                return new T();
        }
        

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="fileName">文件名</param>
        public static string Get(string key, string fileName)
        {
            if (!string.IsNullOrEmpty(key))
            {
                string val;
                if (GetContext(fileName).Get(key, out val))
                    return val;
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="fileName">文件名</param>
        public static bool Exists(string key, string fileName)
        {
            if (!string.IsNullOrEmpty(key))
            {
                string val;
                return GetContext(fileName).Get(key, out val);
            }
            else
                return false;
        }
    }
}
