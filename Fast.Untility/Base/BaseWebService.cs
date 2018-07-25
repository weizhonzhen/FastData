using System;
using System.Text;
using System.Net;
using System.Web.Services.Description;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace Fast.Untility.Base
{
    /// <summary>
    /// 动态解析服务
    /// </summary>
    public static class BaseWebService
    {
        #region 动态调用web服务
        /// < summary>
        /// 动态调用web服务
        /// < /summary>
        /// < param name="url">WSDL服务地址< /param>
        /// < param name="classname">类名< /param>
        /// < param name="methodname">方法名< /param>
        /// < param name="args">参数< /param>
        /// < returns>< /returns>
        public static object InvokeService(string url, string methodName, object[] args,string className=null)
        {
            var nameSpace = "EnterpriseServerBase.WebService.DynamicWebCalling";

            //服务类名
            if (string.IsNullOrEmpty(className)) 
                className = GetWsClassName(url);

            try
            {
                var wc = new WebClient();
                if (url.ToUpper().IndexOf("WSDL") < 0)
                    url += "?WSDL";

                //读取
                var stream = wc.OpenRead(url);

                //获取WSDL
                var sd = ServiceDescription.Read(stream);
                var sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                var cn = new CodeNamespace(nameSpace);

                //生成客户端代理类代码
                var ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);

                var icc = new CSharpCodeProvider();

                //设定编译参数
                var cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                var cr = icc.CompileAssemblyFromDom(cplist, ccu);

                if (true == cr.Errors.HasErrors)
                {
                    var sb = new StringBuilder();

                    foreach (var ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }

                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                var assembly = cr.CompiledAssembly;
                var t = assembly.GetType(nameSpace + "." + className, true, true);
                var bj = Activator.CreateInstance(t);
                var mi = t.GetMethod(methodName);

                return mi.Invoke(bj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }
        #endregion

        #region 获取服务的类名
        /// <summary>
        /// 获取服务的类名
        /// </summary>
        /// <param name="wsUrl"></param>
        /// <returns></returns>
        private static string GetWsClassName(string wsUrl)
        {
            var parts = wsUrl.Split('/');
            var pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion
    }
}
