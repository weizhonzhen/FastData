using System.Configuration;

namespace Data.Config
{
    /// <summary>
    /// 连接串子节点
    /// </summary>
    internal class MapElement : ConfigurationElement
    {
        #region 文件地址
        /// <summary>
        /// 文件地址
        /// </summary>
        [ConfigurationProperty("File", IsRequired = true)]
        public string File
        {
            get
            {
                return base["File"].ToString();
            }
            set
            {
                base["File"] = value;
            }
        }
        #endregion        
    }
}
