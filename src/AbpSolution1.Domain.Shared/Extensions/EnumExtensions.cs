using AbpSolution1.Attributes;
using System;
using System.Reflection;

namespace AbpSolution1.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attr = memInfo[0].GetCustomAttribute<StringValueAttribute>();
            return attr != null ? attr.StringValue : value.ToString();
        }
    }
}
