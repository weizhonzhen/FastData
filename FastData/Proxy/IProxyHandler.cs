using System;
using System.Reflection;

namespace FastData.Proxy
{
    public interface IProxyHandler
    {
        Object Invoke(Object proxy, MethodInfo method, Object[] args);
    }
}
