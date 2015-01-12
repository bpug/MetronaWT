//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IEnumerableExtensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Core.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (T item in array)
            {
                action(item);
            }
        }
    }
}