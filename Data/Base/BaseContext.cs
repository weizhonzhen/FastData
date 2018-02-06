using Data.Context;
using Data.Model;
using System.Runtime.Remoting.Messaging;

namespace Data.Base
{
    internal static class BaseContext
    {
        #region 获取读上下文
        /// <summary>
        /// 获取读上下文
        /// </summary>
        /// <returns></returns>
        public static ReadContext GetReadContext(DataQuery item)
        {
            return new ReadContext(item.Key, item.Config); 

            var dataKey = item.Key ?? "LambdaReadDb";

            var context = CallContext.GetData(dataKey) as ReadContext;

            if (context == null)
            {
                context = new ReadContext(item.Key, item.Config);
                CallContext.SetData(dataKey, context);
            }

            return context;
        }
        #endregion

        #region 获取读上下文
        /// <summary>
        /// 获取读上下文
        /// </summary>
        /// <returns></returns>
        public static ReadContext GetReadContext(string key)
        {
            return new ReadContext(key);

            var dataKey = key ?? "LambdaReadDb";
            
            var context = CallContext.GetData(dataKey) as ReadContext;

            if (context == null)
            {
                context = new ReadContext(key);
                CallContext.SetData(dataKey, context);
            }

            return context;
        }
        #endregion

        #region 获取写上下文
        /// <summary>
        /// 获取写上下文
        /// </summary>
        /// <returns></returns>
        public static WriteContext GetWriteContext(string key)
        {
            return new WriteContext(key);

            var dataKey = key ?? "LambdaWriteDb";

            var context = CallContext.GetData(dataKey) as WriteContext;

            if (context == null)
            {
                context = new WriteContext(key);
                CallContext.SetData(dataKey, context);
            }

            return context;
        }
        #endregion
    }
}
