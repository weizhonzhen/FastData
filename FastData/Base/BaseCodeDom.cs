using System;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using FastUntility.Base;
using System.Linq;
using System.Reflection;

namespace FastData.Base
{
    /// <summary>
    /// 动态解析条件
    /// </summary>
    internal static class BaseCodeDom
    {
        [Obsolete]
        public static bool GetResult(string code,string references=null)
        {
            //动态编译
            var compiler = new CSharpCodeProvider().CreateCompiler(); 
            var param = new CompilerParameters();

            param.ReferencedAssemblies.Add("System.dll");
            param.ReferencedAssemblies.Add("System.Core.dll");
            param.ReferencedAssemblies.Add("mscorlib.dll");

            var assembly = AppDomain.CurrentDomain.GetAssemblies().ToList().Find(a => a.FullName.Split(',')[0] == references);
            if (assembly == null && references != null)
            {
                assembly = Assembly.Load(references);
                param.ReferencedAssemblies.Add(assembly.Location);
            }

            param.GenerateExecutable = false;
            param.GenerateInMemory = true;                        
            var result = compiler.CompileAssemblyFromSource(param, GetCode(code,references));

            if (result.Errors.HasErrors)
            {
                var error = new StringBuilder();
                error.AppendFormat("code:{0},error info:", GetCode(code));
                               
                foreach (CompilerError info in result.Errors)
                {
                    error.Append(info.ErrorText);
                }

                BaseLog.SaveLog(error.ToString(), "DynamicCompiler");

                return false;
            }
            else
            {
                assembly = result.CompiledAssembly;
                var instance = assembly.CreateInstance("DynamicCode.Condition");
                var method = instance.GetType().GetMethod("OutPut");
                return (bool)BaseEmit.Invoke(instance, method, null);
            }
        }

        /// <summary>
        /// 源代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetCode(string code, string references = null)
        {
            var sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append("using System.Collections.Generic;");
            sb.Append("using System.Linq;");
            sb.Append("using System.Web;");

            if (!string.IsNullOrEmpty(references))
                sb.AppendFormat("using {0};", references);

            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCode");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("    public class Condition");
            sb.Append(Environment.NewLine);
            sb.Append("    {");
            sb.Append(Environment.NewLine);
            sb.Append("        public bool OutPut()");
            sb.Append(Environment.NewLine);
            sb.Append("        {");
            sb.Append(Environment.NewLine);
            sb.AppendFormat("             return {0};", code);
            sb.Append(Environment.NewLine);
            sb.Append("        }");
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);
            sb.Append("}");

            return sb.ToString();
        }
    }
}
