using FastData.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace FastData.Aop
{
    internal static class BaseAop
    {
        #region Aop Map Before
        /// <summary>
        /// Aop Map Before
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <param name="config"></param>
        public static void AopMapBefore(string mapName, string sql, DbParameter[] param, ConfigModel config, AopType type)
        {
            if (FastMap.fastAop != null)
            {
                var context = new MapBeforeContext();
                context.mapName = mapName;
                context.sql = sql;
                context.type = type;

                if (param != null)
                    context.param = param.ToList();

                context.dbType = config.DbType;

                FastMap.fastAop.MapBefore(context);
            }
        }
        #endregion

        #region Aop Map After
        /// <summary>
        /// Aop Map After
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <param name="config"></param>
        public static void AopMapAfter(string mapName, string sql, DbParameter[] param, ConfigModel config, AopType type, object data)
        {
            if (FastMap.fastAop != null)
            {
                var context = new MapAfterContext();
                context.mapName = mapName;
                context.sql = sql;
                context.type = type;

                if (param != null)
                    context.param = param.ToList();

                context.dbType = config.DbType;
                context.result = data;

                FastMap.fastAop.MapAfter(context);
            }
        }
        #endregion

        #region Aop Before
        /// <summary>
        /// Aop Before
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <param name="config"></param>
        public static void AopBefore(List<string> tableName, string sql, List<DbParameter> param, ConfigModel config, bool isRead, AopType type,object model = null)
        {
            if (FastMap.fastAop != null)
            {
                var context = new BeforeContext();

                if (tableName != null)
                    context.tableName = tableName;

                context.sql = sql;

                if (param != null)
                    context.param = param;

                context.dbType = config.DbType;
                context.isRead = isRead;
                context.isWrite = !isRead;
                context.model = model;

                FastMap.fastAop.Before(context);
            }
        }
        #endregion

        #region Aop After
        /// <summary>
        /// Aop After
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <param name="config"></param>
        public static void AopAfter(List<string> tableName, string sql, List<DbParameter> param, ConfigModel config, bool isRead, AopType type, object result, object model = null)
        {
            if (FastMap.fastAop != null)
            {
                var context = new AfterContext();

                if (tableName != null)
                    context.tableName = tableName;

                context.sql = sql;

                if (param != null)
                    context.param = param;

                context.dbType = config.DbType;
                context.isRead = isRead;
                context.isWrite = !isRead;
                context.result = result;
                context.model = model;

                FastMap.fastAop.After(context);
            }
        }
        #endregion

        #region aop Exception
        /// <summary>
        /// aop Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="name"></param>
        public static void AopException(Exception ex, string name, ConfigModel config, AopType type,object model=null)
        {
            if (FastMap.fastAop != null)
            {
                var context = new ExceptionContext();
                context.dbType = context.dbType;
                context.ex = ex;
                context.name = name;
                context.type = type;
                context.model = model;
                FastMap.fastAop.Exception(context);
            }
        }
        #endregion
    }
}
