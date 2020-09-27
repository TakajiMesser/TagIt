using System;
using System.Reflection;

namespace TagIt.Shared.Utilities
{
    public static class ReflectionExtensions
    {
        public static bool HasCustomAttribute<T>(this PropertyInfo propertyInfo, bool inherit = true) where T : Attribute
        {
            return propertyInfo.GetCustomAttribute<T>(inherit) != null;
        }
    }
}
