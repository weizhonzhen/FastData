﻿using FastData.Context;
using FastUntility.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FastData.Repository
{
    public abstract class IQueryable<T> where T : class, new()
    {
        public abstract IQueryable<T> AndIf(bool condtion, Expression<Func<T, bool>> predicate);

        public abstract IQueryable<T> And( Expression<Func<T, bool>> predicate);

        public abstract IQueryable<T> OrIf(bool condtion, Expression<Func<T, bool>> predicate);

        public abstract IQueryable<T> Or(Expression<Func<T, bool>> predicate);

        public abstract IQueryable<T,T1> LeftJoin<T1>(Expression<Func<T, T1, bool>> predicate, Expression<Func<T1, object>> field = null, bool isDblink = false);

        public abstract IQueryable<T, T1> RightJoin<T1>(Expression<Func<T, T1, bool>> predicate, Expression<Func<T1, object>> field = null, bool isDblink = false) where T1 : class, new();

        public abstract IQueryable<T, T1> InnerJoin<T1>(Expression<Func<T, T1, bool>> predicate, Expression<Func<T1, object>> field = null, bool isDblink = false) where T1 : class, new();

        public abstract IQueryable<T> OrderBy(Expression<Func<T, object>> field, bool isDesc = true);

        public abstract IQueryable<T> GroupBy(Expression<Func<T, object>> field);

        public abstract IQueryable<T> Take(int i);

        public abstract IQueryable<T> Filter(bool isFilter = true);

        public abstract IQueryable<T> Navigate(bool isNavigate = true);

        public abstract string ToJson(DataContext db = null, bool isOutSql = false);

        public abstract Task<string> ToJsonAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<string> ToLazyJson(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<string>> ToLazyJsonAsy(DataContext db = null, bool isOutSql = false);

        public abstract T ToItem(DataContext db = null, bool isOutSql = false);

        public abstract Task<T> ToItemAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<T> ToLazyItem(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<T>> ToLazyItemAsy(DataContext db = null, bool isOutSql = false);

        public abstract R ToItem<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<R> ToItemAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Lazy<R> ToLazyItem<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<Lazy<R>> ToLazyItemAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract int ToCount(DataContext db = null, bool isOutSql = false);

        public abstract Task<int> ToCountAsy(DataContext db = null, bool isOutSql = false);

        public abstract PageResult<T> ToPage(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<PageResult<T>> ToPageAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Lazy<PageResult<T>> ToLazyPage(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<PageResult<T>>> ToLazyPageAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract PageResult<R> ToPage<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<PageResult<R>> ToPageAsy<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Lazy<PageResult<R>> ToLazyPage<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<Lazy<PageResult<R>>> ToLazyPageAsy<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract PageResult ToPageDic(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<PageResult> ToPageDicAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Lazy<PageResult> ToLazyPageDic(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<PageResult>> ToLazyPageDicAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract DataTable ToDataTable(DataContext db = null, bool isOutSql = false);

        public abstract Task<DataTable> ToDataTableAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<DataTable> ToLazyDataTable(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<DataTable>> ToLazyDataTableAsy(DataContext db = null, bool isOutSql = false);

        public abstract List<Dictionary<string, object>> ToDics(DataContext db = null, bool isOutSql = false);

        public abstract Task<List<Dictionary<string, object>>> ToDicsAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<List<Dictionary<string, object>>> ToLazyDics(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<List<Dictionary<string, object>>>> ToLazyDicsAsy(DataContext db = null, bool isOutSql = false);

        public abstract Dictionary<string, object> ToDic(DataContext db = null, bool isOutSql = false);

        public abstract Task<Dictionary<string, object>> ToDicAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<Dictionary<string, object>> ToLazyDic(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<Dictionary<string, object>>> ToLazyDicAsy(DataContext db = null, bool isOutSql = false);

        public abstract List<T> ToList(DataContext db = null, bool isOutSql = false);

        public abstract Task<List<T>> ToListAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<List<T>> ToLazyList(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<List<T>>> ToLazyListAsy(DataContext db = null, bool isOutSql = false);

        public abstract List<R> ToList<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<List<R>> ToListAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Lazy<List<R>> ToLazyList<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<Lazy<List<R>>> ToLazyListAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new();
        public abstract dynamic ToDyn(DataContext db = null, bool isOutSql = false);

        public abstract Task<dynamic> ToDynAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<dynamic> ToLazyDyn(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<dynamic>> ToLazyDynAsy(DataContext db = null, bool isOutSql = false);

        public abstract List<dynamic> ToDyns(DataContext db = null, bool isOutSql = false);

        public abstract Task<List<dynamic>> ToDynsAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<List<dynamic>> ToLazyDyns(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<List<dynamic>>> ToLazyDynsAsy(DataContext db = null, bool isOutSql = false);

        public abstract PageResultDyn ToDynPage(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<PageResultDyn> ToDynPageAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Lazy<PageResultDyn> ToLazyDynPage(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<PageResultDyn>> ToLazyDynPageAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);
    }

    public abstract class IQueryable<T,T1> where T : class, new()
    {
        public abstract IQueryable<T,T1> AndIf(bool condtion, Expression<Func<T, T1, bool>> predicate);

        public abstract IQueryable<T, T1> And(Expression<Func<T, T1, bool>> predicate);

        public abstract IQueryable<T, T1> OrIf(bool condtion, Expression<Func<T, T1, bool>> predicate);

        public abstract IQueryable<T, T1> Or(Expression<Func<T, T1, bool>> predicate);

        public abstract IQueryable<T, T1> OrderBy(Expression<Func<T, T1, object>> field, bool isDesc = true);

        public abstract IQueryable<T, T1> GroupBy(Expression<Func<T, T1, object>> field);

        public abstract IQueryable<T, T1> Take(int i);

        public abstract IQueryable<T, T1> Filter(bool isFilter = true);

        public abstract IQueryable<T, T1> Navigate(bool isNavigate = true);

        public abstract string ToJson(DataContext db = null, bool isOutSql = false);

        public abstract Task<string> ToJsonAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<string> ToLazyJson(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<string>> ToLazyJsonAsy(DataContext db = null, bool isOutSql = false);

        public abstract R ToItem<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<R> ToItemAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Lazy<R> ToLazyItem<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<Lazy<R>> ToLazyItemAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract int ToCount(DataContext db = null, bool isOutSql = false);

        public abstract Task<int> ToCountAsy(DataContext db = null, bool isOutSql = false);

        public abstract PageResult<R> ToPage<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<PageResult<R>> ToPageAsy<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Lazy<PageResult<R>> ToLazyPage<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<Lazy<PageResult<R>>> ToLazyPageAsy<R>(PageModel pModel, DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract PageResult ToPageDic(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<PageResult> ToPageDicAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Lazy<PageResult> ToLazyPageDic(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<PageResult>> ToLazyPageDicAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract DataTable ToDataTable(DataContext db = null, bool isOutSql = false);

        public abstract Task<DataTable> ToDataTableAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<DataTable> ToLazyDataTable(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<DataTable>> ToLazyDataTableAsy(DataContext db = null, bool isOutSql = false);

        public abstract List<Dictionary<string, object>> ToDics(DataContext db = null, bool isOutSql = false);

        public abstract Task<List<Dictionary<string, object>>> ToDicsAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<List<Dictionary<string, object>>> ToLazyDics(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<List<Dictionary<string, object>>>> ToLazyDicsAsy(DataContext db = null, bool isOutSql = false);

        public abstract Dictionary<string, object> ToDic(DataContext db = null, bool isOutSql = false);

        public abstract Task<Dictionary<string, object>> ToDicAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<Dictionary<string, object>> ToLazyDic(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<Dictionary<string, object>>> ToLazyDicAsy(DataContext db = null, bool isOutSql = false);

        public abstract List<R> ToList<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<List<R>> ToListAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Lazy<List<R>> ToLazyList<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract Task<Lazy<List<R>>> ToLazyListAsy<R>(DataContext db = null, bool isOutSql = false) where R : class, new();

        public abstract dynamic ToDyn(DataContext db = null, bool isOutSql = false);

        public abstract Task<dynamic> ToDynAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<dynamic> ToLazyDyn(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<dynamic>> ToLazyDynAsy(DataContext db = null, bool isOutSql = false);

        public abstract List<dynamic> ToDyns(DataContext db = null, bool isOutSql = false);

        public abstract Task<List<dynamic>> ToDynsAsy(DataContext db = null, bool isOutSql = false);

        public abstract Lazy<List<dynamic>> ToLazyDyns(DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<List<dynamic>>> ToLazyDynsAsy(DataContext db = null, bool isOutSql = false);

        public abstract PageResultDyn ToDynPage(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<PageResultDyn> ToDynPageAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Lazy<PageResultDyn> ToLazyDynPage(PageModel pModel, DataContext db = null, bool isOutSql = false);

        public abstract Task<Lazy<PageResultDyn>> ToLazyDynPageAsy(PageModel pModel, DataContext db = null, bool isOutSql = false);
    }
}
