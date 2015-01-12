//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DataSetLinqExtension.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Core.Extensions
{
    using System.Collections.Generic;
    using System.Data;

    public static class DataSetLinqExtension
    {
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            return new ObjectShredder<T>().Shred(source, table, options);
        }

        public static DataTable CopyToDataTableExt<T>(this IEnumerable<T> source)
        {
            return new ObjectShredder<T>().Shred(source, null, null);
        }
    }
}