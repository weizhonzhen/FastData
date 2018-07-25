using System;
using System.ServiceModel;

namespace Fast.Untility.Host
{
    /// <summary>
    /// 标签：2015.9.6，魏中针
    /// 说明：WCF宿主绑定类
    /// </summary>
    public static class WcfBinding
    {
        #region WebHttpBinding 模式
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：WebHttpBinding 模式
        /// </summary>
        /// <returns></returns>
        public static WebHttpBinding HttpBinding()
        {
            var webBinding = new WebHttpBinding();
            //webBinding.AllowCookies = true;
            webBinding.TransferMode = TransferMode.Streamed;
            webBinding.Security.Mode = WebHttpSecurityMode.None;
            webBinding.ReceiveTimeout = TimeSpan.MaxValue;
            webBinding.SendTimeout = TimeSpan.MaxValue;
            webBinding.MaxReceivedMessageSize = long.MaxValue;
            webBinding.Name = "http";

            return webBinding;
        }
        #endregion

        #region BasicHttpBinding 模式
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：BasicHttpBinding 模式
        /// </summary>
        /// <returns></returns>
        public static BasicHttpBinding BasicBinding()
        {
            var baseBinding = new BasicHttpBinding();
            baseBinding.TransferMode = TransferMode.Streamed;
            baseBinding.ReceiveTimeout = TimeSpan.MaxValue;
            baseBinding.SendTimeout = TimeSpan.MaxValue;
            baseBinding.MaxReceivedMessageSize = int.MaxValue;
            baseBinding.Name = "basic";

            return baseBinding;
        }
        #endregion

        #region WSHttpBinding 模式
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：WSHttpBinding 模式
        /// </summary>
        /// <returns></returns>
        public static WSHttpBinding WsBinding()
        {
            var WsHttpBinding = new WSHttpBinding();
            WsHttpBinding.Security.Mode = SecurityMode.None;
            WsHttpBinding.Name = "ws";

            return WsHttpBinding;
        }
        #endregion

        #region WSDualHttpBinding 模式
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：WSDualHttpBinding 模式
        /// </summary>
        /// <returns></returns>
        public static WSDualHttpBinding WsDualBinding()
        {
            var WsDualHttpBinding = new WSDualHttpBinding();
            WsDualHttpBinding.Security.Mode = WSDualHttpSecurityMode.None;
            WsDualHttpBinding.ReceiveTimeout = TimeSpan.MaxValue;
            WsDualHttpBinding.SendTimeout = TimeSpan.MaxValue;
            WsDualHttpBinding.MaxReceivedMessageSize = int.MaxValue;
            WsDualHttpBinding.Name = "wsdual";

            return WsDualHttpBinding;
        }
        #endregion

        #region WSFederationHttpBinding 模式
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：WSFederationHttpBinding 模式
        /// </summary>
        /// <returns></returns>
        public static WSFederationHttpBinding WsFederationBinding()
        {
            var WsFederationBinding = new WSFederationHttpBinding();
            WsFederationBinding.Security.Mode = WSFederationHttpSecurityMode.None;
            WsFederationBinding.ReceiveTimeout = TimeSpan.MaxValue;
            WsFederationBinding.SendTimeout = TimeSpan.MaxValue;
            WsFederationBinding.MaxReceivedMessageSize = int.MaxValue;
            WsFederationBinding.Name = "wsf";

            return WsFederationBinding;
        }
        #endregion

        #region 流 NetTcpBinding 模式
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：流 NetTcpBinding 模式
        /// </summary>
        /// <returns></returns>
        public static NetTcpBinding TcpBinding()
        {
            var TcpBinding = new NetTcpBinding();
            TcpBinding.TransferMode = TransferMode.Streamed;
            TcpBinding.Security.Mode = SecurityMode.None;
            TcpBinding.ReceiveTimeout = TimeSpan.MaxValue;
            TcpBinding.SendTimeout = TimeSpan.MaxValue;
            TcpBinding.MaxReceivedMessageSize = int.MaxValue;
            TcpBinding.MaxConnections = int.MaxValue;
            TcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            TcpBinding.Name = "tcp";

            return TcpBinding;
        }
        #endregion

        #region NetNamedPipeBinding 模式
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：NetNamedPipeBinding 模式
        /// </summary>
        /// <returns></returns>
        public static NetNamedPipeBinding NetNameBinding()
        {
            var NetBinding = new NetNamedPipeBinding();
            NetBinding.TransferMode = TransferMode.Streamed;
            NetBinding.Security.Mode = NetNamedPipeSecurityMode.None;
            NetBinding.ReceiveTimeout = TimeSpan.MaxValue;
            NetBinding.SendTimeout = TimeSpan.MaxValue;
            NetBinding.MaxReceivedMessageSize = int.MaxValue;
            NetBinding.MaxConnections = int.MaxValue;
            NetBinding.Name = "net";

            return NetBinding;
        }
        #endregion

        #region msmq 模式
        /// <summary>
        /// 标签：2015.7.13，魏中针
        /// 说明：msmq 模式
        /// </summary>
        /// <returns></returns>
        public static NetMsmqBinding MsgBinding()
        {
            var MsgBinding = new NetMsmqBinding();
            MsgBinding.Security.Mode =NetMsmqSecurityMode.None;
            MsgBinding.ReceiveTimeout = TimeSpan.MaxValue;
            MsgBinding.SendTimeout = TimeSpan.MaxValue;
            MsgBinding.MaxReceivedMessageSize = int.MaxValue;
            MsgBinding.QueueTransferProtocol = QueueTransferProtocol.Srmp;
            MsgBinding.Name = "msg";
            MsgBinding.ExactlyOnce = false;
            MsgBinding.Durable = true;
            
            return MsgBinding;
        }
        #endregion

        /*
            WebHttpBinding: 用于为通过 HTTP 请求,其中：
                                            WebGet 是读取(get), WebInvoke 是操作(delete,put,post,get)
                                            RequestFormat = WebMessageFormat.Json 获取格式 json、xml
                                            ResponseFormat = WebMessageFormat.Json 返回格式 json、xml
                                            UriTemplate = "/Select/{id}" 路由
         
            BasicHttpBinding: 最简单的绑定类型，通常用于 Web Services。使用 HTTP 协议，Text/XML 编码方式。
            WSHttpBinding: 比 BasicHttpBinding 更加安全，通常用于 non-duplex 服务通讯。
            WSDualHttpBinding: 和 WSHttpBinding 相比，它支持 duplex(双向通道) 类型的服务。
            WSFederationHttpBinding: 支持 WS-Federation 安全通讯协议。
            NetTcpBinding: 效率最高，安全的跨机器通讯方式。
            NetNamedPipeBinding: 安全、可靠、高效的单机服务通讯方式(适用于跨进程，不能使用于不同机器间)
            NetMsmqBinding: 使用消息队列在不同机器间进行通讯。
            MsmqIntegrationBinding: 一个用于wcf与现有msmq程序进行安全通讯的binding
            NetPeerTcpBinding:  一个支持安全的，多机交互的binding(使用 P2P 协议在多机器间通讯)
            
            WCF在通信过程中有三种模式：请求应答、单向、双工通信。
                        
            单向：[OperationContract(IsOneWay = true)]

            双工通信：客户端与服务端可以互相调用，在web场景中，因为受nat，firewall等的限制，一般来说服务器无法直接访问客户端，
            所以一般不会把wsDualHttpBinding用在internet的场景中。
         
            支持回调的绑定有4种：WSDualHttpBinding、NetTcpBinding、NetNamedPipeBinding、NetPeerTcpBinding
          
        */
    }
}
