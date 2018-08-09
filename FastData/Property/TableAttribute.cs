using System;

namespace FastData.Property
{
    /// <summary>
    /// 表属性
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string Comments { get; set; }
    }
}
