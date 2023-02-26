using FastAop;
using FastAop.Factory;
using FastData.Aop;
using FastData.Base;
using FastData.CacheModel;
using FastData.Check;
using FastData.Config;
using FastData.Context;
using FastData.Filter;
using FastData.Model;
using FastData.Property;
using FastData.Repository;
using FastData.Type;
using FastUntility.Base;
using FastUntility.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using static FastAop.FastAop;

namespace FastData
{
    public static partial class FastMap
    {
        public static IFastAop fastAop;
        private static string configKey = Guid.NewGuid().ToString();

        #region 统一入口
        /// <summary>
        /// 统一入口
        /// </summary>
        /// <param name="action"></param>
        public static void Init(Action<ConfigData> action)
        {
            var projectName = Assembly.GetCallingAssembly().GetName().Name;
            var config = new ConfigData();
            action(config);

            if (!string.IsNullOrEmpty(config.NamespaceProperties))
                InstanceProperties(config.NamespaceProperties, config.dbFile, config.IsResource, config.aop, projectName);

            if (config.IsResource)
                InstanceMapResource(config.dbKey, config.dbFile, config.mapFile, config.aop, projectName);
            else
                InstanceMap(config.dbKey, config.dbFile, config.mapFile, config.aop, projectName);

            if (config.IsCodeFirst && !string.IsNullOrEmpty(config.NamespaceCodeFirst))
            {
                InstanceProperties(config.NamespaceCodeFirst, config.dbFile, config.IsResource, config.aop, projectName);
                InstanceTable(config.NamespaceCodeFirst, config.dbKey, config.dbFile, config.IsResource, config.aop, projectName);
            }

            if (!string.IsNullOrEmpty(config.NamespaceService))
                InstanceService(config.NamespaceService);

            AddScoped(typeof(IUnitOfWorK),new UnitOfWorK());
            AddSingleton(typeof(IUnitOfWorK), new UnitOfWorK());
            AddTransient(typeof(IUnitOfWorK), new UnitOfWorK());

            AddScoped(typeof(IFastRepository), new FastRepository());
            AddSingleton(typeof(IFastRepository), new FastRepository());
            AddTransient(typeof(IFastRepository), new FastRepository());

            InitModelType(config.NamespaceProperties).ForEach(m =>
            {
                var type = typeof(FastRepository<>).MakeGenericType(new System.Type[1] { m });
                var obj = Activator.CreateInstance(type);
                AddScoped(type.GetInterfaces().First(), obj);
                AddSingleton(type.GetInterfaces().First(), obj);
                AddTransient(type.GetInterfaces().First(), obj);
            });

            if (config.webType == WebType.Mvc)
                System.Web.Mvc.ControllerBuilder.Current.SetControllerFactory(new AopMvcFactory());

            if (config.webType == WebType.WebApi)
                System.Web.Http.GlobalConfiguration.Configuration.Services.Replace(typeof(System.Web.Http.Dispatcher.IHttpControllerActivator), new AopWebApiFactory());

            if (config.webType == WebType.MvcAndWebApi)
            {
                System.Web.Mvc.ControllerBuilder.Current.SetControllerFactory(new AopMvcFactory());
                System.Web.Http.GlobalConfiguration.Configuration.Services.Replace(typeof(System.Web.Http.Dispatcher.IHttpControllerActivator), new AopWebApiFactory());
            }
        }
        #endregion

        #region 统一入口含注入
        /// <summary>
        /// 统一入口含注入
        /// </summary>
        /// <param name="action"></param>
        /// <param name="repository"></param>
        public static void InitGeneric(Action<ConfigRepository> repository)
        {
            var config = new ConfigRepository();
            repository(config);
            InitAopGeneric(config.NameSpaceServie, config.NameSpaceModel, config.webType, config.Aop);
        }
        #endregion

