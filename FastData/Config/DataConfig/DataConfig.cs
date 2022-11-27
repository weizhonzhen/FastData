using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using FastData.Type;
using FastData.Model;
using FastData.Base;
using FastUntility.Base;
using System;
using System.Reflection;
using System.IO;
using System.Xml;

namespace FastData.Config
{
    /// <summary>
    /// 数据库配置类
    /// </summary>
    internal class DataConfig : ConfigurationSection
    {
        private static readonly string cacheKey = "FastData.db.config";

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
        public static ConfigModel GetConfig(string key = null, string projectName = null, string dbFile = "web.config")
        {
            var result = new ConfigModel();
            var list = new List<ConfigModel>();
            var config = new DataConfig();

            if (DbCache.Exists(CacheType.Web, cacheKey))
                list = DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);
            else if (projectName == null)
            {
                if (string.Compare( dbFile,"web.config", true) ==0 || string.Compare( dbFile, "app.config", true) ==0)
                    config = (DataConfig)ConfigurationManager.GetSection("DataConfig");
                else
                {
                    var exeConfig = new ExeConfigurationFileMap();
                    exeConfig.ExeConfigFilename = string.Format("{0}bin\\{1}", AppDomain.CurrentDomain.BaseDirectory, dbFile);
                    config = (DataConfig)ConfigurationManager.OpenMappedExeConfiguration(exeConfig, ConfigurationUserLevel.None).GetSection("DataConfig");
                }

                #region Db2
                if (config.DB2.Count != 0)
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
                    }
                }
                #endregion

                #region oracle
                if (config.Oracle.Count != 0)
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
                }
                #endregion

                #region mysql
                if (config.MySql.Count != 0)
                {
                    foreach (var temp in config.MySql)
                    {
                        var item = new ConfigModel();
                        item.DbType = DataDbType.MySql;
                        item.Flag = "@";
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
                }
                #endregion

                #region sqlserver
                if (config.SqlServer.Count != 0)
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
                }
                #endregion

                #region sqlite
                if (config.SQLite.Count != 0)
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
                }
                #endregion

                #region PostgreSql
                if (config.PostgreSql.Count != 0)
                {
                    foreach (var temp in config.PostgreSql)
                    {
                        var item = new ConfigModel();
                        item.DbType = DataDbType.PostgreSql;
                        item.Flag = "@";
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
                }
                #endregion
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
                            var nodelList = xmlDoc.SelectNodes("configuration/DataConfig");
                            foreach (XmlNode node in nodelList)
                            {
                                foreach (XmlNode leaf in node.ChildNodes)
                                {
                                    foreach (XmlNode db in leaf.ChildNodes)
                                    {
                                        var item = new ConfigModel();
                                        if (string.Compare( leaf.Name, DataDbType.DB2, true) ==0)
                                        {
                                            item.DbType = DataDbType.DB2;
                                            item.Flag = "@";
                                            item.ProviderName = Provider.DB2;
                                        }

                                        if (string.Compare(leaf.Name, DataDbType.Oracle, true) == 0)
                                        {
                                            item.DbType = DataDbType.Oracle;
                                            item.Flag = ":";
                                            item.ProviderName = Provider.Oracle;
                                        }

                                        if (string.Compare(leaf.Name, DataDbType.MySql, true) == 0)
                                        {
                                            item.DbType = DataDbType.MySql;
                                            item.Flag = "@";
                                            item.ProviderName = Provider.MySql;
                                        }

                                        if (string.Compare(leaf.Name, DataDbType.SqlServer, true) == 0)
                                        {
                                            item.DbType = DataDbType.SqlServer;
                                            item.Flag = "@";
                                            item.ProviderName = Provider.SqlServer;
                                        }

                                        if (string.Compare(leaf.Name, DataDbType.SQLite, true) == 0)
                                        {
                                            item.DbType = DataDbType.SQLite;
                                            item.Flag = "@";
                                            item.ProviderName = Provider.SQLite;
                                        }

                                        if (string.Compare(leaf.Name, DataDbType.PostgreSql, true) == 0)
                                        {
                                            item.DbType = DataDbType.PostgreSql;
                                            item.Flag = "@";
                                            item.ProviderName = Provider.PostgreSql;
                                        }

                                        if (item.DbType != null)
                                        {
                                            item.ConnStr = db.Attributes["ConnStr"]?.Value;
                                            item.IsOutError = string.Compare( db.Attributes["IsOutError"]?.Value.ToStr(), "true", true) ==0 || db.Attributes["IsOutError"]?.Value.ToStr() == null;
                                            item.IsOutSql =string.Compare( db.Attributes["IsOutSql"]?.Value.ToStr(), "true", true) ==0 || db.Attributes["IsOutSql"]?.Value.ToStr() == null;
                                            item.IsPropertyCache =string.Compare( db.Attributes["IsPropertyCache"]?.Value.ToStr(), "true", true) ==0 || db.Attributes["IsPropertyCache"]?.Value.ToStr() == null;
                                            item.Key = db.Attributes["Key"]?.Value;
                                            item.DbLinkName = db.Attributes["DbLinkName"]?.Value;
                                            item.DesignModel = db.Attributes["DesignModel"]?.Value;
                                            item.IsEncrypt = string.Compare( db.Attributes["IsEncrypt"]?.Value.ToStr(),"true", true) ==0;
                                            item.IsMapSave = string.Compare( db.Attributes["IsMapSave"]?.Value.ToStr(),"true", true) ==0;
                                            item.SqlErrorType = db.Attributes["SqlErrorType"]?.Value;
                                            item.CacheType = db.Attributes["CacheType"]?.Value;
                                            item.IsUpdateCache = false;

                                            item.DesignModel = item.DesignModel == null ? "DbFirst" : item.DesignModel;
                                            item.CacheType = item.CacheType == null ? "db" : item.CacheType;

                                            list.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            DbCache.Set<List<ConfigModel>>(CacheType.Web, cacheKey, list);

            if (string.IsNullOrEmpty(key))
                result = list.First();
            else
                result = list.Find(a => a.Key == key);

            return result;
        }
        #endregion

        public static bool DataType(string key = null, string projectName = null, string dbFile = "web.config")
        {
            var result = new List<bool>();

            if (!DbCache.Exists(CacheType.Web, cacheKey))
                DataConfig.GetConfig(key, projectName, dbFile);

            var list = DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);

            result.Add(list.Count(a => a.DbType == DataDbType.Oracle) > 0);
            result.Add(list.Count(a => a.DbType == DataDbType.DB2) > 0);
            result.Add(list.Count(a => a.DbType == DataDbType.SQLite) > 0);
            result.Add(list.Count(a => a.DbType == DataDbType.SqlServer) > 0);
            result.Add(list.Count(a => a.DbType == DataDbType.PostgreSql) > 0);
            result.Add(list.Count(a => a.DbType == DataDbType.MySql) > 0);

            return result.Count(a => a == true) > 1;
        }

        public static List<ConfigModel> List
        {
            get
            {
                return DbCache.Get<List<ConfigModel>>(CacheType.Web, cacheKey);
            }
        }
    }
}
