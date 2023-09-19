using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;
using FastUntility.Page;
using FastData.Base;
using FastData.Config;
using FastData.Model;
using System.Diagnostics;
using System.IO;
using FastUntility.Base;
using FastData.Context;
using FastData.Aop;
using FastData.Proxy;

namespace FastData
{
    /// <summary>
    /// map
    /// </summary>
    public static partial class FastMap
    {
        #region maq 执行返回结果
        /// <summary>
        /// maq 执行返回结果
        /// </summary>
        public static List<T> Query<T>(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            key = key == null ? MapDb(name) : key;
            var config = db == null ? DataConfig.GetConfig(key) : db.config;
            if (config.IsUpdateCache)
                InstanceMap(config.Key);

            if (DbCache.Exists(config.CacheType, name.ToLower()))
            {
                var sql = MapXml.GetMapSql(name, ref param, db, key);
                isOutSql = isOutSql ? isOutSql : IsMapLog(name);

                BaseAop.AopMapBefore(name, sql, param, config,AopType.Map_List_Model);

                var result = FastRead.ExecuteSql<T>(sql, param, db, key, isOutSql);
                if (MapXml.MapIsForEach(name, config))
                {
                    db = db == null ? FastAop.FastAop.Resolve<IUnitOfWorK>().Contexts(key) : db;
                    for (var i = 1; i < MapXml.MapForEachCount(name, config); i++)
                    {
                        result = MapXml.MapForEach<T>(result, name, db, config, i);
                    }
                }

                BaseAop.AopMapAfter(name, sql, param, config, AopType.Map_List_Model, result);
                return result;
            }
            else
            {
                BaseAop.AopMapBefore(name, "", param, config, AopType.Map_List_Model);
                var data = new List<T>();
                BaseAop.AopMapAfter(name, "", param, config, AopType.Map_List_Model, data);

                return data;
            }
        }
        #endregion

        #region maq 执行返回结果 asy
        /// <summary>
        /// 执行sql asy
        /// </summary>
        public static Task<List<T>> QueryAsy<T>(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            return Task.Run(() =>
           {
               return Query<T>(name, param, db, key, isOutSql);
           });
        }
        #endregion

        #region maq 执行返回结果 lazy
        /// <summary>
        /// maq 执行返回结果 lazy
        /// </summary>
        public static Lazy<List<T>> QueryLazy<T>(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            return new Lazy<List<T>>(() => Query<T>(name, param, db, key, isOutSql));
        }
        #endregion

        #region maq 执行返回结果 lazy asy
        /// <summary>
        /// maq 执行返回结果 lazy asy
        /// </summary>
        public static Task<Lazy<List<T>>> QueryLazyAsy<T>(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            return Task.Run(() =>
           {
               return new Lazy<List<T>>(() => Query<T>(name, param, db, key, isOutSql));
           });
        }
        #endregion


        #region maq 执行写操作
        /// <summary>
        /// 执行写操作
        /// </summary>
        public static WriteReturn Write(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            key = key == null ? MapDb(name) : key;
            var config = db == null ? DataConfig.GetConfig(key) : db.config;
            if (config.IsUpdateCache)
                InstanceMap(config.Key);

            if (DbCache.Exists(config.CacheType, name.ToLower()))
            {
                var sql = MapXml.GetMapSql(name, ref param, db, key);
                isOutSql = isOutSql ? isOutSql : IsMapLog(name);

                BaseAop.AopMapBefore(name, sql, param, config, AopType.Map_Write);

                return FastWrite.ExecuteSql(sql, param, db, key, isOutSql);
            }
            else
            {
                BaseAop.AopMapBefore(name, "", param, config, AopType.Map_Write);
                var data = new WriteReturn();
                BaseAop.AopMapAfter(name, "", param, config,AopType.Map_Write,data);
                return data;
            }
        }
        #endregion

        #region maq 执行写操作 asy
        /// <summary>
        ///  maq 执行写操作 asy
        /// </summary>
        public static Task<WriteReturn> WriteAsy(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return Task.Run(() =>
           {
               return Write(name, param, db, key, isOutSql);
           });
        }
        #endregion

        #region maq 执行写操作 asy lazy
        /// <summary>
        /// maq 执行写操作 asy lazy
        /// </summary>
        public static Lazy<WriteReturn> WriteLazy(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return new Lazy<WriteReturn>(() => Write(name, param, db, key, isOutSql));
        }
        #endregion

