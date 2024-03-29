﻿using FastData.Base;
using FastData.CacheModel;
using FastData.Config;
using FastData.Context;
using FastUntility.Base;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Linq;
using FastUntility.Page;
using DbProviderFactories = FastData.Base.DbProviderFactories;

namespace FastData.Proxy
{
    internal class ProxyHandler : IProxyHandler
    {
        public object Invoke(object proxy, MethodInfo method, object[] args)
        {
            var pModel = new PageModel();
            var param = new List<DbParameter>();
            var config = DataConfig.GetConfig();
            var key = string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);

            if (DbCache.Exists(config.CacheType, key))
            {
                var model = DbCache.Get<ServiceModel>(config.CacheType, key);

                if (model.isPage)
                    pModel = (PageModel)args.ToList().Find(a => a.GetType() == typeof(PageModel));

                config = DataConfig.GetConfig(model.dbKey);
                if (model.isSysType)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].GetType() == typeof(PageModel))
                            continue;

                        var temp = DbProviderFactories.GetFactory(config).CreateParameter();
                        temp.ParameterName = model.param.GetValue(i.ToString()).ToStr();
                        temp.Value = args[i];
                        param.Add(temp);
                    }
                }
                else if(model.isDic)
                {
                    if (!args.ToList().Exists(a => a.GetType() == typeof(Dictionary<string, object>)))
                        throw new ProxyException($"error: service {method.DeclaringType.Name} , method {method.Name} , param type {args[0].GetType().Name} is not support");

                    var dic = (Dictionary<string, object>)args.ToList().Find(a => a.GetType() == typeof(Dictionary<string, object>));
                    var tempDic = new Dictionary<int, string>();

                    foreach (KeyValuePair<string, object> keyValue in dic)
                    {
                        key = string.Format("{0}{1}", config.Flag, keyValue.Key).ToLower();
                        if (model?.sql.IndexOf(key) > 0 || model.isXml)
                        {
                            tempDic.Add((int)(model?.sql.IndexOf(key)), keyValue.Key);
                        }
                    }
                    var list = tempDic.OrderBy(d => d.Key).ToList();
                    foreach (KeyValuePair<int, string> keyValue in list)
                    {
                        var temp = DbProviderFactories.GetFactory(config).CreateParameter();
                        temp.ParameterName = keyValue.Value;
                        temp.Value = dic.GetValue(keyValue.Value);
                        param.Add(temp);
                    }
                }
                else
                {
                    var data = args.ToList().Find(a => a.GetType() != typeof(PageModel));
                    var type = method.GetParameters().ToList().Find(a => a.ParameterType != typeof(PageModel)).ParameterType;
                    for (int i = 0; i < model.param.Count; i++)
                    {
                        var temp = DbProviderFactories.GetFactory(config).CreateParameter();
                        temp.ParameterName = model.param.GetValue(i.ToString()).ToStr();
                        temp.Value = BaseEmit.Get(data, temp.ParameterName);
                        temp.ParameterName = temp.ParameterName.ToLower();
                        param.Add(temp);
                    }
                }

                if (model.isXml)
                    model.sql = MapXml.GetFastMapSql(method, config, ref param);

                using (var db = new DataContext(config.Key))
                {
                    if (model.isWrite)
                        return db.ExecuteSql(model.sql, param.ToArray(), Aop.AopType.FaseWrite).writeReturn;
                    else
                        return db.FastReadAttribute(model, param, pModel);
                }
            }

            throw new ProxyException($"error: service {method.DeclaringType.Name} , method {method.Name} not exists");
        }
    }
}
