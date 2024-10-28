using FastData.Model;
using FastData.Property;
using FastUntility.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;

namespace FastData.Base
{
    /// <summary>
    /// datareader操作类
    /// </summary>
    internal static class BaseDataReader
    {
        #region to list
        /// <summary>
        ///  to list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(DbDataReader dr, ConfigModel config, List<string> field = null) where T : class, new()
        {
            var list = new List<T>();
            var colList = new List<string>();

            if (dr == null)
                return list;

            var propertyList = PropertyCache.GetPropertyInfo<T>(config.IsPropertyCache);

            if (dr.HasRows)
                colList = GetCol(dr);

            while (dr.Read())
            {
                var dic = new Dictionary<string, object>();
                var item = new T();

                if (field == null || field.Count == 0)
                {
                    colList.ForEach(a =>
                    {
                        if (dr[a] is DBNull)
                            return;
                        else
                        {
                            var info = propertyList.Find(b => string.Compare(b.Name, a, true) == 0);

                            if (info == null)
                                return;

                            if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                                return;

                            dic.Add(info.Name, dr[a]);
                        }
                    });
                }
                else
                {
                    colList.ForEach(a =>
                    {
                        if (dr[a] is DBNull)
                            return;
                        else
                        {
                            if (!field.Exists(b => string.Compare(a, b, true) == 0))
                                return;

                            var info = propertyList.Find(b => string.Compare(b.Name, a, true) == 0);

                            if (info == null)
                                return;

                            if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                                return;

                            dic.Add(info.Name, dr[a]);
                        }
                    });
                }

                BaseEmit.Set(item, dic);
                list.Add(item);
            }
                        
            return list;
        }
        #endregion

        #region to list
        /// <summary>
        ///  to list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static IList ToList(System.Type type, Object model, DbDataReader dr, ConfigModel config, List<string> field = null)
        {
            var list = Activator.CreateInstance(type);
            var colList = new List<string>();

            if (dr == null)
                return null;

            var propertyList = PropertyCache.GetPropertyInfo(model, config.IsPropertyCache);

            if (dr.HasRows)
                colList = GetCol(dr);

            while (dr.Read())
            {
                var item = Activator.CreateInstance(model.GetType());
                var dic = new Dictionary<string, object>();

                if (field == null || field.Count == 0)
                {
                    colList.ForEach(a =>
                    {
                        if (dr[a] is DBNull)
                            return;
                        else
                        {
                            var info = propertyList.Find(b => string.Compare(b.Name, a, true) == 0);

                            if (info == null)
                                return;

                            if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                                return;

                            dic.Add(info.Name, dr[a]);
                        }
                    });
                }
                else
                {
                    colList.ForEach(a =>
                    {
                        if (dr[a] is DBNull)
                            return;
                        else
                        {
                            if (!field.Exists(b => string.Compare(a, b, true) == 0))
                                return;

                            var info = propertyList.Find(b => string.Compare(b.Name, a, true) == 0);

                            if (info == null)
                                return;

                            if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                                return;

                            dic.Add(info.Name, dr[a]);
                        }
                    });
                }

                BaseEmit.Set(item, dic);
                list.GetType().GetMethods().ToList().ForEach(m =>
                {
                    if (m.Name == "Add")
                        BaseEmit.Invoke(list, m, new object[] { item });
                });
            }

            return (IList)list;
        }
        #endregion

        #region to model
        /// <summary>
        /// to model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dr"></param>
        /// <param name="config"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Object ToModel(Object model, DbDataReader dr, ConfigModel config, List<string> field = null)
        {
            var result = Activator.CreateInstance(model.GetType());
            var colList = new List<string>();

            if (dr == null)
                return null;

            if (dr.HasRows)
                colList = GetCol(dr);

            var propertyList = PropertyCache.GetPropertyInfo(model, config.IsPropertyCache);

            while (dr.Read())
            {
                var dic = new Dictionary<string, object>();
                if (field == null || field.Count == 0)
                {
                    colList.ForEach(a =>
                    {
                        if (dr[a] is DBNull)
                            return;
                        else
                        {
                            var info = propertyList.Find(b => string.Compare(b.Name, a, true) == 0);

                            if (info == null)
                                return;

                            if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                                return;

                            dic.Add(info.Name, dr[a]);
                        }
                    });
                }
                else
                {
                    colList.ForEach(a =>
                    {
                        if (dr[a] is DBNull)
                            return;
                        else
                        {
                            if (!field.Exists(b => string.Compare(a, b, true) == 0))
                                return;

                            var info = propertyList.Find(b => string.Compare(b.Name, a, true) == 0);

                            if (info == null)
                                return;

                            if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))
                                return;

                            dic.Add(info.Name, dr[a]);
                        }
                    });
                }
                BaseEmit.Set(result, dic);
            }

            return result;
        }
        #endregion

        #region to dyns
        /// <summary>
        ///  to dyns
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static List<dynamic> ToDyns(DbDataReader dr, ConfigModel config)
        {
            List<dynamic> list = new List<dynamic>();
            var colList = new List<string>();

            if (dr == null)
                return list;

            if (dr.HasRows)
                colList = GetCol(dr);

            while (dr.Read())
            {
                dynamic item = new ExpandoObject();
                var dic = (IDictionary<string, object>)item;

                foreach (var key in colList)
                {
                    dic[key] = dr[key];
                }

                list.Add(item);
            }

            return list;
        }
        #endregion

        #region get datareader col
        private static List<string> GetCol(DbDataReader dr)
        {
            var list = new List<string>();
            for(var i=0;i<dr.FieldCount;i++)
            {
                list.Add(dr.GetName(i));
            }
            return list;
        }
        #endregion
    }
}
