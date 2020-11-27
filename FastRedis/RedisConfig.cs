using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Xml;

namespace FastRedis.Config
{
    /// <summary>
    /// redis 缓存配置
    /// </summary>
    internal sealed class RedisConfig : ConfigurationSection
    {
        #region 配置信息
        /// <summary>
        /// 配置信息
        /// </summary>
        /// <returns></returns>
        public static RedisConfig GetConfig(string projectName = null, string dbFile = "db.config")
        {
            var section = new RedisConfig();
            var cacheKey = "FastRedis.db.config";
            if (BaseCache.Exists(cacheKey))
                return BaseCache.Get<RedisConfig>(cacheKey);
            else if (projectName == null)
            {
                if (dbFile.ToLower() == "web.config")
                    section = (RedisConfig)ConfigurationManager.GetSection("RedisConfig");
                else
                {
                    var exeConfig = new ExeConfigurationFileMap();
                    exeConfig.ExeConfigFilename = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, dbFile);
                    ConfigurationManager.OpenMappedExeConfiguration(exeConfig, ConfigurationUserLevel.None);
                    section = (RedisConfig)ConfigurationManager.GetSection("RedisConfig");
                }
            }
            else
            {
                var assembly = Assembly.Load(projectName);
                using (var resource = assembly.GetManifestResourceStream(string.Format("{0}.{1}", projectName, dbFile)))
                {
                    if (resource != null)
                    {
                        using (var reader = new StreamReader(resource))
                        {
                            var content = reader.ReadToEnd();
                            var xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(content);
                            var nodelList = xmlDoc.SelectNodes("configuration/RedisConfig");
                            foreach (XmlNode node in nodelList)
                            {
                                section.AutoStart = node.Attributes["AutoStart"]?.Value.ToStr().ToLower() == "true" || node.Attributes["IsOutError"]?.Value.ToStr() == null;
                                section.MaxReadPoolSize = node.Attributes["MaxReadPoolSize"].Value.ToStr().ToInt(60);
                                section.MaxWritePoolSize = node.Attributes["MaxWritePoolSize"].Value.ToStr().ToInt(60);
                                section.ReadServerList = node.Attributes["ReadServerList"]?.Value;
                                section.WriteServerList = node.Attributes["WriteServerList"]?.Value;
                            }
                        }
                    }
                }
            }

            BaseCache.Set<RedisConfig>(cacheKey, section);
            return section;
        }
        #endregion

        #region 写服务器
        /// <summary>
        /// 写服务器
        /// </summary>
        [ConfigurationProperty("WriteServerList", IsRequired = false)]
        public string WriteServerList
        {
            get
            {
                return base["WriteServerList"].ToString();
            }
            set
            {
                base["WriteServerList"] = value;
            }
        }
        #endregion

        #region 读服务器,各个服务器之间用逗号分开
        /// <summary>
        ///读服务器,各个服务器之间用逗号分开
        /// </summary>
        [ConfigurationProperty("ReadServerList", IsRequired = false)]
        public string ReadServerList
        {
            get
            {
                return base["ReadServerList"].ToString();
            }
            set
            {
                base["ReadServerList"] = value;
            }
        }
        #endregion

        #region 最大写链接数
        /// <summary>
        /// 最大写链接数
        /// </summary>
        [ConfigurationProperty("MaxWritePoolSize", IsRequired = false, DefaultValue = 60)]
        public int MaxWritePoolSize
        {
            get
            {
                int _maxWritePoolSize = (int)base["MaxWritePoolSize"];
                return _maxWritePoolSize > 0 ? _maxWritePoolSize : 60;
            }
            set
            {
                base["MaxWritePoolSize"] = value;
            }
        }
        #endregion

        #region 最大读链接数
        /// <summary>
        /// 最大读链接数
        /// </summary>
        [ConfigurationProperty("MaxReadPoolSize", IsRequired = false, DefaultValue = 60)]
        public int MaxReadPoolSize
        {
            get
            {
                int _maxReadPoolSize = (int)base["MaxReadPoolSize"];
                return _maxReadPoolSize > 0 ? _maxReadPoolSize : 60;
            }
            set
            {
                base["MaxReadPoolSize"] = value;
            }
        }
        #endregion 

        #region 自动重启
        /// <summary>
        /// 自动重启
        /// </summary>
        [ConfigurationProperty("AutoStart", IsRequired = false, DefaultValue = true)]
        public bool AutoStart
        {
            get
            {
                return (bool)base["AutoStart"];
            }
            set
            {
                base["AutoStart"] = value;
            }
        }
        #endregion
    }

    internal static class BaseRegular
    {
        public static string ToStr(this object strValue)
        {
            if (strValue == null)
                return "";
            else
                return strValue.ToString();
        }

        public static int ToInt(this string str, int defValue)
        {
            int tmp = 0;
            if (Int32.TryParse(str, out tmp))
                return (int)tmp;
            else
                return defValue;
        }
    }

    internal static class BaseCache
    {
        public static ObjectCache cache = MemoryCache.Default;

        public static void Set<T>(string key, T value, int Hours = 24 * 30 * 12) where T : class, new()
        {
            if (!string.IsNullOrEmpty(key))
            {
                cache.Remove(key);
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now.AddHours(Hours);
                cache.Set(key, value, policy);
            }
        }

        public static T Get<T>(string key) where T : class, new()
        {
            if (!string.IsNullOrEmpty(key))
            {
                var result = new T();
                var obj = cache.Get(key);
                if (obj != null)
                    result = (T)obj;
                return result;
            }
            else
                return new T();
        }

        public static bool Exists(string key)
        {
            if (!string.IsNullOrEmpty(key))
                return cache.Contains(key);
            else
                return false;
        }
    }
}