        #region 获取导航属性
        /// <summary>
        /// 获取导航属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static NavigateModel GetNavigate(System.Type type)
        {
            var navigate = new NavigateModel();
            type.GetProperties().ToList().ForEach(a =>
            {
                var attribute = a.GetCustomAttribute<NavigateAttribute>();
                if (attribute != null)
                {
                    navigate.Appand.Add(attribute.Appand);
                    navigate.Name.Add(a.Name);
                    navigate.Key.Add(attribute.Name);
                }
            });
            return navigate;
        }
        #endregion

        #region 初始化model成员 1
        /// <summary>
        /// 初始化model成员 1
        /// </summary>
        /// <param name="list"></param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="dll">dll名称</param>
        public static void InstanceProperties(string nameSpace, string dbFile = "web.config", bool isResource = false, IFastAop aop = null, string projectName = null)
        {
            projectName = projectName ?? Assembly.GetCallingAssembly().GetName().Name;
            InitAssembly();
            var config = new ConfigModel();
            if (aop != null)
                fastAop = aop;

            if (isResource)
                config = DataConfig.GetConfig(null, projectName, dbFile);
            else
                config = DataConfig.GetConfig(null, null, dbFile);

            if (config.CacheType == CacheType.Redis && isResource)
                FastRedis.RedisInfo.Init(dbFile, projectName);

            if (config.CacheType == CacheType.Redis)
                FastRedis.RedisInfo.Init(dbFile);

            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
            {
                try
                {
                    assembly.ExportedTypes.ToList().ForEach(t =>
                    {
                        var typeInfo = (t as TypeInfo);
                        if (typeInfo.Namespace != null && typeInfo.Namespace == nameSpace)
                        {
                            var key = string.Format("{0}.{1}", typeInfo.Namespace, typeInfo.Name);
                            var navigateKey = string.Format("{0}.navigate", key);
                            var cacheList = new List<PropertyModel>();
                            var cacheNavigate = new List<NavigateModel>();

                            typeInfo.DeclaredProperties.ToList().ForEach(a =>
                            {
                                var navigateType = a.GetCustomAttribute<NavigateTypeAttribute>();
                                if (navigateType != null && a.PropertyType == typeof(Dictionary<string, object>) && a.GetMethod.IsVirtual)
                                {
                                    var navigate = GetNavigate(navigateType.Type);
                                    navigate.IsList = false;
                                    navigate.PropertyType = navigateType.Type;
                                    navigate.MemberName = a.Name;
                                    navigate.MemberType = a.PropertyType;
                                    navigate.IsUpdate = navigateType.IsUpdate;
                                    navigate.IsDel = navigateType.IsDel;
                                    navigate.IsAdd = navigateType.IsAdd;
                                    if (navigate.Name.Count != 0)
                                        cacheNavigate.Add(navigate);
                                }
                                else if (navigateType != null && a.PropertyType == typeof(List<Dictionary<string, object>>) && a.GetMethod.IsVirtual)
                                {
                                    var navigate = GetNavigate(navigateType.Type);
                                    navigate.IsList = true;
                                    navigate.PropertyType = navigateType.Type;
                                    navigate.MemberName = a.Name;
                                    navigate.MemberType = a.PropertyType;
                                    navigate.IsUpdate = navigateType.IsUpdate;
                                    navigate.IsDel = navigateType.IsDel;
                                    navigate.IsAdd = navigateType.IsAdd;
                                    if (navigate.Name.Count != 0)
                                        cacheNavigate.Add(navigate);
                                }
                                else if (a.PropertyType.GetGenericArguments().Length > 0 && a.GetMethod.IsVirtual)
                                {
                                    var navigate = GetNavigate(a.PropertyType.GenericTypeArguments[0]);
                                    navigate.IsList = true;
                                    navigate.PropertyType = a.PropertyType.GenericTypeArguments[0];
                                    navigate.MemberName = a.Name;
                                    navigate.MemberType = a.PropertyType;
                                    if (navigateType != null)
                                    {
                                        navigate.IsUpdate = navigateType.IsUpdate;
                                        navigate.IsDel = navigateType.IsDel;
                                        navigate.IsAdd = navigateType.IsAdd;
                                    }
                                    if (navigate.Name.Count != 0)
                                        cacheNavigate.Add(navigate);
                                }
                                else if (a.GetMethod.IsVirtual)
                                {
                                    var navigate = GetNavigate(a.PropertyType);
                                    navigate.IsList = false;
                                    navigate.PropertyType = a.PropertyType;
                                    navigate.MemberName = a.Name;
                                    navigate.MemberType = a.PropertyType;
                                    if (navigateType != null)
                                    {
                                        navigate.IsUpdate = navigateType.IsUpdate;
                                        navigate.IsDel = navigateType.IsDel;
                                        navigate.IsAdd = navigateType.IsAdd;
                                    }
                                    if (navigate.Name.Count != 0)
                                        cacheNavigate.Add(navigate);
                                }
                                else
                                {
                                    var model = new PropertyModel();
                                    model.Name = a.Name;
                                    model.PropertyType = a.PropertyType;
                                    cacheList.Add(model);
                                }
                            });

                            if (cacheNavigate.Count > 0)
                                DbCache.Set<List<NavigateModel>>(config.CacheType, navigateKey, cacheNavigate);

                            DbCache.Set<List<PropertyModel>>(config.CacheType, key, cacheList);
                        }
                    });
                }
                catch (Exception ex) { }
            });
        }
        #endregion

