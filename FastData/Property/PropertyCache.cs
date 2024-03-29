﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FastData.CacheModel;
using FastData.Base;
using FastData.Config;
using FastUntility.Base;

namespace FastData.Property
{
    /// <summary>
    /// 缓存类
    /// </summary>
    internal static class PropertyCache
    {
        #region 泛型缓存属性成员
        /// <summary>
        /// 泛型缓存属性成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<PropertyModel> GetPropertyInfo<T>(bool IsCache = true)
        {
            var list = new List<PropertyModel>();
            var key = string.Format("{0}.{1}", typeof(T).Namespace, typeof(T).Name);
            var config = DataConfig.GetConfig();

            if (IsCache)
            {
                if (DbCache.Exists(config.CacheType, key))
                    return DbCache.Get<List<PropertyModel>>(config.CacheType, key);
                else
                {
                    typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList().ForEach(a =>
                    {
                        if (!a.GetMethod.IsVirtual)
                        {
                            var temp = new PropertyModel();
                            temp.Name = a.Name;
                            temp.PropertyType = a.PropertyType;
                            list.Add(temp);

                        }
                    });

                    DbCache.Set<List<PropertyModel>>(config.CacheType, key, list);
                }
            }
            else
            {
                DbCache.Remove(config.CacheType, key);
                typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList().ForEach(a =>
                {
                    if (!a.GetMethod.IsVirtual)
                    {
                        var temp = new PropertyModel();
                        temp.Name = a.Name;
                        temp.PropertyType = a.PropertyType;
                        list.Add(temp);
                    }
                });
            }

            return list;
        }
        #endregion

        #region 缓存发属性成员
        public static List<PropertyModel> GetPropertyInfo(object model, bool IsCache = true)
        {
            var list = new List<PropertyModel>();
            var key = string.Format("{0}.{1}", model.GetType().Namespace, model.GetType().Name);
            var config = DataConfig.GetConfig();

            if (IsCache)
            {
                if (DbCache.Exists(config.CacheType, key))
                    return DbCache.Get<List<PropertyModel>>(config.CacheType, key);
                else
                {
                    model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList().ForEach(a =>
                    {
                        if (!a.GetMethod.IsVirtual)
                        {
                            var temp = new PropertyModel();
                            temp.Name = a.Name;
                            temp.PropertyType = a.PropertyType;
                            list.Add(temp);
                        }
                    });

                    DbCache.Set<List<PropertyModel>>(config.CacheType, key, list);
                }
            }
            else
            {
                model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList().ForEach(a =>
                {
                    if (!a.GetMethod.IsVirtual)
                    {
                        var temp = new PropertyModel();
                        temp.Name = a.Name;
                        temp.PropertyType = a.PropertyType;
                        list.Add(temp);
                    }
                });
            }

            return list;
        }
        #endregion

        #region 特性列
        /// <summary>
        /// 特性列
        /// </summary>
        public static List<ColumnModel> GetAttributesColumnInfo(string tableName, List<PropertyInfo> ListInfo)
        {
            var list = new List<ColumnModel>();

            ListInfo.ForEach(a => {
                var temp = new ColumnModel();
                temp.Name = a.Name;
                var paramList = GetPropertyInfo<ColumnModel>(true);

                a.CustomAttributes.ToList().ForEach(b => {
                    if (b.AttributeType.Name == typeof(ColumnAttribute).Name)
                    {
                        b.NamedArguments.ToList().ForEach(c => {
                            if (c.MemberName == "Name" && c.TypedValue.Value != null)
                                temp.Name = c.TypedValue.Value.ToStr();

                            if (paramList.Exists(p => string.Compare(p.Name, c.MemberName, true) == 0))
                                BaseEmit.Set(temp, c.MemberName, c.TypedValue.Value);
                        });
                    }
                });

                if (temp.IsKey && temp.IsNull)
                    temp.IsNull = false;

                list.Add(temp);
            });

            return list;
        }
        #endregion

        #region 特性表
        /// <summary>
        /// 特性表
        /// </summary>
        public static TableAttribute GetAttributesTableInfo(List<Attribute> listAttribute)
        {
            var result = new TableAttribute();

            listAttribute.ForEach(a => {
                if (a is TableAttribute)
                    result = a as TableAttribute;
            });

            return result;
        }
        #endregion
    }
}
