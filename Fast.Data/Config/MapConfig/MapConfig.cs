using System;
using System.IO;
using System.Configuration;
using Fast.Untility.Base;
using Fast.Base;
using Fast.Redis;
using Fast.CacheModel;

namespace Fast.Config
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
                var key = "Fast.Config.SqlMap";
                var exeConfig = new ExeConfigurationFileMap();
                exeConfig.ExeConfigFilename = string.Format("{0}SqlMap.config", AppDomain.CurrentDomain.BaseDirectory);
                var info = new FileInfo(exeConfig.ExeConfigFilename);

                if (!RedisInfo.Exists(key, RedisDb.Config))
                {
                    var temp = getModel(exeConfig, info);
                    RedisInfo.SetItem<MapConfigModel>(key, temp,8640, RedisDb.Config);
                    return temp;
                }
                else if (RedisInfo.GetItem<MapConfigModel>(key, RedisDb.Config).LastWrite != info.LastWriteTime)
                {
                    var temp = getModel(exeConfig, info);
                    RedisInfo.SetItem<MapConfigModel>(key, temp,8640, RedisDb.Config);
                    return temp;
                }
                else
                    return RedisInfo.GetItem<MapConfigModel>(key,RedisDb.Config);
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
