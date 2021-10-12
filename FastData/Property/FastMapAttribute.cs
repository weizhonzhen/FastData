using System;

namespace FastData.Property
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FastMapAttribute : Attribute
    {
        public string xml { get; set; }

        public string dbKey { get; set; }

        public bool isPage { get; set; }
    }
}
