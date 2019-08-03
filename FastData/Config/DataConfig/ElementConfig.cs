using System.Configuration;

namespace FastData.Config
{
    /// <summary>
    /// 连接串子节点
    /// </summary>
    internal class ElementConfig : ConfigurationElement
    {
        #region 数据库连接DLL名
        /// <summary>
        /// 数据库连接DLL名
        /// </summary>
        public string ProviderName { get; set; }
        #endregion
        
        #region 参数串字符
        /// <summary>
        /// 参数串字符
        /// </summary>
        public string Flag { get; set; }
        #endregion

        #region 数据库类型
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType {get;set;}
        #endregion

        
        #region 连接串
        /// <summary>
        /// 连接串
        /// </summary>
        [ConfigurationProperty("ConnStr", IsRequired = true)]
        public string ConnStr
        {
            get
            {
                return base["ConnStr"].ToString();
            }
            set
            {
                base["ConnStr"] = value;
            }
        }
        #endregion

        #region 是否输出SQL
        /// <summary>
        /// 是否输出SQL
        /// </summary>
        [ConfigurationProperty("IsOutSql", IsRequired = false, DefaultValue = true)]
        public bool IsOutSql
        {
            get
            {
                return (bool)base["IsOutSql"];
            }
            set
            {
                base["IsOutSql"] = value;
            }
        }
        #endregion

        #region 是否输出错误
        /// <summary>
        /// 是否输入错误
        /// </summary>
        [ConfigurationProperty("IsOutError", IsRequired = false, DefaultValue = true)]
        public bool IsOutError
        {
            get
            {
                return (bool)base["IsOutError"];
            }
            set
            {
                base["IsOutError"] = value;
            }
        }
        #endregion

        #region 是否缓存属性
        /// <summary>
        /// 是否缓存属性
        /// </summary>
        [ConfigurationProperty("IsPropertyCache", IsRequired = false, DefaultValue = true)]
        public bool IsPropertyCache
        {
            get
            {
                return (bool)base["IsPropertyCache"];
            }
            set
            {
                base["IsPropertyCache"] = value;
            }
        }
        #endregion

        #region DbLinkName
        /// <summary>
        /// DbLinkName
        /// </summary>
        [ConfigurationProperty("DbLinkName", IsRequired = false, DefaultValue = "")]
        public string DbLinkName
        {
            get
            {
                return base["DbLinkName"].ToString();
            }
            set
            {
                base["DbLinkName"] = value;
            }
        }
        #endregion
                        
        #region key 
        /// <summary>
        ///  key 
        /// </summary>
        [ConfigurationProperty("Key", IsRequired = false, DefaultValue = "")]
        public string Key
        {
            get
            {
                return base["Key"].ToString();
            }
            set
            {
                base["Key"] = value;
            }
        }
        #endregion

        #region map文件是否放数据库 
        /// <summary>
        ///  map文件是否放数据库 
        /// </summary>
        [ConfigurationProperty("IsMapSave", IsRequired = false, DefaultValue = "false")]
        public bool IsMapSave
        {
            get
            {
                return (bool)base["IsMapSave"];
            }
            set
            {
                base["IsMapSave"] = value;
            }
        }
        #endregion

        #region map文件是否加密 
        /// <summary>
        ///  map文件是否加密 
        /// </summary>
        [ConfigurationProperty("IsEncrypt", IsRequired = false, DefaultValue = "false")]
        public bool IsEncrypt
        {
            get
            {
                return (bool)base["IsEncrypt"];
            }
            set
            {
                base["IsEncrypt"] = value;
            }
        }
        #endregion

        #region 设计模式
        /// <summary>
        /// 设计模式
        /// </summary>
        [ConfigurationProperty("DesignModel", IsRequired = false, DefaultValue = "DbFirst")]
        public string DesignModel
        {
            get
            {
                return base["DesignModel"].ToString();
            }
            set
            {
                base["DesignModel"] = value;
            }
        }
        #endregion

        #region sql error 存放类型file,db 
        /// <summary>
        ///  sql Error存放类型file,db  
        /// </summary>
        [ConfigurationProperty("SqlErrorType", IsRequired = false, DefaultValue = "db")]
        public string SqlErrorType
        {
            get
            {
                return (string)base["SqlErrorType"];
            }
            set
            {
                base["SqlErrorType"] = value;
            }
        }
        #endregion

        #region 缓存类型
        /// <summary>
        ///  缓存类型  web,redis
        /// </summary>
        [ConfigurationProperty("CacheType", IsRequired = false, DefaultValue = "web")]
        public string CacheType
        {
            get
            {
                return (string)base["CacheType"];
            }
            set
            {
                base["CacheType"] = value;
            }
        }
        #endregion

        #region 是否更新缓存
        /// <summary>
        ///  是否更新缓存
        /// </summary>
        [ConfigurationProperty("IsUpdateCache", IsRequired = false, DefaultValue = "false")]
        public bool IsUpdateCache
        {
            get
            {
                return (bool)base["IsUpdateCache"];
            }
            set
            {
                base["IsUpdateCache"] = value;
            }
        }
        #endregion
    }
}