        #region maq 执行写操作 asy lazy asy
        /// <summary>
        /// maq 执行写操作 asy lazy asy
        /// </summary>
        public static Task<Lazy<WriteReturn>> WriteLazyAsy(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return Task.Run(() =>
           {
               return new Lazy<WriteReturn>(() => Write(name, param, db, key, isOutSql));
           });
        }
        #endregion


        #region maq 执行返回 List<Dictionary<string, object>>
        /// <summary>
        /// maq 执行返回 List<Dictionary<string, object>>
        /// </summary>
        public static List<Dictionary<string, object>> Query(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            key = key == null ? MapDb(name) : key;
            var config = db == null ? DataConfig.GetConfig(key) : db.config;
            if (config.IsUpdateCache)
                InstanceMap(config.Key);

            if (DbCache.Exists(config.CacheType, name.ToLower()))
            {
                var sql = MapXml.GetMapSql(name, ref param, db, key);
                isOutSql = isOutSql ? isOutSql : IsMapLog(name);

                BaseAop.AopMapBefore(name, sql, param, config,AopType.Map_List_Dic);

                var result = FastRead.ExecuteSql(sql, param, db, key, isOutSql);

                if (MapXml.MapIsForEach(name, config))
                {
                    db = db == null ? FastAop.FastAop.Resolve<IUnitOfWorK>().Contexts(key) : db;
                    for (var i = 1; i < MapXml.MapForEachCount(name, config); i++)
                    {
                        result = MapXml.MapForEach(result, name, db, key, config, i);
                    }
                }

                BaseAop.AopMapAfter(name, sql, param, config, AopType.Map_List_Dic, result);
                return result;
            }
            else
            {
                BaseAop.AopMapBefore(name, "", param, config, AopType.Map_List_Dic);
                var data = new List<Dictionary<string, object>>();
                BaseAop.AopMapAfter(name, "", param, config, AopType.Map_List_Dic, data);
                return data;
            }
        }
        #endregion

        #region maq 执行返回 List<Dictionary<string, object>> asy
        /// <summary>
        /// 执行sql List<Dictionary<string, object>> asy
        /// </summary>
        public static Task<List<Dictionary<string, object>>> QueryAsy(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return Task.Run(() =>
           {
               return Query(name, param, db, key, isOutSql);
           });
        }
        #endregion

        #region maq 执行返回 List<Dictionary<string, object>> lazy
        /// <summary>
        /// maq 执行返回 List<Dictionary<string, object>> lazy
        /// </summary>
        public static Lazy<List<Dictionary<string, object>>> QueryLazy(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return new Lazy<List<Dictionary<string, object>>>(() => Query(name, param, db, key, isOutSql));
        }
        #endregion

        #region maq 执行返回 List<Dictionary<string, object>> lazy asy
        /// <summary>
        /// maq 执行返回 List<Dictionary<string, object>> lazy asy
        /// </summary>
        public static Task<Lazy<List<Dictionary<string, object>>>> ExecuteLazyMapAsy(string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return Task.Run(() =>
           {
               return new Lazy<List<Dictionary<string, object>>>(() => Query(name, param, db, key, isOutSql));
           });
        }
        #endregion


        #region 执行分页
        /// <summary>
        /// 执行分页 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static PageResult ExecuteSqlPage(PageModel pModel, string sql, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            var result = new DataReturn();
            var config = DataConfig.GetConfig(key);
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            db = db == null ? FastAop.FastAop.Resolve<IUnitOfWorK>().Contexts(key) : db;
            result = db.GetPageSql(pModel, sql, param,false);

            stopwatch.Stop();

            config.IsOutSql = config.IsOutSql ? config.IsOutSql : isOutSql;
            DbLog.LogSql(config.IsOutSql, result.Sql, config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            stopwatch = null;

            return result.PageResult;
        }
        #endregion

