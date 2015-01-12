namespace Metrona.Wt.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    using Metrona.Wt.Model.Meteo;

    public static class DataTableExtensions
    {
        public static DataTable ToDataTable(this MeteoGtzYear source)
        {
            var list = new List<MeteoGtzYear>
            {
                source
            };
            return list.ToDataTable();
        }
        public static DataTable ToDataTable<TSource>(this IEnumerable<TSource> source)
        {
            var dataTable = new DataTable(typeof(TSource).Name);
            var propertyInfos = typeof(TSource).GetProperties();

            var colums = GetHeaderSorted(propertyInfos).ToArray();
            dataTable.Columns.AddRange(colums);

            source.ForEach(d => dataTable.Rows.Add(GetDataRowSorted(d, propertyInfos).ToArray()));
            
            //foreach (PropertyInfo prop in props)
            //{
            //    var display = prop.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
            //    var columnAtrr = prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as ColumnAttribute;
            //    Type type = null;

            //    try
            //    {
            //        type = columnAtrr != null && !string.IsNullOrEmpty(columnAtrr.TypeName)
            //        ? Type.GetType(columnAtrr.TypeName) : null;
            //    }
            //    catch
            //    {
            //    }
                

            //    dataTable.Columns.Add(display != null ? display.Name : prop.Name, type ?? (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
            //}

            //foreach (TSource item in data)
            //{
            //    var values = new object[propertyInfos.Length];

            //    for (int i = 0; i < propertyInfos.Length; i++)
            //        {
            //            values[i] = propertyInfos[i].GetValue(item, null);
            //        } 
               
            //    dataTable.Rows.Add(values);
            //}
            return dataTable;
        }

        private static IEnumerable<DataColumn> GetHeaderSorted(IEnumerable<PropertyInfo> propertyInfos)
        {
            var headersSorted = propertyInfos.Select(
                propertyInfo => new
                {
                    propertyInfo.Name,
                    DisplayAttribute =
                        propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute,
                    CsvColumnAttribute =
                        (propertyInfo.GetCustomAttribute(typeof(ColumnAttribute), false) as ColumnAttribute) ?? new ColumnAttribute { Order = 9999 },
                    PropertyInfo = propertyInfo
                }).Select(
                    e => new
                    {
                        Name =
                            e.CsvColumnAttribute.Name ?? (e.DisplayAttribute != null ? e.DisplayAttribute.Name : e.Name),
                        e.CsvColumnAttribute.Order,
                        Typ =
                            !string.IsNullOrEmpty(e.CsvColumnAttribute.TypeName)
                                ? Type.GetType(e.CsvColumnAttribute.TypeName)
                                : (Nullable.GetUnderlyingType(e.PropertyInfo.PropertyType) ?? e.PropertyInfo.PropertyType)
                    }).OrderBy(x => x.Order)
                    .Select(
                        c => new DataColumn
                        {
                            ColumnName = c.Name,
                            DataType = c.Typ
                        });

            return headersSorted;
        }


        private static IEnumerable<object> GetDataRowSorted<T>(T csvDataObject, IEnumerable<PropertyInfo> propertyInfos)
        {
            var valuesSorted = propertyInfos.Select(
                x => new
                {
                    Value = x.GetValue(csvDataObject, null),
                    Attribute = (ColumnAttribute)Attribute.GetCustomAttribute(x, typeof(ColumnAttribute), false) ?? new ColumnAttribute { Order = 9999}
                });

            var result = valuesSorted.OrderBy(x => x.Attribute.Order).Select(x => x.Value);
            return result;
        }


       }
}