
namespace Untility.WinService
{
    /// <summary>
    /// 硬件参数
    /// </summary>
    internal static class ConfigWin32PSI
    {
        /// <summary>
        /// CPU 处理器 
        /// </summary>
        public readonly static string Cpu = "Win32_Processor";

        /// <summary>
        /// 物理内存条
        /// </summary>
        public readonly static string Memory = "Win32_PhysicalMemory";

        /// <summary>
        /// 键盘
        /// </summary>
        public readonly static string Keyboard = "Win32_Keyboard";
                
        /// <summary>
        /// 点输入设备，包括鼠标
        /// </summary>
        public readonly static string PointingDevice = "Win32_PointingDevice";
        
        /// <summary>
        /// 软盘驱动器
        /// </summary>
        public readonly static string FloppyDrive = "Win32_FloppyDrive";
        
        /// <summary>
        /// 硬盘驱动器
        /// </summary>
        public readonly static string DiskDrive = "Win32_DiskDrive";
        
        /// <summary>
        /// 光盘驱动器
        /// </summary>
        public readonly static string CDROMDrive = "Win32_CDROMDrive";
                
        /// <summary>
        /// 主板
        /// </summary>
        public readonly static string BaseBoard = "Win32_BaseBoard";
 
        /// <summary>
        ///  BIOS 芯片 
        /// </summary>
        public readonly static string BIOS = "Win32_BIOS";
        
        /// <summary>
        ///  并口
        /// </summary>
        public readonly static string ParallelPort = "Win32_ParallelPort";
        
        /// <summary>
        ///  串口
        /// </summary>
        public readonly static string SerialPort = "Win32_SerialPort";
        
        /// <summary>
        ///  串口配置
        /// </summary>
        public readonly static string SerialPortConfiguration = "Win32_SerialPortConfiguration";
        
        /// <summary>
        ///  多媒体设置，一般指声卡。
        /// </summary>
        public readonly static string SoundDevice = "Win32_SoundDevice";
        
        /// <summary>
        ///  主板插槽 (ISA & PCI & AGP)
        /// </summary>
        public readonly static string SystemSlot = "Win32_SystemSlot";
                
        /// <summary>
        ///  USB 控制器 
        /// </summary>
        public readonly static string USBController = "Win32_USBController";
        
        /// <summary>
        ///  网络适配器
        /// </summary>
        public readonly static string NetworkAdapter = "Win32_NetworkAdapter";
        
        /// <summary>
        ///  网络适配器设置
        /// </summary>
        public readonly static string NetworkAdapterConfiguration = "Win32_NetworkAdapterConfiguration";
        
        /// <summary>
        ///  打印机
        /// </summary>
        public readonly static string Printer = "Win32_Printer";
        
        /// <summary>
        ///  打印机设置
        /// </summary>
        public readonly static string PrinterConfiguration = "Win32_PrinterConfiguration";
        
        /// <summary>
        ///  打印机任务
        /// </summary>
        public readonly static string PrintJob = "Win32_PrintJob";
        
        /// <summary>
        ///  打印机端口
        /// </summary>
        public readonly static string TCPIPPrinterPort = "Win32_TCPIPPrinterPort";
        
        /// <summary>
        ///  MODEM
        /// </summary>
        public readonly static string POTSModem = "Win32_POTSModem";
        
        /// <summary>
        ///  MODEM 端口 
        /// </summary>
        public readonly static string POTSModemToSerialPort = "Win32_POTSModemToSerialPort";
                
        /// <summary>
        ///  显示器
        /// </summary>
        public readonly static string DesktopMonitor = "Win32_DesktopMonitor";
        
        /// <summary>
        ///  显卡
        /// </summary>
        public readonly static string DisplayConfiguration = "Win32_DisplayConfiguration";
        
        /// <summary>
        ///  显卡设置 
        /// </summary>
        public readonly static string DisplayControllerConfiguration = "Win32_DisplayControllerConfiguration";
        
        /// <summary>
        ///  显卡细节 
        /// </summary>
        public readonly static string VideoController = "Win32_VideoController";
        
        /// <summary>
        ///  显卡支持的显示模式 
        /// </summary>
        public readonly static string VideoSettings = "Win32_VideoSettings";
    }    
}
