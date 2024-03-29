﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace FastData.Proxy
{
    public static class FastProxy
    {
        private static TypeBuilder CreateTypeBuilder(System.Type type)
        {
            var assemblyName = new AssemblyName(type.FullName);
            var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(assemblyName.Name);
            return module.DefineType(assemblyName.Name, TypeAttributes.Public | TypeAttributes.Class, null, new[] { type });
        }

        private static void Proxy(System.Type type, TypeBuilder typeBuilder, MethodInfo[] methods)
        {
            var handler = typeof(IProxyHandler).GetMethod("Invoke");

            var handlerField = typeBuilder.DefineField("Proxy_Handler", typeof(IProxyHandler), FieldAttributes.Private);

            var methodField = typeBuilder.DefineField("Proxy_Method", typeof(MethodInfo), FieldAttributes.Private);

            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public,
                                        CallingConventions.Standard, new[] { typeof(IProxyHandler), typeof(MethodInfo[]) });

            var mIL = constructor.GetILGenerator();
            mIL.Emit(OpCodes.Ldarg_0);
            mIL.Emit(OpCodes.Call, typeof(object).GetConstructor(new System.Type[0]) ?? throw new Exception("error:object.GetConstructor"));
            mIL.Emit(OpCodes.Ldarg_0);
            mIL.Emit(OpCodes.Ldarg_1);
            mIL.Emit(OpCodes.Stfld, handlerField);
            mIL.Emit(OpCodes.Ldarg_0);
            mIL.Emit(OpCodes.Ldarg_2);
            mIL.Emit(OpCodes.Stfld, methodField);
            mIL.Emit(OpCodes.Ret);

            for (var i = 0; i < methods.Length; i++)
            {
                var parameterTypes = methods[i].GetParameters().Select(p => p.ParameterType).ToArray();

                var methodBuilder = typeBuilder.DefineMethod(methods[i].Name, MethodAttributes.Public | MethodAttributes.Virtual,
                                        methods[i].CallingConvention, methods[i].ReturnType, parameterTypes);

                var pIL = methodBuilder.GetILGenerator();
                pIL.Emit(OpCodes.Ldarg_0);
                pIL.Emit(OpCodes.Ldfld, handlerField);
                pIL.Emit(OpCodes.Ldarg_0);
                pIL.Emit(OpCodes.Ldarg_0);
                pIL.Emit(OpCodes.Ldfld, methodField);
                pIL.Emit(OpCodes.Ldc_I4, i);
                pIL.Emit(OpCodes.Ldelem_Ref);
                pIL.Emit(OpCodes.Ldc_I4, parameterTypes.Length);
                pIL.Emit(OpCodes.Newarr, typeof(object));

                for (var j = 0; j < parameterTypes.Length; j++)
                {
                    pIL.Emit(OpCodes.Dup);
                    pIL.Emit(OpCodes.Ldc_I4_S, (short)j);
                    pIL.Emit(OpCodes.Ldarg_S, (short)(j + 1));
                    pIL.Emit(OpCodes.Box, parameterTypes[j]);
                    pIL.Emit(OpCodes.Stelem_Ref);
                }

                pIL.Emit(OpCodes.Callvirt, handler);
                if (methods[i].ReturnType == typeof(void))
                    throw new ProxyException(string.Format("method：{0}, ReturnType void Not available", methods[i].Name));

                pIL.Emit(OpCodes.Unbox_Any, methods[i].ReturnType);
                pIL.Emit(OpCodes.Ret);
            }
        }

        public static T Invoke<T>(IProxyHandler handler)
        {
            if (typeof(T).IsInterface)
                return (T)Invoke(typeof(T), handler);
            else
                throw new ProxyException(string.Format("Type：{0} is not Interface ", typeof(T).Name));
        }

        public static object Invoke(System.Type type, IProxyHandler handler)
        {
            if (!typeof(IProxyHandler).GetMethods().ToList().Exists(a => a.Name == "Invoke"))
                throw new ProxyException(string.Format("Type：{0}, not exists Invoke Method", type.Name));

            var methods = type.GetMethods();
            var typeBuilder = CreateTypeBuilder(type);
            Proxy(type, typeBuilder, methods);

            return Activator.CreateInstance(typeBuilder.CreateType(), handler, methods);
        }
    }
}