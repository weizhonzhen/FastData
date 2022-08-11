using System;

namespace FastData.Property
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FastWriteAttribute : Attribute
    {
        /// <summary>
        /// sql
        /// </summary>
        public string sql { get; set; }

        /// <summary>
        /// 数据库key
        /// </summary>
        public string dbKey { get; set; }
    }
}
