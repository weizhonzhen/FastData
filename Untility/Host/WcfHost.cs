using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Messaging;
using Untility.Base;

namespace Untility.Host
{
    /// <summary>
    /// 标签：2015.9.6，魏中针
    /// 说明：创建WCF宿主类
    /// </summary>
    public static class WcfHost
    {
        #region 创建WCF宿主类
        /// <summary>
        /// 标签：2015.9.6，魏中针
        /// 说明：宿主类
        /// </summary>
        /// <param name="Url">网址</param>
        /// <param name="TypeName">类，typeof(test)</param>
        /// <param name="ITypeName">接口类，typeof(itest)</param>
        /// <param name="bind">绑定模式，BindingModel类提供</param>
        /// <param name="TcpUrl">TCP访问地址</param>
        /// <param name="QueueName">队列名称</param>
        /// <returns></returns>
        public static ServiceHost GetHost(string BaseUrl, Type TypeName, Type ITypeName, Binding bind,string NetUrl="", string QueueName = "")
        {
            try
            {
                //创建宿主
                var host = new ServiceHost(TypeName, new Uri(BaseUrl));
                ServiceEndpoint endpoint = null;

                if (bind.Name == "msg" || bind.Name == "tcp")
                    endpoint = host.AddServiceEndpoint(ITypeName, bind, new Uri(NetUrl));
                else
                    endpoint = host.AddServiceEndpoint(ITypeName, bind, new Uri(BaseUrl));

                //webhttpbinding
                if (bind.Name == "http")
                {
                    var whb = new WebHttpBehavior();

                    //显示接口参数帮助页
                    whb.HelpEnabled = true; 

                    endpoint.Behaviors.Add(whb);
                }

                //创建队列
                if (bind.Name == "msg" && !MessageQueue.Exists(".\\private$\\" + QueueName))
                {
                    MessageQueue.Create(".\\private$\\" + QueueName, true);
                }                               

                host.Open();

                return host;
            }
            catch(Exception ex)
            {
                BaseLog.SaveLog(ex.ToString(), "wcfHost_exp.txt");
                return null;
            }
        }
        #endregion
    }
}