        #region maq 执行分页
        /// <summary>
        /// maq 执行分页
        /// </summary>
        public static PageResult QueryPage(PageModel pModel, string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            key = key == null ? MapDb(name) : key;
            var config = db == null ? DataConfig.GetConfig(key) : db.config;
            if (config.IsUpdateCache)
                InstanceMap(config.Key);
            if (DbCache.Exists(config.CacheType, name.ToLower()))
            {
                var sql = MapXml.GetMapSql(name, ref param, db, key);
                isOutSql = isOutSql ? isOutSql : IsMapLog(name);

                BaseAop.AopMapBefore(name, sql, param, config,AopType.Map_Page_Dic);

                var result = ExecuteSqlPage(pModel, sql, param, db, key, isOutSql);

                if (MapXml.MapIsForEach(name, config))
                {
                    db = db == null ? FastAop.FastAop.Resolve<IUnitOfWorK>().Contexts(key) : db;
                    for (var i = 1; i < MapXml.MapForEachCount(name, config); i++)
                    {
                        result.list = MapXml.MapForEach(result.list, name, db, key, config, i);
                    }
                }

                BaseAop.AopMapAfter(name, sql, param, config, AopType.Map_Page_Dic, result.list);
                return result;
            }
            else
            {
                BaseAop.AopMapBefore(name, "", param, config, AopType.Map_Page_Dic);
                var data = new PageResult();
                BaseAop.AopMapAfter(name, "", param, config, AopType.Map_Page_Dic, data.list);
                return data;
            }
        }
        #endregion

        #region maq 执行分页 asy
        /// <summary>
        /// 执行分页 asy
        /// </summary>
        public static Task<PageResult> QueryPageAsy(PageModel pModel, string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return Task.Run(() =>
           {
               return QueryPage(pModel, name, param, db, key, isOutSql);
           });
        }
        #endregion

        #region maq 执行分页 lazy
        /// <summary>
        /// maq 执行分页 lazy
        /// </summary>
        public static Lazy<PageResult> QueryPageLazy(PageModel pModel, string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return new Lazy<PageResult>(() => QueryPage(pModel, name, param, db, key, isOutSql));
        }
        #endregion

        #region maq 执行分页 lazy asy
        /// <summary>
        /// maq 执行分页lazy asy
        /// </summary>
        public static Task<Lazy<PageResult>> QueryPageLazyAsy(PageModel pModel, string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false)
        {
            return Task.Run(() =>
           {
               return new Lazy<PageResult>(() => QueryPage(pModel, name, param, db, key, isOutSql));
           });
        }
        #endregion


