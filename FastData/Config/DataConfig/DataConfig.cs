using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using FastData.Type;
using FastData.Model;
using FastUntility.Base;
using FastData.Base;
using System;
using System.IO;
using FastUntility.Cache;

namespace FastData.Config
{
    /// <summary>
    /// 数据库配置类
    /// </summary>
    internal class DataConfig : ConfigurationSection
    {
        #region oralce 节点
        /// <summary>
        /// oralce 节点
        /// </summary>
        [ConfigurationProperty("Oracle")]
        [ConfigurationCollection(typeof(CollectionConfig), AddItemName = "Add")]
        public CollectionConfig Oracle
        {
            get
            {
                return (CollectionConfig)this["Oracle"];
            }
        }
        #endregion
        
        #region sqlserver 节点
         ///<summary>
         ///sqlserver 节点
         ///</summary>
        [ConfigurationProperty("SqlServer")]
        [ConfigurationCollection(typeof(CollectionConfig), AddItemName = "Add")]
        public CollectionConfig SqlServer
        {
            get
            {
                return (CollectionConfig)this["SqlServer"];
            }
        }
        #endregion

        #region mysql 节点
        /// <summary>
        /// mysql 节点
        /// </summary>
        [ConfigurationProperty("MySql")]
        [ConfigurationCollection(typeof(CollectionConfig), AddItemName = "Add")]
        public CollectionConfig MySql
        {
            get
            {
                return (CollectionConfig)this["MySql"];
            }
        }
        #endregion
        
        #region db2 节点
        /// <summary>
        /// db2 节点
        /// </summary>
        [ConfigurationProperty("DB2")]
        [ConfigurationCollection(typeof(CollectionConfig), AddItemName = "Add")]
        public CollectionConfig DB2
        {
            get
            {
                return (CollectionConfig)this["DB2"];
            }
        }
        #endregion

        #region SQLite 节点
        /// <summary>
        /// SQLite 节点
        /// </summary>
        [ConfigurationProperty("SQLite", IsRequired = false)]
        [ConfigurationCollection(typeof(CollectionConfig), AddItemName = "Add")]
        public CollectionConfig SQLite
        {
            get
            {
                return (CollectionConfig)this["SQLite"];
            }
        }
        #endregion

        #region PostgreSql 节点
        /// <summary>
        /// PostgreSql 节点
        /// </summary>
        [ConfigurationProperty("PostgreSql", IsRequired = false)]
        [ConfigurationCollection(typeof(CollectionConfig), AddItemName = "Add")]
        public CollectionConfig PostgreSql
        {
            get
            {
                return (CollectionConfig)this["PostgreSql"];
            }
        }
        #endregion

        #region 获取配置节点
        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <returns></returns>
        public static ConfigModel GetConfig(string key = null)
        {
            var result = new ConfigModel();
            var list = new List<ConfigModel>();
            var config = (DataConfig)ConfigurationManager.GetSection("DataConfig");

            #region Db2
            if (config.DB2.Count != 0)
            {
                var cacheKey = "DataConfig.DB2";
                
                if (IsReadCache)
                {
                    var cacheList = DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);

                    if (string.IsNullOrEmpty(key))
                        result = cacheList.First();
                    else
                        result = cacheList.Find(a => a.Key == key);
                }
                else
                {
                    foreach (var temp in config.DB2)
                    {
                        var item = new ConfigModel();
                        item.DbType = DataDbType.DB2;
                        item.Flag = "@";
                        item.ProviderName = Provider.DB2;
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        item.SqlErrorType = (temp as ElementConfig).SqlErrorType;
                        item.CacheType = (temp as ElementConfig).CacheType;
                        item.IsUpdateCache = (temp as ElementConfig).IsUpdateCache;
                        list.Add(item);

                        if (string.IsNullOrEmpty(key))
                            result = list.First();
                        else
                            result = list.Find(a => a.Key == key);
                    }
                    
                    DbCache.Set<List<ConfigModel>>(CacheType.Web,cacheKey, list);
                }
            }
            #endregion

            #region oracle
            if (config.Oracle.Count != 0)
            {
                var cacheKey = "DataConfig.Oracle";

                if (IsReadCache)
                {
                    var cacheList = DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);

                    if (string.IsNullOrEmpty(key))
                        result = cacheList.First();
                    else
                        result = cacheList.Find(a => a.Key == key);
                }
                else
                {
                    foreach (var temp in config.Oracle)
                    {
                        var item = new ConfigModel();
                        item.DbType = DataDbType.Oracle;
                        item.Flag = ":";
                        item.ProviderName = Provider.Oracle;
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        item.SqlErrorType = (temp as ElementConfig).SqlErrorType;
                        item.CacheType = (temp as ElementConfig).CacheType;
                        item.IsUpdateCache = (temp as ElementConfig).IsUpdateCache;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);

                    DbCache.Set<List<ConfigModel>>(CacheType.Web, cacheKey, list);
                }
            }
            #endregion

