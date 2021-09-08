using System;

namespace FastData.Property
{
    /// <summary>
    /// 导航属性
    /// </summary>
    public class NavigateAttribute : Attribute
    {
        /// <summary>
        /// 附加条件
        /// </summary>
        public string Appand { get; set; }

        /// <summary>
        /// 关联列名
        /// </summary>
        public string Name { get; set; }
    }
}
