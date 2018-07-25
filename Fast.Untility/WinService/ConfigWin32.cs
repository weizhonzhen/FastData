
namespace Fast.Untility.WinService
{
    /// <summary>
    /// windows 参数
    /// </summary>
    internal static class ConfigWin32
    {
        /// <summary>
        /// 时区 
        /// </summary>
        public readonly static string TimeZone = "Win32_TimeZone";
        
        /// <summary>
        /// 驱动程序 
        /// </summary>
        public readonly static string SystemDriver = "Win32_SystemDriver";
        
        /// <summary>
        /// 磁盘分区 
        /// </summary>
        public readonly static string DiskPartition = "Win32_DiskPartition";
        
        /// <summary>
        /// 逻辑磁盘 
        /// </summary>
        public readonly static string LogicalDisk = "Win32_LogicalDisk";
        
        /// <summary>
        /// 逻辑磁盘所在分区及始末位置 
        /// </summary>
        public readonly static string LogicalDiskToPartition = "Win32_LogicalDiskToPartition";

        /// <summary>
        /// 逻辑内存配置 
        /// </summary>
        public readonly static string LogicalMemoryConfiguration = "Win32_LogicalMemoryConfiguration";
        
        /// <summary>
        /// 系统页文件信息 
        /// </summary>
        public readonly static string PageFile = "Win32_PageFile";
        
        /// <summary>
        /// 页文件设置 
        /// </summary>
        public readonly static string PageFileSetting = "Win32_PageFileSetting";
        
        /// <summary>
        /// 系统启动配置 
        /// </summary>
        public readonly static string BootConfiguration = "Win32_BootConfiguration";
        
        /// <summary>
        /// 计算机信息简要 
        /// </summary>
        public readonly static string ComputerSystem = "Win32_ComputerSystem";
        
        /// <summary>
        /// 操作系统信息 
        /// </summary>
        public readonly static string OperatingSystem = "Win32_OperatingSystem";
        
        /// <summary>
        /// 系统自动启动程序 
        /// </summary>
        public readonly static string StartupCommand = "Win32_StartupCommand";
        
        /// <summary>
        /// 系统安装的服务 
        /// </summary>
        public readonly static string Service = "Win32_Service";

        /// <summary>
        /// 系统管理组 
        /// </summary>
        public readonly static string Group = "Win32_Group";
        
        /// <summary>
        /// 系统管理组 
        /// </summary>
        public readonly static string GroupUser = "Win32_GroupUser";
        
        /// <summary>
        /// 系统组帐号 
        /// </summary>
        public readonly static string UserAccount = "Win32_UserAccount";

        /// <summary>
        /// 系统进程 
        /// </summary>
        public readonly static string Process = "Win32_Process";
                
        /// <summary>
        /// 系统进程 
        /// </summary>
        public readonly static string Thread = "Win32_Thread";
        
        /// <summary>
        /// 共享 
        /// </summary>
        public readonly static string Share = "Win32_Share";
          
        /// <summary>
        /// 已安装的网络客户端 
        /// </summary>
        public readonly static string NetworkClient = "Win32_NetworkClient";
        
        /// <summary>
        /// 已安装的网络协议 
        /// </summary>
        public readonly static string NetworkProtocol = "Win32_NetworkProtocol";        
    }
}
