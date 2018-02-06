using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Untility.Host
{
    /// <summary>
    /// 吞吐量配置实类
    /// </summary>
    public sealed class ServiceThrottlingAttribute : Attribute, IServiceBehavior
    {
        private ServiceThrottlingBehavior Throttling;

        /// <summary>
        /// 实例化 ServiceThrottlingAttribute 新的实例
        /// </summary>
        public ServiceThrottlingAttribute()
            : this(64, System.Int32.MaxValue, 10)
        {

        }

        /// <summary>
        /// 实例化 ServiceThrottlingAttribute 新的实例
        /// </summary>
        /// <param name="maxConcurrentCalls">指定整个 System.ServiceModel.ServiceHost 中正在处理的最多消息数</param>
        /// <param name="maxConcurrentInstances">指定服务中可以一次执行的最多 System.ServiceModel.InstanceContext 对象数</param>
        /// <param name="maxConcurrentSessions">指定 System.ServiceModel.ServiceHost 对象可一次接受的最多会话数的值</param>
        public ServiceThrottlingAttribute(int maxConcurrentCalls, int maxConcurrentInstances, int maxConcurrentSessions)
        {
            this.Throttling = new ServiceThrottlingBehavior();
            this.MaxConcurrentCalls = maxConcurrentCalls;
            this.MaxConcurrentInstances = maxConcurrentInstances;
            this.MaxConcurrentSessions = maxConcurrentSessions;
        }

        /// <summary>
        /// 获取或设置一个值，该值指定整个 System.ServiceModel.ServiceHost 中正在处理的最多消息数(默认64)。
        /// </summary>
        public int MaxConcurrentCalls
        {
            get { return this.Throttling.MaxConcurrentCalls; }
            set { this.Throttling.MaxConcurrentCalls = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指定服务中可以一次执行的最多 System.ServiceModel.InstanceContext 对象数(默认System.Int32.MaxValue)。
        /// </summary>
        public int MaxConcurrentInstances
        {
            get { return this.Throttling.MaxConcurrentInstances; }
            set { this.Throttling.MaxConcurrentInstances = value; }
        }

        /// <summary>
        /// 获取或设置一个指定 System.ServiceModel.ServiceHost 对象可一次接受的最多会话数的值(默认10)。
        /// </summary>
        public int MaxConcurrentSessions
        {
            get { return this.Throttling.MaxConcurrentSessions; }
            set { this.Throttling.MaxConcurrentSessions = value; }
        }

        #region IServiceBehavior 成员

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            ServiceThrottlingBehavior CurrentThrottling = serviceDescription.Behaviors.Find<ServiceThrottlingBehavior>();
            if (CurrentThrottling == null)
            {
                serviceDescription.Behaviors.Add(this.Throttling);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