            #region mysql
            if (config.MySql.Count != 0)
            {
                var cacheKey = "DataConfig.MySql";

                if (IsReadCache)
                {
                    var cacheList = DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);

                    if (string.IsNullOrEmpty(key))
                        result = cacheList.First();
                    else
                        result = cacheList.Find(a => a.Key == key);
                }
                else
                {
                    foreach (var temp in config.MySql)
                    {
                        var item = new ConfigModel();
                        item.DbType = DataDbType.MySql;
                        item.Flag = "?";
                        item.ProviderName = Provider.MySql;
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        item.SqlErrorType = (temp as ElementConfig).SqlErrorType;
                        item.CacheType = (temp as ElementConfig).CacheType;
                        item.IsUpdateCache = (temp as ElementConfig).IsUpdateCache;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);

                    DbCache.Set<List<ConfigModel>>(CacheType.Web, cacheKey, list);
                }
            }
            #endregion

            #region sqlserver
            if (config.SqlServer.Count != 0)
            {
                var cacheKey = "DataConfig.SqlServer";

                if (IsReadCache)
                {
                    var cacheList = DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);

                    if (string.IsNullOrEmpty(key))
                        result = cacheList.First();
                    else
                        result = cacheList.Find(a => a.Key == key);
                }
                else
                {
                    foreach (var temp in config.SqlServer)
                    {
                        var item = new ConfigModel();
                        item.DbType = DataDbType.SqlServer;
                        item.Flag = "@";
                        item.ProviderName = Provider.SqlServer;
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        item.SqlErrorType = (temp as ElementConfig).SqlErrorType;
                        item.CacheType = (temp as ElementConfig).CacheType;
                        item.IsUpdateCache = (temp as ElementConfig).IsUpdateCache;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);

                    DbCache.Set<List<ConfigModel>>(CacheType.Web, cacheKey, list);
                }
            }
            #endregion

            #region sqlite
            if (config.SQLite.Count != 0)
            {
                var cacheKey = "DataConfig.SQLite";

                if (IsReadCache)
                {
                    var cacheList = DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);

                    if (string.IsNullOrEmpty(key))
                        result = cacheList.First();
                    else
                        result = cacheList.Find(a => a.Key == key);
                }
                else
                {
                    foreach (var temp in config.SQLite)
                    {
                        var item = new ConfigModel();
                        item.DbType = DataDbType.SQLite;
                        item.Flag = "@";
                        item.ProviderName = Provider.SQLite;
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        item.SqlErrorType = (temp as ElementConfig).SqlErrorType;
                        item.CacheType = (temp as ElementConfig).CacheType;
                        item.IsUpdateCache = (temp as ElementConfig).IsUpdateCache;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);
                    DbCache.Set<List<ConfigModel>>(CacheType.Web, cacheKey, list);
                }
            }
            #endregion

            #region PostgreSql
            if (config.PostgreSql.Count != 0)
            {
                var cacheKey = "DataConfig.PostgreSql";

                if (IsReadCache)
                {
                    var cacheList = DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);

                    if (string.IsNullOrEmpty(key))
                        result = cacheList.First();
                    else
                        result = cacheList.Find(a => a.Key == key);
                }
                else
                {
                    foreach (var temp in config.PostgreSql)
                    {
                        var item = new ConfigModel();
                        item.DbType = DataDbType.PostgreSql;
                        item.Flag = ":";
                        item.ProviderName = Provider.PostgreSql;
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        item.SqlErrorType = (temp as ElementConfig).SqlErrorType;
                        item.CacheType = (temp as ElementConfig).CacheType;
                        item.IsUpdateCache = (temp as ElementConfig).IsUpdateCache;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);
                    DbCache.Set<List<ConfigModel>>(CacheType.Web, cacheKey, list);
                }
            }
            #endregion

            return result;
        }
        #endregion
                
        public static bool DataType(string key = null)
        {
            var config = (DataConfig)ConfigurationManager.GetSection("DataConfig");

            var result = new List<bool>();
            result.Add(config.Oracle.Count > 1);
            result.Add(config.DB2.Count > 1);
            result.Add(config.SQLite.Count > 1);
            result.Add(config.SqlServer.Count > 1);
            result.Add(config.PostgreSql.Count > 1);
            result.Add(config.MySql.Count > 1);

            return result.Count(a => a == true) > 1;
        }

        #region 是否从缓存读取
        /// <summary>
        /// 是否从缓存读取
        /// </summary>
        private static bool IsReadCache
        {
            get
            {
                var fileName = string.Format("{0}Web.config", AppDomain.CurrentDomain.BaseDirectory);
                var info = new FileInfo(fileName);
                var fileKey = "DataConfig.File";

                if (DbCache.Exists(CacheType.Web, fileKey))
                {
                    if ((DbCache.Get(CacheType.Web, fileKey).ToDate() - info.LastWriteTime).Minutes != 0)
                        return false;
                    else
                        return true;
                }
                else
                    DbCache.Set(CacheType.Web, fileKey, info.LastWriteTime.ToDate("yyyy-MM-dd HH:mm:ss"));
                return false;
            }
        }
        #endregion
    }
}