        private static void InitAssembly()
        {
            Assembly.GetCallingAssembly().GetReferencedAssemblies().ToList().ForEach(a => {
                try
                {
                    if (!AppDomain.CurrentDomain.GetAssemblies().ToList().Exists(b => b.GetName().Name == a.Name))
                        Assembly.Load(a.Name);
                }
                catch (Exception ex) { }
            });
        }

        #region 初始化code first 2
        /// <summary>
        /// 初始化code first 2
        /// </summary>
        /// <param name="list"></param>
        /// <param name="nameSpace">命名空间</param>
        /// <param name="dll">dll名称</param>
        public static void InstanceTable(string nameSpace, string dbKey = null, string dbFile = "web.config", bool isResource = false, IFastAop aop = null, string projectName = null)
        {
            projectName = projectName ?? Assembly.GetCallingAssembly().GetName().Name;

            InitAssembly();
            if (aop != null)
                fastAop = aop;

            var query = new DataQuery();

            if (isResource)
                query.Config = DataConfig.GetConfig(dbKey, projectName, dbFile);
            else
                query.Config = DataConfig.GetConfig(dbKey, null, dbFile);

            query.Key = dbKey;

            if (query.Config.CacheType == CacheType.Redis && isResource)
                FastRedis.RedisInfo.Init(dbFile, projectName);

            if (query.Config.CacheType == CacheType.Redis && isResource)
                FastRedis.RedisInfo.Init(dbFile);

            MapXml.CreateLogTable(query);

            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
            {
                try
                {
                    assembly.ExportedTypes.ToList().ForEach(a =>
                    {
                        var typeInfo = (a as TypeInfo);
                        if (typeInfo.Namespace != null && typeInfo.Namespace == nameSpace && typeInfo.GetCustomAttribute<TableAttribute>() != null)
                        {
                            var tableName = typeInfo.GetCustomAttribute<TableAttribute>().Name ?? a.Name;
                            BaseTable.Check(query, tableName, typeInfo.DeclaredProperties.ToList(), typeInfo.GetCustomAttributes().ToList());
                        }
                    });
                }
                catch (Exception ex) { }
            });
        }
        #endregion

        #region 初始化map 3  by Resource
        public static void InstanceMapResource(string dbKey = null, string dbFile = "web.config", string mapFile = "SqlMap.config", IFastAop aop = null, string projectName = null)
        {
            if (aop != null)
                fastAop = aop;

            projectName = projectName ?? Assembly.GetCallingAssembly().GetName().Name;
            var config = DataConfig.GetConfig(dbKey, projectName, dbFile);

            if (config.CacheType == CacheType.Redis)
                FastRedis.RedisInfo.Init(dbFile, projectName);
            DbCache.Set<ConfigModel>(CacheType.Web, configKey, config);

            var assembly = Assembly.Load(projectName);

            using (var db = new DataContext(dbKey))
            {
                var map = new MapConfigModel();
                using (var resource = assembly.GetManifestResourceStream(string.Format("{0}.{1}", projectName, mapFile)))
                {
                    if (resource != null)
                    {
                        using (var reader = new StreamReader(resource))
                        {
                            var content = reader.ReadToEnd();
                            var xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(content);
                            var nodelList = xmlDoc.SelectNodes("configuration/MapConfig/SqlMap/Add");
                            foreach (XmlNode item in nodelList)
                            {
                                map.Path.Add(item.Attributes["File"].Value);
                            }
                        }
                    }
                    else
                        map = MapConfig.GetConfig(mapFile);
                }

                if (map.Path == null)
                    return;

                map.Path.ForEach(a =>
                {
                    using (var resource = assembly.GetManifestResourceStream(string.Format("{0}.{1}", projectName, a.Replace("/", "."))))
                    {
                        var xml = "";
                        if (resource != null)
                        {
                            using (var reader = new StreamReader(resource))
                            {
                                xml = reader.ReadToEnd();
                            }
                        }
                        var info = new FileInfo(a);
                        var key = BaseSymmetric.Generate(info.FullName);
                        if (!DbCache.Exists(config.CacheType, key))
                        {
                            var temp = new MapXmlModel();
                            temp.LastWrite = info.LastWriteTime;
                            temp.FileKey = MapXml.ReadXml(info.FullName, config, info.Name.ToLower().Replace(".xml", ""), xml);
                            temp.FileName = info.FullName;
                            if (MapXml.SaveXml(dbKey, key, info, config, db))
                                DbCache.Set<MapXmlModel>(config.CacheType, key, temp);
                        }
                        else if ((DbCache.Get<MapXmlModel>(config.CacheType, key).LastWrite - info.LastWriteTime).Milliseconds != 0)
                        {
                            DbCache.Get<MapXmlModel>(config.CacheType, key).FileKey.ForEach(f => { DbCache.Remove(config.CacheType, f); });

                            var model = new MapXmlModel();
                            model.LastWrite = info.LastWriteTime;
                            model.FileKey = MapXml.ReadXml(info.FullName, config, info.Name.ToLower().Replace(".xml", ""), xml);
                            model.FileName = info.FullName;
                            if (MapXml.SaveXml(dbKey, key, info, config, db))
                                DbCache.Set<MapXmlModel>(config.CacheType, key, model);
                        }
                    }
                });
            }
        }
        #endregion

        #region 初始化map 3
        /// <summary>
        /// 初始化map 3
        /// </summary>
        /// <returns></returns>
        public static void InstanceMap(string dbKey = null, string dbFile = "web.config", string mapFile = "SqlMap.config", IFastAop aop = null, string projectName = null)
        {
            if (aop != null)
                fastAop = aop;

            projectName = projectName ?? Assembly.GetCallingAssembly().GetName().Name;
            var list = MapConfig.GetConfig(mapFile);
            var config = DataConfig.GetConfig(dbKey, projectName, dbFile);
            DbCache.Set<ConfigModel>(CacheType.Web, configKey, config);

            using (var db = new DataContext(dbKey))
            {
                var query = new DataQuery { Config = config, Key = dbKey };

                if (config.IsMapSave)
                {
                    query.Config.DesignModel = FastData.Base.Config.CodeFirst;
                    if (query.Config.DbType == DataDbType.Oracle)
                    {
                        var listInfo = typeof(FastData.DataModel.Oracle.Data_MapFile).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList();
                        var listAttribute = typeof(FastData.DataModel.Oracle.Data_MapFile).GetTypeInfo().GetCustomAttributes().ToList();
                        BaseTable.Check(query, "Data_MapFile", listInfo, listAttribute);
                    }

                    if (query.Config.DbType == DataDbType.MySql)
                    {
                        var listInfo = typeof(FastData.DataModel.MySql.Data_MapFile).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList();
                        var listAttribute = typeof(FastData.DataModel.MySql.Data_MapFile).GetTypeInfo().GetCustomAttributes().ToList();
                        BaseTable.Check(query, "Data_MapFile", listInfo, listAttribute);
                    }

                    if (query.Config.DbType == DataDbType.SqlServer)
                    {
                        var listInfo = typeof(FastData.DataModel.SqlServer.Data_MapFile).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList();
                        var listAttribute = typeof(FastData.DataModel.SqlServer.Data_MapFile).GetTypeInfo().GetCustomAttributes().ToList();
                        BaseTable.Check(query, "Data_MapFile", listInfo, listAttribute);
                    }
                }

                if (list.Path == null)
                    return;

                list.Path.ForEach(p => {
                    var info = new FileInfo(p);
                    var key = BaseSymmetric.md5(32, info.FullName);

                    if (!DbCache.Exists(config.CacheType, key))
                    {
                        var temp = new MapXmlModel();
                        temp.LastWrite = info.LastWriteTime;
                        temp.FileKey = MapXml.ReadXml(p, config, info.Name.ToLower().Replace(".xml", ""));
                        temp.FileName = info.FullName;
                        if (MapXml.SaveXml(dbKey, key, info, config, db))
                            DbCache.Set<MapXmlModel>(config.CacheType, key, temp);
                    }
                    else if ((DbCache.Get<MapXmlModel>(config.CacheType, key).LastWrite - info.LastWriteTime).Milliseconds != 0)
                    {
                        DbCache.Get<MapXmlModel>(config.CacheType, key).FileKey.ForEach(a => { DbCache.Remove(config.CacheType, a); });

                        var model = new MapXmlModel();
                        model.LastWrite = info.LastWriteTime;
                        model.FileKey = MapXml.ReadXml(p, config, info.Name.ToLower().Replace(".xml", ""));
                        model.FileName = info.FullName;
                        if (MapXml.SaveXml(dbKey, key, info, config, db))
                            DbCache.Set<MapXmlModel>(config.CacheType, key, model);
                    }
                });
            }
        }
        #endregion

        #region 初始化 interface service
        public static void InstanceService(string nameSpace)
        {
            InitAssembly();
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
            {
                try
                {
                    foreach (var a in assembly.ExportedTypes.ToList())
                    {
                        if (a.Namespace == nameSpace)
                        {
                            var isRegister = false;
                            a.GetMethods().ToList().ForEach(m =>
                            {
                                ConfigModel config = new ConfigModel();
                                var model = new ServiceModel();
                                var read = m.GetCustomAttribute<FastReadAttribute>();
                                var write = m.GetCustomAttribute<FastWriteAttribute>();
                                var map = m.GetCustomAttribute<FastMapAttribute>();

                                if (read != null)
                                {
                                    isRegister = true;
                                    model.isWrite = false;
                                    model.sql = read.sql.ToLower();
                                    model.dbKey = read.dbKey;
                                    config = DataConfig.GetConfig(model.dbKey);
                                    model.isPage = read.isPage;
                                    model.type = m.ReturnType;
                                    ServiceParam(m, model, config);
                                }

                                if (write != null)
                                {
                                    isRegister = true;
                                    model.isWrite = true;
                                    model.sql = write.sql.ToLower();
                                    model.dbKey = write.dbKey;
                                    model.type = m.ReturnType;
                                    config = DataConfig.GetConfig(model.dbKey);
                                    ServiceParam(m, model, config);
                                    model.isList = false;
                                }

                                if (map != null)
                                {
                                    isRegister = true;
                                    model.isWrite = false;
                                    model.isXml = true;
                                    model.dbKey = map.dbKey;
                                    model.isPage = map.isPage;
                                    model.type = m.ReturnType;
                                    config = DataConfig.GetConfig(model.dbKey);
                                    MapXml.ReadFastMap(map.xml, m, config);
                                    ServiceParam(m, model, config);
                                }

                                if (isRegister)
                                {
                                    var key = string.Format("{0}.{1}", a.FullName, m.Name);
                                    DbCache.Set<ServiceModel>(config.CacheType, key, model);
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ProxyException)
                        throw ex;
                }
            });
        }
        #endregion

        #region 注入
        public static void InitAopGeneric(string NameSpaceServie, string NameSpaceModel, FastAop.WebType webType, FastAopAttribute Aop = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (string.IsNullOrEmpty(NameSpaceServie))
                return;

            if (string.IsNullOrEmpty(NameSpaceModel))
                return;

            InitModelType(NameSpaceModel).ForEach(m =>
            {
                var type = typeof(FastRepository<>).MakeGenericType(new System.Type[1] { m });
                var obj = Activator.CreateInstance(type);

                if (serviceLifetime == ServiceLifetime.Scoped)
                    AddScoped(type.GetInterfaces().First(), obj);

                if (serviceLifetime == ServiceLifetime.Transient)
                    AddTransient(type.GetInterfaces().First(), obj);

                if (serviceLifetime == ServiceLifetime.Singleton)
                    AddSingleton(type.GetInterfaces().First(), obj);
            });

            if (Aop != null)
            {
                FastAop.FastAop.InitGeneric(NameSpaceServie, NameSpaceModel, webType, Aop.GetType(), serviceLifetime);
                FastAop.FastAop.Init(NameSpaceServie, webType, Aop.GetType(), serviceLifetime);
            }

            if (Aop == null)
            {
                FastAop.FastAop.InitGeneric(NameSpaceServie, NameSpaceModel, webType, null, serviceLifetime);
                FastAop.FastAop.Init(NameSpaceServie, webType, null, serviceLifetime);
            }
        }
        #endregion


        #region 服务参数
        /// <summary>
        /// 服务参数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="model"></param>
        private static void ServiceParam(MethodInfo info, ServiceModel model, ConfigModel config)
        {
            if (info.ReturnType != typeof(WriteReturn) && model.isWrite)
                throw new ProxyException($"[return type only WriteReturn, service:{info.DeclaringType.Name}, method:{info.Name}, return type:{info.ReturnType} is not support]");

            if (string.IsNullOrEmpty(model.dbKey))
                throw new ProxyException($"[service:{info.DeclaringType.Name}, method:{info.Name}, dbkey is not null]");

            if (info.ReturnType.isSysType())
                throw new ProxyException($"[service:{info.DeclaringType.Name}, method:{info.Name}, return type:{info.ReturnType} is not support]");

            if (string.IsNullOrEmpty(model.sql) && !model.isXml)
                throw new ProxyException($"[service:{info.DeclaringType.Name}, method:{info.Name}, sql is not null]");

            if (model.isPage && !info.GetParameters().ToList().Exists(a => a.ParameterType == typeof(PageModel)))
                throw new ProxyException($"[service:{info.DeclaringType.Name}, method:{info.Name}, read data by page , parameter type:{typeof(PageModel).FullName} not exists]");

            if (info.GetParameters().Length == 1 && info.GetParameters()[0].ParameterType.IsGenericType)
                throw new ProxyException($"[service:{info.DeclaringType.Name}, method:{info.Name}, parameter type:{info.GetParameters()[0].ParameterType} is not support]");

            if (model.isPage && info.ReturnType.GetGenericArguments().Length > 0 && info.ReturnType == typeof(PageResult<>).MakeGenericType(new System.Type[] { info.ReturnType.GetGenericArguments()[0] }))
                model.type = info.ReturnType.GetGenericArguments()[0];
            else if (model.isPage && info.ReturnType == typeof(PageResult))
                model.type = null;
            else if (model.isPage)
                throw new ProxyException($"[service:{info.DeclaringType.Name}, method:{info.Name}, read data by page , return type:{info.ReturnType} is not support]");

            if (info.ReturnType == typeof(Dictionary<string, object>) && (!model.isWrite || model.isXml))
                model.isList = false;
            else if (info.ReturnType == typeof(List<Dictionary<string, object>>) && (!model.isWrite || model.isXml))
                model.isList = true;
            else if (!model.isWrite || model.isXml)
            {
                model.isList = info.ReturnType.GetGenericArguments().Length > 0;
                System.Type argType;

                if (model.isList)
                    argType = info.ReturnType.GetGenericArguments()[0];
                else
                    argType = info.ReturnType;

                if (argType.isSysType())
                    throw new ProxyException($"[service:{info.DeclaringType.Name}, method:{info.Name}, return type:{info.ReturnType} is not support]");
            }
            var dic = new Dictionary<int, string>();

            if (info.GetParameters().ToList().Exists(a => a.ParameterType == typeof(Dictionary<string, object>)))
                model.isDic = true;
            else if (!info.GetParameters().ToList().Exists(a => a.ParameterType.isSysType()))
            {
                var type = info.GetParameters().ToList().Find(a => a.ParameterType != typeof(PageModel)).ParameterType;
                var pro = PropertyCache.GetPropertyInfo(Activator.CreateInstance(type));
                pro.ForEach(a =>
                {
                    var key = string.Format("{0}{1}", config.Flag, a.Name).ToLower();
                    if (!model.isXml && model.sql.IndexOf(key) > 0)
                    {
                        dic.Add(model.sql.IndexOf(key), a.Name);
                    }
                });
            }
            else
            {
                for (int i = 0; i < info.GetParameters().Length; i++)
                {
                    var key = string.Format("{0}{1}", config.Flag, info.GetParameters()[i].Name).ToLower();
                    if (!model.isXml && model.sql.IndexOf(key) > 0)
                    {
                        dic.Add(model.sql.IndexOf(key), info.GetParameters()[i].Name.ToLower());
                    }
                }
                model.isSysType = true;
            }

            var list = dic.OrderBy(d => d.Key).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                model.param.Add(i.ToString(), dic[list[i].Key]);
            }
        }
        #endregion

        #region 增加过滤器
        public static void AddFastFilter<T>(Expression<Func<T, bool>> predicate, FilterType type)
        {
            var config = DbCache.Get<ConfigModel>(CacheType.Web, configKey);

            var query = new DataQuery();
            query.Table.Add(typeof(T).Name);
            query.Config = config;
            query.TableAsName.Add(typeof(T).Name, predicate.Parameters[0].Name);

            var model = VisitExpression.LambdaWhere<T>(predicate, query);

            if (predicate.Parameters.Count > 0)
            {
                var flag = string.Format("{0}.", (predicate.Parameters[0] as ParameterExpression).Name);
                model.Where = model.Where.Replace(flag, "");
            }

            var key = $"Filter.{typeof(T).Name}.{type.ToString()}";
            DbCache.Set<VisitModel>(CacheType.Web, key, model);
        }
        #endregion

        #region 增加全局多数据切换
        public static void AddFastKey(Action<ConfigKey> action)
        {
            var model = new ConfigKey();
            action(model);

            var key = $"FastData.Key.{typeof(ConfigKey).Name}";
            DbCache.Set<ConfigKey>(CacheType.Web, key, model);
        }
        #endregion

        private static List<System.Type> InitModelType(string nameSpaceModel)
        {
            var list = new List<System.Type>();
            if (string.IsNullOrEmpty(nameSpaceModel))
                return list;

            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
            {
                try
                {
                    if (assembly.IsDynamic)
                        return;

                    assembly.ExportedTypes.Where(a => a.Namespace != null && a.Namespace.Contains(nameSpaceModel)).ToList().ForEach(b =>
                    {
                        if (b.IsPublic && b.IsClass && !b.IsAbstract && !b.IsGenericType)
                            list.Add(b);
                    });
                }
                catch { }
            });

            return list;
        }
    }
}


public class ConfigKey
{
    public string dbKey { get; set; }
}