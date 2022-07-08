using FastData.Base;
using FastData.Context;
using FastData.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FastUntility.Page;
using System.Data;

namespace FastData
{
    public class FastQueryable<T> where T : class, new()
    {
        public DataQuery Query = new DataQuery();

        #region 查询join
        /// <summary>
        /// 查询join
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="T1">泛型</typeparam>
        /// <param name="joinType">left join,right join,inner join</param>
        /// <param name="item"></param>
        /// <param name="predicate">条件</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        private FastQueryable<T> JoinType<T1>(string joinType, Expression<Func<T, T1, bool>> predicate, Expression<Func<T1, object>> field = null, bool isDblink = false) where T1 : class, new()
        {
            var queryField = BaseField.QueryField<T, T1>(predicate, field, Query.Config);
            Query.Field.Add(queryField.Field);
            Query.AsName.AddRange(queryField.AsName);

            var condtion = VisitExpression.LambdaWhere<T, T1>(predicate, Query.Config);
            Query.Predicate.Add(condtion);
            Query.Table.Add(string.Format("{2} {0}{3} {1}", typeof(T1).Name, predicate.Parameters[1].Name
            , joinType, isDblink && !string.IsNullOrEmpty(Query.Config.DbLinkName) ? string.Format("@", Query.Config.DbLinkName) : ""));

            Query.TableName.Add(typeof(T1).Name);
            return this;
        }
        #endregion

        #region 查询left join
        /// <summary>
        /// 查询left join
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="item"></param>
        /// <param name="predicate"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public FastQueryable<T> LeftJoin<T1>(Expression<Func<T, T1, bool>> predicate, Expression<Func<T1, object>> field = null, bool isDblink = false) where T1 : class, new()
        {
            return JoinType("left join", predicate, field);
        }
        #endregion

        #region 查询right join
        /// <summary>
        /// 查询right join
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="item"></param>
        /// <param name="predicate"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public FastQueryable<T> RightJoin<T1>(Expression<Func<T, T1, bool>> predicate, Expression<Func<T1, object>> field = null, bool isDblink = false) where T1 : class, new()
        {
            return JoinType("right join", predicate, field);
        }
        #endregion

        #region 查询inner join
        /// <summary>
        /// 查询inner join
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="item"></param>
        /// <param name="predicate"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public FastQueryable<T> InnerJoin<T1>(Expression<Func<T, T1, bool>> predicate, Expression<Func<T1, object>> field = null, bool isDblink = false) where T1 : class, new()
        {
            return JoinType("inner join", predicate, field);
        }
        #endregion

        #region 查询order by
        /// <summary>
        /// 查询order by
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public FastQueryable<T> OrderBy(Expression<Func<T, object>> field, bool isDesc = true)
        {
            var orderBy = BaseField.OrderBy(field, Query.Config, isDesc);
            Query.OrderBy.AddRange(orderBy);
            return this;
        }
        #endregion

        #region 查询group by
        /// <summary>
        /// 查询group by
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public FastQueryable<T> GroupBy(Expression<Func<T, object>> field)
        {
            var groupBy = BaseField.GroupBy(field, Query.Config);
            Query.GroupBy.AddRange(groupBy);
            return this;
        }
        #endregion

        #region 查询take
        /// <summary>
        /// 查询take
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public FastQueryable<T> Take(int i)
        {
            Query.Take = i;
            return this;
        }
        #endregion


