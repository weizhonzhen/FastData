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
        [ConfigurationProperty("IsOutSql", IsRequired = true, DefaultValue = true)]
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
        [ConfigurationProperty("IsOutError", IsRequired = true, DefaultValue = true)]
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
        [ConfigurationProperty("IsMapSave", IsRequired = false, DefaultValue = "true")]
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
    }
}
