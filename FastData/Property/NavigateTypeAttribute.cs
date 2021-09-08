using System;

namespace FastData.Property
{
    /// <summary>
    /// 字典导航属性
    /// </summary>
    public class NavigateTypeAttribute : Attribute
    {
        /// <summary>
        /// 类型
        /// </summary>
        public System.Type Type { get; set; }
    }
}
