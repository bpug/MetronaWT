//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ObjectExtensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public static class ObjectExtensions
    {
        public static bool IsBetween<T>(this T value, T lowest, T highest) where T : IComparable
        {
            return (Comparer<T>.Default.Compare(lowest, value) <= 0 && Comparer<T>.Default.Compare(highest, value) >= 0);
        }

        public static T ConvertTo<T>(this object value)
        {
            return ConvertTo<T>(value, CultureInfo.CurrentCulture);
        }

        public static T ConvertTo<T>(this object value, CultureInfo cultureInfo)
        {
            var toType = typeof(T);

            if (value == null)
            {
                return default(T);
            }

            if (value is string)
            {
                if (toType == typeof(Guid))
                {
                    return ConvertTo<T>(new Guid(Convert.ToString(value, cultureInfo)), cultureInfo);
                }

                if ((string)value == string.Empty && toType != typeof(string))
                {
                    return ConvertTo<T>(null, cultureInfo);
                }
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    return ConvertTo<T>(Convert.ToString(value, cultureInfo), cultureInfo);
                }
            }

            if (toType.IsGenericType && toType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                toType = Nullable.GetUnderlyingType(toType);
            }

            bool canConvert = toType is IConvertible || (toType.IsValueType && !toType.IsEnum && toType != typeof(bool));

            if (canConvert)
            {
                return (T)Convert.ChangeType(value, toType, cultureInfo);
            }

            if (toType.IsValueType && toType == typeof(bool))
            {
                return
                    (T)
                        (object)
                            (value.ToString().Equals("1")
                             || value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase));
            }

            if (toType.IsValueType && toType.IsEnum)
            {
                return (T)Enum.Parse(toType, value.ToString());
            }

            return (T)value;
        }
    }
}