        #region 执行分页
        /// <summary>
        /// 执行分页 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static PageResult<T> ExecuteSqlPage<T>(PageModel pModel, string sql, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            var result = new DataReturn<T>();
            var config = DataConfig.GetConfig(key);
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(key))
                {
                    result = tempDb.GetPageSql<T>(pModel, sql, param,false);
                }
            }
            else
                result = db.GetPageSql<T>(pModel, sql, param,false);

            stopwatch.Stop();

            config.IsOutSql = config.IsOutSql ? config.IsOutSql : isOutSql;
            DbLog.LogSql(config.IsOutSql, result.sql, config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            stopwatch = null;

            return result.pageResult;
        }
        #endregion

        #region maq 执行分页
        /// <summary>
        /// maq 执行分页
        /// </summary>
        public static PageResult<T> QueryPage<T>(PageModel pModel, string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            key = key == null ? MapDb(name) : key;
            var config = db == null ? DataConfig.GetConfig(key) : db.config;
            if (config.IsUpdateCache)
                InstanceMap(config.Key);
            if (DbCache.Exists(config.CacheType, name.ToLower()))
            {
                var sql = MapXml.GetMapSql(name, ref param, db, key);
                isOutSql = isOutSql ? isOutSql : IsMapLog(name);

                BaseAop.AopMapBefore(name, sql, param, config,AopType.Map_Page_Model);

                var result = ExecuteSqlPage<T>(pModel, sql, param, db, key, isOutSql);

                if (MapXml.MapIsForEach(name, config))
                {
                    db = db == null ? FastAop.FastAop.Resolve<IUnitOfWorK>().Contexts(key) : db;
                    for (var i = 1; i < MapXml.MapForEachCount(name, config); i++)
                    {
                        result.list = MapXml.MapForEach<T>(result.list, name, db, config, i);
                    }
                }
                BaseAop.AopMapAfter(name, sql, param, config, AopType.Map_Page_Model, result.list);
                return result;
            }
            else
            {
                BaseAop.AopMapBefore(name, "", param, config, AopType.Map_Page_Model);
                var data = new PageResult<T>();
                BaseAop.AopMapAfter(name, "", param, config, AopType.Map_Page_Model, data.list);
                return data;
            }
        }
        #endregion

        #region maq 执行分页 asy
        /// <summary>
        /// 执行分页 asy
        /// </summary>
        public static Task<PageResult<T>> QueryPageAsy<T>(PageModel pModel, string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            return Task.Run(() =>
           {
               return QueryPage<T>(pModel, name, param, db, key, isOutSql);
           });
        }
        #endregion

        #region maq 执行分页 lazy
        /// <summary>
        /// maq 执行分页 lazy
        /// </summary>
        public static Lazy<PageResult<T>> QueryPageLazy<T>(PageModel pModel, string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            return new Lazy<PageResult<T>>(() => QueryPage<T>(pModel, name, param, db, key, isOutSql));
        }
        #endregion

        #region maq 执行分页 lazy asy
        /// <summary>
        /// maq 执行分页lazy asy
        /// </summary>
        public static Task<Lazy<PageResult<T>>> QueryPageLazyAsy<T>(PageModel pModel, string name, DbParameter[] param, DataContext db = null, string key = null, bool isOutSql = false) where T : class, new()
        {
            return Task.Run(() =>
           {
               return new Lazy<PageResult<T>>(() => QueryPage<T>(pModel, name, param, db, key, isOutSql));
           });
        }
        #endregion


        #region 服务实例
        /// <summary>
        /// 服务实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            var handler = new ProxyHandler();
            return FastProxy.Invoke<T>(handler);
        }
        #endregion


        #region 验证xml
        /// <summary>
        /// 验证xml
        /// </summary>
        /// <returns></returns>
        public static bool CheckMap(string xml, string dbKey = null)
        {
            var config = DataConfig.GetConfig(dbKey);
            var info = new FileInfo(xml);
            return MapXml.GetXmlList(info.FullName, "sqlMap", config).IsSuccess;
        }
        #endregion

        #region map 参数列表
        /// <summary>
        /// map 参数列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<string> MapParam(string name, ConfigModel config = null)
        {
            var cacheType = config == null ? DataConfig.GetConfig().CacheType : config.CacheType;
            return DbCache.Get<List<string>>(cacheType, string.Format("{0}.param", name.ToLower()));
        }
        #endregion

        #region map db
        /// <summary>
        /// map db
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MapDb(string name)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.db", name.ToLower()));
        }
        #endregion

        #region map type
        /// <summary>
        /// map db
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MapType(string name)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.type", name.ToLower()));
        }
        #endregion

        #region map view
        /// <summary>
        /// map view
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MapView(string name)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.view", name.ToLower()));
        }
        #endregion

        #region 是否存在map id
        /// <summary>
        /// 是否存在map id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsExists(string name)
        {
            return DbCache.Exists(DataConfig.GetConfig().CacheType, name.ToLower());
        }
        #endregion

        #region 获取map备注
        public static string MapRemark(string name)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.remark", name.ToLower()));
        }
        #endregion

        #region 获取map日志
        public static bool IsMapLog(string name)
        {
            return string.Compare( DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.log", name.ToLower())).ToStr(), "true", true) ==0;
        }
        #endregion

        #region 获取map参数备注
        public static string MapParamRemark(string name, string param)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.{1}.remark", name.ToLower(), param.ToLower()));
        }
        #endregion

        #region  获取api接口key
        /// <summary>
        /// 获取api接口key
        /// </summary>
        public static Dictionary<string, object> Api
        {
            get
            {
                return DbCache.Get<Dictionary<string, object>>(DataConfig.GetConfig().CacheType, "FastMap.Api") ?? new Dictionary<string, object>();
            }
        }
        #endregion

        #region 获取map验证必填
        /// <summary>
        /// 获取map验证必填
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string MapRequired(string name, string param)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.{1}.required", name.ToLower(), param.ToLower()));
        }
        #endregion

        #region 获取map验证长度
        /// <summary>
        /// 获取map验证长度
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string MapMaxlength(string name, string param)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.{1}.maxlength", name.ToLower(), param.ToLower()));
        }
        #endregion

        #region 获取map验证日期 
        /// <summary>
        /// 获取map验证日期
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string MapDate(string name, string param)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.{1}.date", name.ToLower(), param.ToLower()));
        }
        #endregion

        #region 获取map验证map
        /// <summary>
        /// 获取map验证map
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string MapCheck(string name, string param)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.{1}.checkmap", name.ToLower(), param.ToLower()));
        }
        #endregion

        #region 获取map验证map
        /// <summary>
        /// 获取map验证map
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string MapExists(string name, string param)
        {
            return DbCache.Get(DataConfig.GetConfig().CacheType, string.Format("{0}.{1}.existsmap", name.ToLower(), param.ToLower()));
        }
        #endregion

        #region 获取db配置文件
        /// <summary>
        /// 获取db配置文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ConfigModel DbConfig(string name)
        {
            return DataConfig.GetConfig(name);
        }
        #endregion
    }
}