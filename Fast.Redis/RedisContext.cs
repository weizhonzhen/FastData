using NServiceKit.Redis;
using Fast.Redis.Config;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Fast.Redis
{
    internal static class RedisContext
    {
        #region 获取上下文
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns></returns>
        public static PooledRedisClientManager GetContext(int db = 0)
        {
            var key = string.Format("redisKey{0}", db);

            if (HttpContext.Current == null)
            {
                var context = CallContext.GetData(key) as PooledRedisClientManager;

                if (context == null)
                {
                    context = ClientInfo(db);
                    CallContext.SetData(key, context);
                }

                return context;
            }
            else
            {
                var context = HttpContext.Current.Items[key] as PooledRedisClientManager;

                if (context == null)
                {
                    context = ClientInfo(db);
                    HttpContext.Current.Items[key] = ClientInfo(db);
                }

                return context;
            }
        }
        #endregion

        #region 连接配置
        /// <summary>
        /// 连接配置
        /// </summary>
        /// <returns></returns>
        private static PooledRedisClientManager ClientInfo(int db = 0)
        {
            //获取配置
            var config = RedisConfig.GetConfig();

            //redis连接
            return new PooledRedisClientManager(config.WriteServerList.Split(',')
             , config.ReadServerList.Split(','), new RedisClientManagerConfig
             {
                 DefaultDb = db,
                 MaxReadPoolSize = config.MaxReadPoolSize,
                 MaxWritePoolSize = config.MaxWritePoolSize,
                 AutoStart = config.AutoStart
             });
        }
        #endregion
    }
}
