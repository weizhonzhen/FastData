using System;

namespace FastData.Property
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FastWriteAttribute : Attribute
    {
        public string sql { get; set; }

        public string dbKey { get; set; }
    }
}
