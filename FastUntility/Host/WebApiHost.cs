using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.IO;
using Untility.Base;

namespace Untility.Host
{
    public sealed class WebApiHost
    {
        #region webapi 宿主类
        /// <summary>
        /// webapi 宿主类
        /// </summary>
        public static HttpSelfHostServer GetHost(string url)
        {
            try
            {
                // 配置一个自宿主 http 服务
                var HostConfig = new HttpSelfHostConfiguration(new Uri(url));

                // 配置 http 服务的路由
                HostConfig.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                var host = new HttpSelfHostServer(HostConfig);

                host.OpenAsync().Wait();

                return host;
            }
            catch(HttpException ex)
            {
                BaseLog.SaveLog(ex.ToString(), "WebApiHost_exp.txt"); return null;
            }
        }
        #endregion
    }
}
