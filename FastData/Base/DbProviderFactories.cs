using FastData.Model;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace FastData.Base
{
    internal class DbProviderFactories : DbProviderFactory
    {
        /// <summary>
        /// 动态加载数据库工厂
        /// </summary>
        /// <returns></returns>
        public static DbProviderFactory GetFactory(ConfigModel config)
        {
            try
            {
                if (DbCache.Exists(CacheType.Web, config.ProviderName))
                    return DbCache.Get<object>(CacheType.Web, config.ProviderName) as DbProviderFactory;
                else
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies().ToList().Find(a => a.FullName.Split(',')[0] == config.ProviderName);
                    if (assembly == null)
                        assembly = Assembly.Load(config.ProviderName);

                    var model = assembly.GetType(config.FactoryClient, false);
                    var field = model.GetField("Instance");
                    var instance = field.GetValue(model);

                    DbCache.Set<object>(CacheType.Web, config.ProviderName, instance);
                    return instance as DbProviderFactory;
                }
            }
            catch (Exception ex)
            {
                DbLog.LogException(config.IsOutError, config.DbType, ex, "GetFactory", "");
                return null;
            }
        }
    }
}