        #region 返回list
        /// <summary>
        /// 返回list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<T> ToList(DataContext db = null, bool isOutSql = false)
        {
            var stopwatch = new Stopwatch();
            var result = new DataReturn<T>();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.list;

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetList<T>(Query);
                }
            }
            else
                result = db.GetList<T>(Query);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.list;
        }
        #endregion

        #region 返回list asy
        /// <summary>
        /// 返回list asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<List<T>> ToListAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() => {
                return ToList(db, isOutSql);
            });
        }
        #endregion

        #region 返回lazy<list>
        /// <summary>
        /// 返回lazy<list>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Lazy<List<T>> ToLazyList(DataContext db = null, bool isOutSql = false)
        {
            return new Lazy<List<T>>(() => ToList(db, isOutSql));
        }
        #endregion

        #region 返回lazy<list> asy
        /// <summary>
        /// 返回lazy<list> asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Lazy<List<T>>> ToLazyListAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return new Lazy<List<T>>(() => ToList(db, isOutSql));
            });
        }
        #endregion


        #region 返回list<R>
        /// <summary>
        /// 返回list<R>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<R> ToList<R>(DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            var stopwatch = new Stopwatch();
            var result = new DataReturn<R>();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.list;

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetList<R>(Query);
                }
            }
            else
                result = db.GetList<R>(Query);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.list;
        }
        #endregion

        #region 返回list<R> asy
        /// <summary>
        /// 返回list<R> asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<List<R>> ToListAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return Task.Run(() =>
            {
                return ToList<R>(db, isOutSql);
            });
        }
        #endregion

        #region 返回lazy<list<R>>
        /// <summary>
        /// 返回lazy<list<R>>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Lazy<List<R>> ToLazyList<R>(DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return new Lazy<List<R>>(() => ToList<R>(db, isOutSql));
        }
        #endregion

        #region 返回lazy<list<R>> asy
        /// <summary>
        /// 返回lazy<list<R>> asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Lazy<List<R>>> ToLazyListAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return Task.Run(() =>
            {
                return new Lazy<List<R>>(() => ToList<R>(db, isOutSql));
            });
        }
        #endregion


        #region 返回json
        /// <summary>
        /// 返回json
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string ToJson(DataContext db = null, bool isOutSql = false)
        {
            var result = new DataReturn();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.Json;

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetJson(Query);
                }
            }
            else
                result = db.GetJson(Query);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.Sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.Json;
        }
        #endregion

        #region 返回json asy
        /// <summary>
        /// 返回json asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<string> ToJsonAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return ToJson(db, isOutSql);
            });
        }
        #endregion

        #region 返回lazy<json>
        /// <summary>
        /// 返回lazy<json>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Lazy<string> ToLazyJson(DataContext db = null, bool isOutSql = false)
        {
            return new Lazy<string>(() => ToJson(db, isOutSql));
        }
        #endregion

        #region 返回lazy<json> asy
        /// <summary>
        /// 返回lazy<json> asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Lazy<string>> ToLazyJsonAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return new Lazy<string>(() => ToJson(db, isOutSql));
            });
        }
        #endregion


        #region 返回item
        /// <summary>
        /// 返回item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public T ToItem(DataContext db = null, bool isOutSql = false)
        {
            var result = new DataReturn<T>();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.item;

            stopwatch.Start();

            Query.Take = 1;

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetList<T>(Query);
                }
            }
            else
                result = db.GetList<T>(Query);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.item;
        }
        #endregion

        #region 返回item asy
        /// <summary>
        /// 返回item asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<T> ToItemAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return ToItem(db, isOutSql);
            });
        }
        #endregion

        #region 返回Lazy<item>
        /// <summary>
        /// 返回Lazy<item>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Lazy<T> ToLazyItem(DataContext db = null, bool isOutSql = false)
        {
            return new Lazy<T>(() => ToItem(db, isOutSql));
        }
        #endregion

        #region 返回Lazy<item> asy
        /// <summary>
        /// 返回Lazy<item> asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Lazy<T>> ToLazyItemAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return new Lazy<T>(() => ToItem(db, isOutSql));
            });
        }
        #endregion


        #region 返回item<R>
        /// <summary>
        /// 返回item<R>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public R ToItem<R>(DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            var result = new DataReturn<R>();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.item;

            stopwatch.Start();

            Query.Take = 1;

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetList<R>(Query);
                }
            }
            else
                result = db.GetList<R>(Query);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.item;
        }
        #endregion

        #region 返回item<R> asy
        /// <summary>
        /// 返回item<R> asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<R> ToItemAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return Task.Run(() =>
            {
                return ToItem<R>(db, isOutSql);
            });
        }
        #endregion

        #region 返回Lazy<item<R>>
        /// <summary>
        /// 返回Lazy<item<R>>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Lazy<R> ToLazyItem<R>(DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return new Lazy<R>(() => ToItem<R>(db, isOutSql));
        }
        #endregion

        #region 返回Lazy<item<R>> asy
        /// <summary>
        /// 返回Lazy<item<R>> asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Lazy<R>> ToLazyItemAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return Task.Run(() =>
            {
                return new Lazy<R>(() => ToItem<R>(db, isOutSql));
            });
        }
        #endregion


        #region 返回条数
        /// <summary>
        /// 返回条数
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int ToCount(DataContext db = null, bool isOutSql = false)
        {
            var result = new DataReturn();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.Count;

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetCount(Query);
                }
            }
            else
                result = db.GetCount(Query);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.Sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);

            return result.Count;
        }
        #endregion

        #region 返回条数 asy
        /// <summary>
        /// 返回条数 asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> ToCountAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return ToCount(db, isOutSql);
            });
        }
        #endregion


        #region 返回分页
        /// <summary>
        /// 返回分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public PageResult<T> ToPage(PageModel pModel, DataContext db = null, bool isOutSql = false)
        {
            var result = new DataReturn<T>();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.pageResult;

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetPage<T>(Query, pModel);
                }
            }
            else
                result = db.GetPage<T>(Query, pModel);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.pageResult;
        }
        #endregion

        #region 返回分页 asy
        /// <summary>
        /// 返回分页 asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Task<PageResult<T>> ToPageAsy(PageModel pModel, DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return ToPage(pModel, db, isOutSql);
            });
        }
        #endregion

        #region 返回分页lazy
        /// <summary>
        /// 返回分页lazy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Lazy<PageResult<T>> ToLazyPage(PageModel pModel, DataContext db = null, bool isOutSql = false)
        {
            return new Lazy<PageResult<T>>(() => ToPage(pModel, db, isOutSql));
        }
        #endregion

        #region 返回分页lazy asy
        /// <summary>
        /// 返回分页lazy asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Task<Lazy<PageResult<T>>> ToLazyPageAsy(PageModel pModel, DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return new Lazy<PageResult<T>>(() => ToPage(pModel, db, isOutSql));
            });
        }
        #endregion


        #region 返回分页<R>
        /// <summary>
        /// 返回分页<R>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public PageResult<R> ToPage<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            var result = new DataReturn<R>();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.pageResult;

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetPage<R>(Query, pModel);
                }
            }
            else
                result = db.GetPage<R>(Query, pModel);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.pageResult;
        }
        #endregion

        #region 返回分页<R> asy
        /// <summary>
        /// 返回分页<R> asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Task<PageResult<R>> ToPageAsy<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return Task.Run(() =>
            {
                return ToPage<R>(pModel, db, isOutSql);
            });
        }
        #endregion

        #region 返回分页lazy<R>
        /// <summary>
        /// 返回分页lazy<R>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Lazy<PageResult<R>> ToLazyPage<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return new Lazy<PageResult<R>>(() => ToPage<R>(pModel, db, isOutSql));
        }
        #endregion

        #region 返回分页lazy<R> asy
        /// <summary>
        /// 返回分页lazy<R> asy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Task<Lazy<PageResult<R>>> ToLazyPageAsy<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new()
        {
            return Task.Run(() =>
            {
                return new Lazy<PageResult<R>>(() => ToPage<R>(pModel, db, isOutSql));
            });
        }
        #endregion


        #region 返回分页Dictionary<string, object>
        /// <summary>
        /// 返回分页Dictionary<string, object>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public PageResult ToPageDic(PageModel pModel, DataContext db = null, bool isOutSql = false)
        {
            var result = new DataReturn();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.PageResult;

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetPage(Query, pModel);
                }
            }
            else
                result = db.GetPage(Query, pModel);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.Sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.PageResult;
        }
        #endregion

        #region 返回分页Dictionary<string, object> asy
        /// <summary>
        /// 返回分页Dictionary<string, object> asy
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Task<PageResult> ToPageDicAsy(PageModel pModel, DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return ToPageDic(pModel, db, isOutSql);
            });
        }
        #endregion

        #region 返回分页Dictionary<string, object> lazy
        /// <summary>
        /// 返回分页Dictionary<string, object> lazy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Lazy<PageResult> ToLazyPageDic(PageModel pModel, DataContext db = null, bool isOutSql = false)
        {
            return new Lazy<PageResult>(() => ToPageDic(pModel, db, isOutSql));
        }
        #endregion

        #region 返回分页Dictionary<string, object> lazy asy
        /// <summary>
        /// 返回分页Dictionary<string, object> lazy asy
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public Task<Lazy<PageResult>> ToLazyPageDicAsy(PageModel pModel, DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return new Lazy<PageResult>(() => ToPageDic(pModel, db, isOutSql));
            });
        }
        #endregion


        #region DataTable
        /// <summary>
        /// DataTable
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataContext db = null, bool isOutSql = false)
        {
            var result = new DataReturn();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.Table;

            stopwatch.Start();
            Query.Take = 1;

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetDataTable(Query);
                }
            }
            else
                result = db.GetDataTable(Query);

            stopwatch.Stop();

            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.Sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.Table;
        }
        #endregion

        #region DataTable asy
        /// <summary>
        /// DataTable asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<DataTable> ToDataTableAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return ToDataTable(db, isOutSql);
            });
        }
        #endregion

        #region DataTable lazy
        /// <summary>
        /// DataTable lazy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Lazy<DataTable> ToLazyDataTable(DataContext db = null, bool isOutSql = false)
        {
            return new Lazy<DataTable>(() => ToDataTable(db, isOutSql));
        }
        #endregion

        #region DataTable lazy asy
        /// <summary>
        /// DataTable lazy asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Lazy<DataTable>> ToLazyDataTableAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return new Lazy<DataTable>(() => ToDataTable(db, isOutSql));
            });
        }
        #endregion


        #region 返回List<Dictionary<string, object>>
        /// <summary>
        /// 返回List<Dictionary<string, object>>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> ToDics(DataContext db = null, bool isOutSql = false)
        {
            var result = new DataReturn();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.DicList;

            stopwatch.Start();

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetDic(Query);
                }
            }
            else
                result = db.GetDic(Query);

            stopwatch.Stop();
            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.Sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.DicList;
        }
        #endregion

        #region 返回List<Dictionary<string, object>> asy
        /// <summary>
        /// 返回List<Dictionary<string, object>> asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<List<Dictionary<string, object>>> ToDicsAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return ToDics(db, isOutSql);
            });
        }
        #endregion

        #region 返回lazy<List<Dictionary<string, object>>>
        /// <summary>
        /// 返回lazy<List<Dictionary<string, object>>>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Lazy<List<Dictionary<string, object>>> ToLazyDics(DataContext db = null, bool isOutSql = false)
        {
            return new Lazy<List<Dictionary<string, object>>>(() => ToDics(db, isOutSql));
        }
        #endregion

        #region 返回lazy<List<Dictionary<string, object>>> asy
        /// <summary>
        /// 返回lazy<List<Dictionary<string, object>>> asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Lazy<List<Dictionary<string, object>>>> ToLazyDicsAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return new Lazy<List<Dictionary<string, object>>>(() => ToDics(db, isOutSql));
            });
        }
        #endregion


        #region Dictionary<string, object>
        /// <summary>
        /// Dictionary<string, object>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Dictionary<string, object> ToDic(DataContext db = null, bool isOutSql = false)
        {
            var result = new DataReturn();
            var stopwatch = new Stopwatch();

            if (Query.Predicate.Exists(a => a.IsSuccess == false))
                return result.Dic;

            stopwatch.Start();
            Query.Take = 1;

            if (db == null)
            {
                using (var tempDb = new DataContext(Query.Key))
                {
                    result = tempDb.GetDic(Query);
                }
            }
            else
                result = db.GetDic(Query);

            stopwatch.Stop();

            Query.Config.IsOutSql = Query.Config.IsOutSql ? Query.Config.IsOutSql : isOutSql;
            DbLog.LogSql(Query.Config.IsOutSql, result.Sql, Query.Config.DbType, stopwatch.Elapsed.TotalMilliseconds);
            return result.Dic;
        }
        #endregion

        #region Dictionary<string, object> asy
        /// <summary>
        /// Dictionary<string, object> asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Dictionary<string, object>> ToDicAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return ToDic(db, isOutSql);
            });
        }
        #endregion

        #region Dictionary<string, object>
        /// <summary>
        /// Dictionary<string, object>>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Lazy<Dictionary<string, object>> ToLazyDic(DataContext db = null, bool isOutSql = false)
        {
            return new Lazy<Dictionary<string, object>>(() => ToDic(db, isOutSql));
        }
        #endregion

        #region Dictionary<string, object> asy
        /// <summary>
        /// Dictionary<string, object> asy
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<Lazy<Dictionary<string, object>>> ToLazyDicAsy(DataContext db = null, bool isOutSql = false)
        {
            return Task.Run(() =>
            {
                return new Lazy<Dictionary<string, object>>(() => ToDic(db, isOutSql));
            });
        }
        #endregion
    }
}