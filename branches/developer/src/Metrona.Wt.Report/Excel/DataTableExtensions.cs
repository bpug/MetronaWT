//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DataTableExtensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Excel
{
    using System.Data;
    using System.Linq;

    using Metrona.Wt.Core;
    using Metrona.Wt.Core.Extensions;

    public static class DataTableExtensions
    {
        public static DataTable CalculateSum(this DataTable source)
        {
            var query = source.AsEnumerable();

            var table = query.CopyToDataTable();

            var newRow = table.NewRow();

            //Summe Heizperiode

            newRow[0] = "Summe Heizperiode";

            int cellsCount = table.Columns.Count - 1;
            for (int i = 1; i <= cellsCount; i++)
            {
                newRow[i] = CalcColumnSumme(table, i, true);
            }

            //Summe Jahr
            var newRow2 = table.NewRow();
            newRow2[0] = "Summe Jahr";

            for (int i = 1; i <= cellsCount; i++)
            {
                newRow2[i] = CalcColumnSumme(table, i, false);
            }
            table = GetTableWithMonts(table);

            table.Rows.Add(newRow);
            table.Rows.Add(newRow2);

            return table;
        }

        public static DataTable GetTableWithMonts(DataTable table, string columnName = "Monat")
        {
            foreach (DataRow row in table.Rows)
            {
                int month = row[columnName].ConvertTo<int>();
                string monthName = Utils.GetMonthName(month, "MMMM");
                if (!month.IsHeizMonat())
                {
                    monthName = string.Format("({0})", monthName);
                }
                row[columnName] = monthName;
            }
            return table;
        }

        
        private static double CalcColumnSumme(DataTable table, int colNumber, bool heizperiode = false)
        {
            var query = table.AsEnumerable();
            if ((heizperiode))
            {
                query = query.Where(p => p["Monat"].ConvertTo<int>().IsHeizMonat());
            }
            double sum = query.Sum(p => p.Field<double>(colNumber));
            return sum;
        }
    }
}