using System;
using System.Web;
using System.Management;

namespace Fast.Untility.WinService
{
    /// <summary>
    /// 读取硬件信息 
    /// </summary>
    public static class Win32PSI
    {
        #region 获取CPU序列号
        /// <summary>
        /// 获取CPU序列号
        /// </summary>
        /// <returns></returns>
        public static string CpuInfo()
        {
            string retValue = "";

            //信息模型
            var SysInfo = new ManagementClass(ConfigWin32PSI.Cpu);

            //信息集合
            ManagementObjectCollection moc = SysInfo.GetInstances();

            foreach (var mo in moc)
            {
                retValue += mo.Properties["ProcessorId"].Value.ToString();
            }

            return retValue;
        }
        #endregion

        #region 获取网卡硬件地址
        /// <summary>
        /// 获取网卡硬件地址
        /// </summary>
        /// <returns></returns>
        public static string MacAddress()
        {
            string MacAddress = "";
            ManagementClass mc = new ManagementClass(ConfigWin32PSI.NetworkAdapterConfiguration);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (var mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    MacAddress = mo["MacAddress"].ToString();
                    break;
                }
            }

            return MacAddress;
        }
        #endregion

        #region 操作系统的登录用户名
        /// <summary>
        /// 操作系统的登录用户名
        /// </summary>
        /// <returns></returns>
        public static string SysUserName()
        {
            return Environment.UserName;
        }
        #endregion

        #region 操作系统类型
        /// <summary>
        /// 操作系统类型
        /// </summary>
        /// <returns></returns>
        public static string SystemType()
        {
            string SystemType = "";
            ManagementClass mc = new ManagementClass(ConfigWin32.ComputerSystem);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                SystemType += mo["SystemType"].ToString();
            }

            return SystemType;
        }
        #endregion

        #region 获取计算机名
        /// <summary>
        /// 获取计算机名
        /// </summary>
        /// <returns></returns> 
        public static string ComputerName()
        {
            return Environment.MachineName;
        }
        #endregion

        #region 物理内存
        /// <summary>
        /// 物理内存
        /// </summary>
        /// <returns></returns>
        public static string PhysicalMemory()
        {
            string PhysicalMemory = "";
            ManagementClass mc = new ManagementClass(ConfigWin32.ComputerSystem);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                PhysicalMemory += mo["TotalPhysicalMemory"].ToString();
            }

            return PhysicalMemory;
        }
        #endregion

        #region 获取IP地址
        /// <summary>
        /// 说明：获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string IPAsync(HttpContextBase context)
        {
            try
            {
                string userIP = "";
                if (context.Request.ServerVariables["HTTP_VIA"] == null)
                {
                    userIP = context.Request.UserHostAddress;
                }
                else
                {
                    userIP = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }

                if (userIP == "::1")
                    userIP = "127.0.0.1";
                return userIP;
            }
            catch
            {
                return "127.0.0.1";
            }
        }
        #endregion
        
        #region 获取IP地址
        /// <summary>
        /// 说明：获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string IP()
        {
            try
            {
                string userIP = "";
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null)
                {
                    userIP = HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    userIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }

                if (userIP == "::1")
                    userIP = "127.0.0.1";
                return userIP;
            }
            catch
            {
                return "127.0.0.1";
            }
        }
        #endregion
    }
}
