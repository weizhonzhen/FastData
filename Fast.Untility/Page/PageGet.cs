using System.Linq;
using System.Web;
using System.Web.Routing;
using Fast.Untility.Base;

namespace Fast.Untility.Page
{
    /// <summary>
    /// 分页相关方法
    /// </summary>
    public static class PageGet
    {
        #region 获取当前页数
        /// <summary>
        /// 获取当前页数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int PageIdAsync(HttpContextBase context, string routeKey, int refValue = 1)
        {
            return RouteTable.Routes.GetRouteData(context).Values.ToList().Find(a => a.Key == routeKey).Value.ToStr().ToInt(refValue);
        }
        #endregion

        #region 获取页面地址
        /// <summary>
        /// 获取页面地址
        /// </summary>
        /// <param name="context"></param>
        /// <param name="routeKey"></param>
        /// <returns></returns>
        public static string PageUrlAsync(HttpContextBase context, string routeKey)
        {
            var url = string.Format("http://{0}:{1}", context.Request.Url.Host, context.Request.Url.Port);
            var list = RouteTable.Routes.GetRouteData(context).Values.ToList().FindAll(a => a.Value.ToStr() != "");

            foreach (var item in list)
            {
                url = string.Format("{0}/{1}", url, item.Key == routeKey ? "{0}" : item.Value);
            }

            return url;
        }
        #endregion

        #region 获取路由值
        /// <summary>
        /// 获取路由值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string RouteValueAsync(HttpContextBase context, string routeKey)
        {
            return RouteTable.Routes.GetRouteData(context).Values.ToList().Find(a => a.Key == routeKey).Value.ToStr();
        }
        #endregion
    }
}
