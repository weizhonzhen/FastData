using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Fast.Type;
using Fast.Model;
using Fast.Untility.Base;
using Fast.Base;
using System;
using System.IO;
using Fast.Redis;

namespace Fast.Config
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

                if (IsReadRedis)
                {
                    var cacheList = RedisInfo.Get<List<ConfigModel>>(cacheKey, RedisDb.Config);

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
                        item.ProviderName = "IBM.Fast.DB2.iSeries";
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        list.Add(item);

                        if (string.IsNullOrEmpty(key))
                            result = list.First();
                        else
                            result = list.Find(a => a.Key == key);
                    }
                    
                    RedisInfo.Set<List<ConfigModel>>(cacheKey, list, RedisDb.Config);
                }
            }
            #endregion

            #region oracle
            if (config.Oracle.Count != 0)
            {
                var cacheKey = "DataConfig.Oracle";

                if (IsReadRedis)
                {
                    var cacheList = RedisInfo.Get<List<ConfigModel>>(cacheKey, RedisDb.Config);

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
                        item.ProviderName = "Oracle.ManagedDataAccess.Client";
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);

                    RedisInfo.Set<List<ConfigModel>>(cacheKey, list, RedisDb.Config);
                }
            }
            #endregion

            #region mysql
            if (config.MySql.Count != 0)
            {
                var cacheKey = "DataConfig.MySql";

                if (IsReadRedis)
                {
                    var cacheList = RedisInfo.Get<List<ConfigModel>>(cacheKey, RedisDb.Config);

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
                        item.ProviderName = "MySql.Fast.MySqlClient";
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);

                    RedisInfo.Set<List<ConfigModel>>(cacheKey, list, RedisDb.Config);
                }
            }
            #endregion

            #region sqlserver
            if (config.SqlServer.Count != 0)
            {
                var cacheKey = "DataConfig.SqlServer";

                if (IsReadRedis)
                {
                    var cacheList = RedisInfo.Get<List<ConfigModel>>(cacheKey, RedisDb.Config);

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
                        item.ProviderName = "System.Data.SqlClient";
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);

                    RedisInfo.Set<List<ConfigModel>>(cacheKey, list, RedisDb.Config);
                }
            }
            #endregion

            #region sqlite
            if (config.SQLite.Count != 0)
            {
                var cacheKey = "DataConfig.SQLite";

                if (IsReadRedis)
                {
                    var cacheList = RedisInfo.Get<List<ConfigModel>>(cacheKey, RedisDb.Config);

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
                        item.ProviderName = "System.Data.SQLite";
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);
                    RedisInfo.Set<List<ConfigModel>>(cacheKey, list, RedisDb.Config);
                }
            }
            #endregion

            #region PostgreSql
            if (config.PostgreSql.Count != 0)
            {
                var cacheKey = "DataConfig.SQLite";

                if (IsReadRedis)
                {
                    var cacheList = RedisInfo.Get<List<ConfigModel>>(cacheKey, RedisDb.Config);

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
                        item.ProviderName = "Npgsql";
                        item.ConnStr = (temp as ElementConfig).ConnStr;
                        item.IsOutError = (temp as ElementConfig).IsOutError;
                        item.IsOutSql = (temp as ElementConfig).IsOutSql;
                        item.IsPropertyCache = (temp as ElementConfig).IsPropertyCache;
                        item.Key = (temp as ElementConfig).Key;
                        item.DbLinkName = (temp as ElementConfig).DbLinkName;
                        item.DesignModel = (temp as ElementConfig).DesignModel;
                        item.IsEncrypt = (temp as ElementConfig).IsEncrypt;
                        item.IsMapSave = (temp as ElementConfig).IsMapSave;
                        list.Add(item);
                    }

                    if (string.IsNullOrEmpty(key))
                        result = list.First();
                    else
                        result = list.Find(a => a.Key == key);
                    RedisInfo.Set<List<ConfigModel>>(cacheKey, list, RedisDb.Config);
                }
            }
            #endregion

            return result;
        }
        #endregion

        #region 是否从redis读取
        /// <summary>
        /// 是否从redis读取
        /// </summary>
        private static bool IsReadRedis
        {
            get
            {
                var fileName = string.Format("{0}Web.config", AppDomain.CurrentDomain.BaseDirectory);
                var info = new FileInfo(fileName);
                var fileKey = "DataConfig.File";

                if (RedisInfo.Exists(fileKey, RedisDb.Config))
                {
                    if ((RedisInfo.Get(fileKey, RedisDb.Config).ToDate() - info.LastWriteTime).Minutes != 0)
                        return false;
                    else
                        return true;
                }
                else
                    RedisInfo.Set(fileKey, info.LastWriteTime.ToDate("yyyy-MM-dd HH:mm:ss"), RedisDb.Config);
                return false;
            }
        }
        #endregion
    }
}
