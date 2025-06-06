﻿using System.Reflection;

namespace SME.SERAp.Boletim.Infra.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            try
            {
                return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
            }
            catch
            {
                return default;
            }
        }
    }
}
