using System;
using System.Collections;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Configuration.Install;

namespace FastUntility.WinService
{
    /// <summary>
    /// windows 服务
    /// </summary>
    public static class Service
    {
        #region 服务重起
        /// <summary>
        /// 服务重起
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="IsForced">是否强制重起服务</param>
        public static bool ReStatService(string serviceName, bool IsForced = false)
        {
            var returnValue = true;
            try
            {
                var mySevice = new ServiceController(serviceName);

                if (IsForced)
                {
                    mySevice.Stop();
                    mySevice.WaitForStatus(ServiceControllerStatus.Stopped);
                    mySevice.Start();
                    mySevice.WaitForStatus(ServiceControllerStatus.Running);
                }
                else
                {
                    if (mySevice.Status != ServiceControllerStatus.Running
                        && mySevice.Status != ServiceControllerStatus.StartPending)
                    {
                        mySevice.Start();
                        mySevice.WaitForStatus(ServiceControllerStatus.Running);
                    }
                }
            }
            catch
            {
                return false;
            }

            return returnValue;
        }
        #endregion

        #region 返回服务状态
        /// <summary>
        /// 返回服务状态
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            try
            {
                var mySevice = new ServiceController(serviceName);

                return mySevice.Status;
            }
            catch
            {
                return ServiceControllerStatus.ContinuePending;
            }
        }
        #endregion

        #region 服务停止
        /// <summary>
        /// 服务停止
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static bool StopService(string serviceName)
        {
            var returnValue = true;
            try
            {
                var mySevice = new ServiceController(serviceName);

                if (mySevice.Status == ServiceControllerStatus.Stopped
                    && mySevice.Status == ServiceControllerStatus.StopPending)
                    returnValue = true;
            }
            catch
            {
                returnValue = false;
            }

            return returnValue;
        }
        #endregion

        #region 修改服务的启动项
        /// <summary>    
        /// 修改服务的启动项    
        /// </summary>    
        /// <param name="startType">2为自动,3为手动 </param>    
        /// <param name="serviceName">服务名称</param>    
        /// <returns></returns>    
        public static bool ChangeServiceStartType(int startType, string serviceName)
        {
            try
            {
                var regist = Registry.LocalMachine;
                var sysReg = regist.OpenSubKey("SYSTEM");
                var currentControlSet = sysReg.OpenSubKey("CurrentControlSet");
                var services = currentControlSet.OpenSubKey("Services");
                var servicesName = services.OpenSubKey(serviceName, true);
                servicesName.SetValue("Start", startType);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 安装服务
        /// <summary>  
        /// 安装服务  
        /// </summary>  
        public static bool InstallService(string serviceName)
        {
            var flag = true;
            if (!IsExistsService(serviceName))
            {
                try
                {
                    var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var serviceFileName = location.Substring(0, location.LastIndexOf('\\') + 1) + serviceName + ".exe";
                    InstallmyService(null, serviceFileName);
                }
                catch
                {
                    flag = false;
                }

            }
            return flag;
        }
        #endregion 

        #region 卸载服务
        /// <summary>  
        /// 卸载服务  
        /// </summary>  
        public static bool UninstallService(string serviceName)
        {
            var flag = true;
            if (IsExistsService(serviceName))
            {
                try
                {
                    var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var serviceFileName = location.Substring(0, location.LastIndexOf('\\') + 1) + serviceName + ".exe";
                    UnInstallmyService(serviceFileName);
                }
                catch
                {
                    flag = false;
                }
            }
            return flag;
        }
        #endregion  

        #region 检查服务是否存在基类
        /// <summary>  
        /// 检查服务是否存在基类
        /// </summary>  
        /// <param name="serviceName">服务名</param>  
        /// <returns></returns>  
        private static bool IsExistsService(string serviceName)
        {
            var services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 卸载Windows服务基类
        /// <summary>  
        /// 卸载Windows服务  
        /// </summary>  
        /// <param name="filepath">程序文件路径</param>  
        private static void UnInstallmyService(string filepath)
        {
            var AssemblyInstaller1 = new AssemblyInstaller();
            AssemblyInstaller1.UseNewContext = true;
            AssemblyInstaller1.Path = filepath;
            AssemblyInstaller1.Uninstall(null);
            AssemblyInstaller1.Dispose();
        }
        #endregion  

        #region 安装Windows服务基类
        /// <summary>  
        /// 安装Windows服务  
        /// </summary>  
        /// <param name="stateSaver">集合</param>  
        /// <param name="filepath">程序文件路径</param>  
        private static void InstallmyService(IDictionary stateSaver, string filepath)
        {
            var AssemblyInstaller1 = new AssemblyInstaller();
            AssemblyInstaller1.UseNewContext = true;
            AssemblyInstaller1.Path = filepath;
            AssemblyInstaller1.Install(stateSaver);
            AssemblyInstaller1.Commit(stateSaver);
            AssemblyInstaller1.Dispose();
        }
        #endregion  
    }
}
