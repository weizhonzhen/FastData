using System.Configuration;

namespace Fast.Config
{
    /// <summary>
    /// 连接串节点集
    /// </summary>
    internal class CollectionConfig:ConfigurationElementCollection
    {
        /// <summary>
        /// 节点集节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ElementConfig();
        }

        /// <summary>
        /// 节点集 键
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ElementConfig)element).ConnStr;
        }
    }
}
