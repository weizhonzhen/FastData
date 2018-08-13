using System.Configuration;

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
        public static RedisConfig GetConfig()
        {
            RedisConfig section = (RedisConfig)ConfigurationManager.GetSection("RedisConfig");
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
}
