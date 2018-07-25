namespace Fast.Base
{
    internal static class Config
    {
        /// <summary>
        /// 表缓存键
        /// </summary>
        public static readonly string TableKey = "Fast.Cache.Table.{0}";

        /// <summary>
        /// 设计模式
        /// </summary>
        public static readonly string CodeFirst = "CodeFirst";

        /// <summary>
        /// 设计模式
        /// </summary>
        public static readonly string DbFirst = "DbFirst";
    }

    internal static class RedisDb
    {
        /// <summary>
        /// 属性
        /// </summary>
        public static readonly int Properties = 1;

        /// <summary>
        /// map xml
        /// </summary>
        public static readonly int Xml = 0;

        /// <summary>
        /// config
        /// </summary>
        public static readonly int Config = 0;
    }
}
