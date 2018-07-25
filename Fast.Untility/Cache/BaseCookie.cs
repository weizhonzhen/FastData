using System;
using System.Web;

namespace Fast.Untility.Cache
{
    /// <summary>
    /// 标签：2015.7.13，魏中针
    /// 说明：cookies操作类
    /// </summary>
    public static class BaseCookie
    {
        #region 写cookie值
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static bool WriteAsync(HttpContextBase context,string strName, string strValue, int days = 1)
        {
            if (string.IsNullOrEmpty(strName))
                return false;
            var cookie = new HttpCookie(strName);
            cookie.HttpOnly = true;
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddDays(days);
            context.Response.Cookies.Add(cookie);
            return true;
        }
        #endregion

        #region 写cookie值
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static bool Write(string strName, string strValue, int days = 1)
        {
            if (string.IsNullOrEmpty(strName))
                return false;
            var cookie = new HttpCookie(strName);
            cookie.HttpOnly = true;
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddDays(days);
            HttpContext.Current.Response.Cookies.Add(cookie);
            return true;
        }
        #endregion

        #region 读cookie值
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string Read(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value;
            }
            else
                return "";
        }
        #endregion

        #region 读cookie值
        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string ReadAsync(HttpContextBase context,string strName)
        {
            if (context.Request.Cookies != null && context.Request.Cookies[strName] != null)
            {
                return context.Request.Cookies[strName].Value;
            }
            else
                return "";
        }
        #endregion

        #region Cookie 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name">名称</param>
        public static void Remove(string name)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[name] != null)
            {
                var myCookie = new HttpCookie(name);
                myCookie.Expires = DateTime.Now.AddMinutes(-1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
        #endregion

        #region Cookie 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="name">名称</param>
        public static void RemoveAsync(HttpContextBase context,string name)
        {
            if (context.Request.Cookies != null && context.Request.Cookies[name] != null)
            {
                var myCookie = new HttpCookie(name);
                myCookie.Expires = DateTime.Now.AddMinutes(-1);
                context.Response.Cookies.Add(myCookie);
            }
        }
        #endregion
    }
}
