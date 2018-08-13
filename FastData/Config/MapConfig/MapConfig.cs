using System;
using System.IO;
using System.Configuration;
using FastUntility.Base;
using FastData.Base;
using FastData.CacheModel;
using FastUntility.Cache;

namespace FastData.Config
{
    /// <summary>
    /// 数据库配置类
    /// </summary>
    internal class MapConfig : ConfigurationSection
    {
        #region MapSql 节点
        /// <summary>
        /// MapSql 节点
        /// </summary>
        [ConfigurationProperty("SqlMap")]
        [ConfigurationCollection(typeof(MapCollection), AddItemName = "Add")]
        public MapCollection SqlMap
        {
            get
            {
                return (MapCollection)this["SqlMap"];
            }
        }
        #endregion

        #region 获取配置节点
        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <returns></returns>
        public static MapConfigModel GetConfig()
        {
            try
            {
                var key = "FastData.Config.SqlMap";
                var exeConfig = new ExeConfigurationFileMap();
                exeConfig.ExeConfigFilename = string.Format("{0}SqlMap.config", AppDomain.CurrentDomain.BaseDirectory);
                var info = new FileInfo(exeConfig.ExeConfigFilename);
                
                if (!DbCache.Exists(CacheType.Web, key))
                {
                    var temp = getModel(exeConfig, info);
                    DbCache.Set<MapConfigModel>(CacheType.Web, key, temp);
                    return temp;
                }
                else if (DbCache.Get<MapConfigModel>(CacheType.Web, key).LastWrite != info.LastWriteTime)
                {
                    var temp = getModel(exeConfig, info);
                    DbCache.Set<MapConfigModel>(CacheType.Web, key, temp);
                    return temp;
                }
                else
                    return DbCache.Get<MapConfigModel>(CacheType.Web, key);
            }
            catch(Exception ex)
            {
                BaseLog.SaveLog(ex.ToString(), "MapConfig");
                return null;
            }
        }
        #endregion

        #region 获取model
        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="exeConfig"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static MapConfigModel getModel(ExeConfigurationFileMap exeConfig, FileInfo info)
        {
            var result = new MapConfigModel();
            var config = ConfigurationManager.OpenMappedExeConfiguration(exeConfig, ConfigurationUserLevel.None);
            var item = (MapConfig)config.GetSection("MapConfig");
            foreach (var temp in item.SqlMap)
            {
                var path = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, (temp as MapElement).File.Replace("/", "\\"));
                if (File.Exists(path))
                    result.Path.Add(path);
            }
            result.LastWrite = info.LastWriteTime;

            return result;
        }
        #endregion
    }
}
