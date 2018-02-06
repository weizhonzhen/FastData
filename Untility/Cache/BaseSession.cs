using System.Web;

namespace Untility.Cache
{
    /// <summary>
    /// 标签：2015.7.13，魏中针
    /// 说明：session 操作
    /// </summary>
    public static class BaseSession
    {
        #region 添加Session
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static bool Add(string SessionName, object Value, int iExpires)
        {
            if (string.IsNullOrEmpty(SessionName))
                return false;
            
            HttpContext.Current.Session[SessionName] = Value;
            HttpContext.Current.Session.Timeout = iExpires;
            return true;
        }
        #endregion
        
        #region 添加Session
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static bool AddAsync(HttpContextBase context,string SessionName, object Value, int iExpires)
        {
            if (string.IsNullOrEmpty(SessionName))
                return false;
            
            context.Session[SessionName] = Value;
            context.Session.Timeout = iExpires;
            return true;
        }
        #endregion

        #region 添加Session组
        /// <summary>
        /// 添加Session组
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValues">Session值数组</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static bool Adds(string SessionName, object[] Values, int iExpires)
        {
            if (string.IsNullOrEmpty(SessionName))
                return false;
            
            HttpContext.Current.Session[SessionName] = Values;
            HttpContext.Current.Session.Timeout = iExpires;
            return true;
        }
        #endregion

        #region 添加Session组
        /// <summary>
        /// 添加Session组
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValues">Session值数组</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static bool AddsAsync(HttpContextBase context,string SessionName, object[] Values, int iExpires)
        {
            if (string.IsNullOrEmpty(SessionName))
                return false;
            
            context.Session[SessionName] = Values;
            context.Session.Timeout = iExpires;
            return true;
        }
        #endregion

        #region 读取某个Session对象值
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string Get(string SessionName)
        {
            if (HttpContext.Current.Session[SessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[SessionName].ToString();
            }
        }
        #endregion

        #region 读取某个Session对象值
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string GetAsync(HttpContextBase context,string SessionName)
        {
            if (context.Session[SessionName] == null)
            {
                return null;
            }
            else
            {
                return context.Session[SessionName].ToString();
            }
        }
        #endregion

        #region 读取某个Session对象值数组
        /// <summary>
        /// 读取某个Session对象值数组
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值数组</returns>
        public static string[] Gets(string SessionName)
        {
            if (string.IsNullOrEmpty(SessionName))
                return null;
            
            if (HttpContext.Current.Session[SessionName] == null)
            {
                return null;
            }
            else
            {
                return (string[])HttpContext.Current.Session[SessionName];
            }
        }
        #endregion
        
        #region 读取某个Session对象值数组
        /// <summary>
        /// 读取某个Session对象值数组
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值数组</returns>
        public static string[] Gets(HttpContext context,string SessionName)
        {
            if (string.IsNullOrEmpty(SessionName))
                return null;
            
            if (context.Session[SessionName] == null)
            {
                return null;
            }
            else
            {
                return (string[])context.Session[SessionName];
            }
        }
        #endregion

        #region 删除某个Session对象
        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Del(string SessionName)
        {
            if (!string.IsNullOrEmpty(SessionName))
                HttpContext.Current.Session[SessionName] = null;
        }
        #endregion

        #region 删除某个Session对象
        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void DelAsync(HttpContextBase context, string SessionName)
        {
            if (!string.IsNullOrEmpty(SessionName))
                context.Session[SessionName] = null;
        }
        #endregion
    }
}
