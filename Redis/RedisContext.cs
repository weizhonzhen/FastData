using NServiceKit.Redis;
using Redis.Config;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Redis
{
    internal static class RedisContext
    {
        #region 获取上下文
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        public static PooledRedisClientManager GetContext
        {
            get
            {
                var key = "redisKey";

                if (HttpContext.Current == null)
                {
                    var context = CallContext.GetData(key) as PooledRedisClientManager;

                    if (context == null)
                    {
                        context = ClientInfo;
                        CallContext.SetData(key, context);
                    }

                    return context;
                }
                else
                {
                    var context = HttpContext.Current.Items[key] as PooledRedisClientManager;

                    if (context == null)
                    {
                        context = ClientInfo;
                        HttpContext.Current.Items[key] = ClientInfo;
                    }

                    return context;
                }
            }
        }
        #endregion

        #region 连接配置
        /// <summary>
        /// 连接配置
        /// </summary>
        /// <returns></returns>
        private static PooledRedisClientManager ClientInfo
        {
            get
            {
                //获取配置
                var config = RedisConfig.GetConfig();

                //redis连接
                return new PooledRedisClientManager(config.WriteServerList.Split(',')
                 , config.ReadServerList.Split(','), new RedisClientManagerConfig
                 {
                     MaxReadPoolSize = config.MaxReadPoolSize,
                     MaxWritePoolSize = config.MaxWritePoolSize,
                     AutoStart = config.AutoStart
                 });
            }
        }
        #endregion
    }
}
