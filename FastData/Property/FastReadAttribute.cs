using System;

namespace FastData.Property
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FastReadAttribute : Attribute
    {
        public string sql { get; set; }

        public string dbKey { get; set; }

        public bool isPage { get; set; }
    }
}