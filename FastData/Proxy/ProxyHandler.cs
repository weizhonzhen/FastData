using FastData.Base;
using FastData.CacheModel;
using FastData.Config;
using FastData.Context;
using FastUntility.Base;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace FastData.Proxy
{
    internal class ProxyHandler : IProxyHandler
    {
        public object Invoke(object proxy, MethodInfo method, object[] args)
        {
            var param = new List<DbParameter>();
            var config = DataConfig.GetConfig();
            var key = string.Format("{0}.{1}", method.DeclaringType.Name, method.Name);

            if (DbCache.Exists(config.CacheType, key))
            {
                var model = DbCache.Get<ServiceModel>(config.CacheType, key);
                config = DataConfig.GetConfig(model.dbKey);

                for (int i = 0; i < args.Length; i++)
                {
                    var temp = DbProviderFactories.GetFactory(config.ProviderName).CreateParameter();
                    temp.ParameterName = model.param.GetValue(i.ToString()).ToStr();
                    temp.Value = args[i];
                    param.Add(temp);
                }

                using (var db = new DataContext(config.Key))
                {
                    if (model.isWrite)
                        return db.ExecuteSql(model.sql, param.ToArray(), Aop.AopType.FaseWrite).writeReturn;
                    else
                        return db.FastReadAttribute(model, param);
                }
            }

            throw new Exception($"error: service {method.DeclaringType.Name} , method {method.Name} not exists");
        }
    }
}
