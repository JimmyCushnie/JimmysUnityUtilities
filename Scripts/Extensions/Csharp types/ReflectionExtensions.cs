using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JimmysUnityUtilities
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Search all loaded assemblies for extension methods on a given type.
        /// </summary>
        public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type)
        {
            var loadedTypes = new List<Type>();
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                loadedTypes.AddRange(ass.GetTypes());
            }

            var query = 
                from loadedType in loadedTypes
                where loadedType.IsSealed && !loadedType.IsGenericType && !loadedType.IsNested
                from method in loadedType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                where method.IsDefined(typeof(ExtensionAttribute), false)
                where method.GetParameters()[0].ParameterType == type
                select method;

            return query;
        }
    }
}