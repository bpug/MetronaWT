//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ChartService.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Service
{
    using System;
    using System.Data;
    using System.Linq;

    using Metrona.Wt.Core;
    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Data;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;

    public class ChartService
    {
        private readonly RequestType _requestType;

        private readonly int _search;

        private IntervalType _interval;

        private DataTable temperaturData;

        private DataTable monatsSummenGtzJahr;

        public ChartService(DateTime startDate, int search, RequestType requestType)
        {
            this.StartDate = DateTimeExtensions.GetFirstDayOfMonth(startDate);
            this._search = search;
            this._requestType = requestType;

            this._interval = IntervalType.M24;

            this.EndDate = DateTimeExtensions.GetPastDate(this.StartDate, (int)this._interval);
        }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

       

        public DataTable TemperaturData
        {
            get
            {
                return this.temperaturData ?? (this.temperaturData = this.GetTemperaturData());
            }
        }

        public DataTable MonatsSummenGtzJahr
        {
            get
            {
                return this.monatsSummenGtzJahr ?? (this.monatsSummenGtzJahr = this.GetMonatsSummenGtzVorJahr());
            }
        }

        public bool HasGtzData
        {
            get
            {
                return MonatsSummenGtzJahr != null && MonatsSummenGtzJahr.Rows.Count  > 0;
            }
        }

        public bool HasTemperaturData
        {
            get
            {
                return TemperaturData != null && TemperaturData.Rows.Count > 0;
            }
        }

        public DataTable GetJahresSummenGtz(IntervalType intervalType = IntervalType.M24, bool heizperiode = true)
        {
	        var dt = this.MonatsSummenGtzJahr;

	        if ((dt.Rows.Count < 1)) {
		        return null;
	        }

	        var query = dt.AsEnumerable();

	        if ((heizperiode))
	        {
	            query = query.Where(p => Utils.IsHeizMonat(p["Monat"].ConvertTo<int>()));
	        }

	        string aktuellJahrColumn = dt.Columns[2].ColumnName;
	        string vorJahrColumn = dt.Columns[3].ColumnName;
            string vorVorJahrColumn = dt.Columns[4].ColumnName;
	        string lgtzColumn = dt.Columns[5].ColumnName;

	        double sumAktuellYear = 0;
	        double sumVorYear = 0;
            double sumVorVorYear = 0;
	        double sumLGTZ = 0;

	        sumAktuellYear = query.Sum(p => p.Field<double>(aktuellJahrColumn));
	        sumVorYear = query.Sum(p => p.Field<double>(vorJahrColumn));
            sumVorVorYear = query.Sum(p => p.Field<double>(vorVorJahrColumn));
	        sumLGTZ = query.Sum(p => p.Field<double>(lgtzColumn));
	
	        var table = new DataTable();

            var column = new DataColumn("AktuellJahr", typeof(double));
	        table.Columns.Add(column);

            column = new DataColumn("Vorjahr", typeof(double));
	        table.Columns.Add(column);
            if (intervalType == IntervalType.M36)
            {
                column = new DataColumn("Vorvorjahr", typeof(double));
                table.Columns.Add(column);
            }

            column = new DataColumn("LGTZ", typeof(double));
	        table.Columns.Add(column);

	        DataRow row = table.NewRow();
	        row[0] = sumAktuellYear;
	        row[1] = sumVorYear;
            if (intervalType == IntervalType.M36)
            {
                row[2] = sumVorVorYear;
                row[3] = sumLGTZ;
            }
            else
            {
                row[2] = sumLGTZ;
            }
	        table.Rows.Add(row);

	        return table;
        }

        public DataTable GetJahresbetrachtungProzentual(IntervalType intervalType = IntervalType.M24, bool heizperiode = true)
        {
            var dt = this.GetJahresSummenGtz(intervalType, heizperiode);
            if ((dt == null))
            {
                return null;
            }

            var firstRow = dt.Rows[0];

            double sumAktuellYear = firstRow["AktuellJahr"].ConvertTo<double>();
            double sumVorYear = firstRow["Vorjahr"].ConvertTo<double>();
            double sumLGTZ = firstRow["LGTZ"].ConvertTo<double>();


            double relVorJahr = Utils.GetProzentual(sumVorYear, sumAktuellYear, null);
            double relLGTZ = Utils.GetProzentual(sumLGTZ, sumAktuellYear, null);

            double relVorVorJahr = 0;
            if (intervalType == IntervalType.M36)
            {
                relVorVorJahr = Utils.GetProzentual(firstRow["Vorvorjahr"].ConvertTo<double>(), sumAktuellYear, null);
            }

            var table = new DataTable();

            var column = new DataColumn("Vorjahr", typeof(double));
            table.Columns.Add(column);

            if (intervalType == IntervalType.M36)
            {
                column = new DataColumn("Vorvorjahr", typeof(double));
                table.Columns.Add(column);
            }

            column = new DataColumn("LGTZ", typeof(double));
            table.Columns.Add(column);

            var row = table.NewRow();
            row[0] = relVorJahr;
            if (intervalType == IntervalType.M36)
            {
                row[1] = relVorVorJahr;
                row[2] = relLGTZ;
            }
            else
            {
                row[1] = relLGTZ;
            }
            table.Rows.Add(row);

            return table;
        }

        public DataTable GetMonatsRelativeVerteilungJahr(bool heizperiode)
        {
            var dt = this.MonatsSummenGtzJahr;

	        if ((dt.Rows.Count > 0)) {
		        string aktuellJahrColumn = dt.Columns[2].ColumnName;
		        string vorJahrColumn = dt.Columns[3].ColumnName;
	            const string lgtzColumn = "LGTZ";

		        double sumLgtzYear = 0;

	            var query = dt.AsEnumerable();

		        if ((heizperiode))
		        {
                    query = query.Where(p => Utils.IsHeizMonat(p["Monat"].ConvertTo<int>()));
		        }

                sumLgtzYear = query.Sum(p => p.Field<double>(lgtzColumn));

		        var result = from i in query select new {
			        Monat = i.Field<string>("Monat"),
			        Promille = i.Field<double>("Promille"),
                    @AktuellesJahr = (i.Field<double>(lgtzColumn) - i.Field<double>(aktuellJahrColumn)) / sumLgtzYear * 100,
                    Vorjahr = (i.Field<double>(lgtzColumn) - i.Field<double>(vorJahrColumn)) / sumLgtzYear * 100,
			        //LGTZ = (i.Field<double>(aktuellJahrColumn) - i.Field<double>("LGTZ")) / sumAktuellYear * 100
		        };

                var resultTable = result.CopyToDataTableExt();
                var column = new DataColumn("Aktuelles Jahr gewichtet", typeof(Double))
                {
                    Expression = "AktuellesJahr * (Promille/100)"
                };
                resultTable.Columns.Add(column);

		        column = new DataColumn("Vorjahr gewichtet", typeof(Double))
		        {
		            Expression = "Vorjahr * (Promille/100)"
		        };
	            resultTable.Columns.Add(column);

		        return resultTable;
	        }

	        return null;
        }



        public double GetJahrBedarfWithPromille(bool heizperiode = true)
        {
            double sum = 0;

            var dt = this.MonatsSummenGtzJahr;

            foreach (DataRow row in dt.Rows)
            {
                if ((heizperiode & !Utils.IsHeizMonat(row["Monat"].ConvertTo<int>())))
                {
                    continue;
                }
                var aktuell = row[2].ConvertTo<double>();
                var vorjahr = row[3].ConvertTo<double>();
                var promille = row["Promille"].ConvertTo<double>();
                if ((vorjahr != 0))
                {
                    sum += aktuell / vorjahr * promille;
                }
            }
            var result = (sum / 97 - 1) * 100;
            return result;
        }


        private DataTable GetSortTemperaturTable(DataTable table)
        {
            var dv = table.DefaultView;

            // Fix me
            dv.RowFilter = "Datum <> '02-29'";
            var dt = dv.ToTable();
            var startDate = DateTime.Parse("2000-" + dt.Rows[0]["Datum"]);

            foreach (DataRow row in dt.Rows)
            {
                row["Datum"] = startDate; //.ToString("yyyy-MM-dd")
                startDate = startDate.AddDays(1);
            }

            return dt;
        }

        

        public DataTable GetTemperaturData()
        {
            var dt = new DataTable();

            switch (this._requestType)
            {
                case RequestType.Plz:
                    var station = StationDal.GetByPlz(this._search);
                    if(station == null) return null;
                    dt = ChartDal.GetTemperaturByWSCode(station.WsCode, this.StartDate, this.EndDate);
                    break;
                case RequestType.Bundesland:
                    dt = ChartDal.GetTemperaturByBundesland(this._search, this.StartDate, this.EndDate);
                    break;
                case RequestType.Deutschland:
                    dt = ChartDal.GetTemperaturDeutschland(this.StartDate, this.EndDate);
                    break;
            }
            return this.GetSortTemperaturTable(dt);
        }

        public DataTable GetMonatsSummenGtzVorJahr()
        {
            var dt = new DataTable();

            switch (this._requestType)
            {
                case RequestType.Plz:
                    //int wscode = StationDa.GetByPlz(this._search).WsCode;
                    //dt = ChartDAL.GetMonatssummenGTZVorJahrByPLZ(this._search, this.StartDate, this.EndDate);
                    dt = ChartDal.GetMonatssummenGTZVor2JahreByPLZ(this._search, this.StartDate);
                    break;
                case RequestType.Bundesland:
                    //dt = ChartDAL.GetMonatssummenGTZVorJahrByBundesland(this._search, this.StartDate, this.EndDate);
                    dt = ChartDal.GetMonatssummenGTZVor2JahreByBundesland(this._search, this.StartDate);
                    break;
                case RequestType.Deutschland:
                    //dt = ChartDAL.GetMonatssummenGTZVorJahrDeutschland(this.StartDate, this.EndDate);
                    dt = ChartDal.GetMonatssummenGTZVor2JahreDeutschland(this.StartDate);
                    break;
            }
            return dt;
        }


        public DataTable GetTemperaturDrill(int month)
        {
	        int month1 = 0, month2 = 0, month3 = 0;
	      

	        int startMonth = StartDate.Month;
	        int endMonth = EndDate.Month;

	        if (!(month == startMonth)) {
		        month3 = (month == 12 ? 1 : month + 1);
	        }
	        if (!(month == endMonth)) {
		        month1 = (month == 1 ? 12 : month - 1);
	        }
	        month2 = month;

	        var temperaturs = TemperaturData;

            var query =
                temperaturs.AsEnumerable()
                    .Where(
                        p =>
                            p["Datum"].ConvertTo<DateTime>().Month == month1
                            || p["Datum"].ConvertTo<DateTime>().Month == month2
                            || p["Datum"].ConvertTo<DateTime>().Month == month3);

	        var result = query.CopyToDataTable();
	        return result;
        }

        public static DataTable CalculateSum(DataTable dataTable)
        {
            var query = dataTable.AsEnumerable();

            var table = query.CopyToDataTable();

            var newRow = table.NewRow();

            //Summe Heizperiode

            newRow[0] =  "Summe Heizperiode";

            var cellsCount = table.Columns.Count - 1;
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


        private static double CalcColumnSumme(DataTable table, int colNumber, bool heizperiode = false)
        {
            var query = table.AsEnumerable();
            if ((heizperiode))
            {
                query = query.Where(p => Utils.IsHeizMonat(p["Monat"].ConvertTo<int>()));
            }
            var sum = query.Sum(p => p.Field<double>(colNumber));
            return sum;
        }

        public static DataTable GetTableWithMonts(DataTable table, string columnName = "Monat")
        {
            foreach (DataRow row in table.Rows)
            {
                var month = row[columnName].ConvertTo<int>();
                var monthName = Utils.GetMonthName(month, "MMMM");
                if (!Utils.IsHeizMonat(month))
                {
                    monthName = string.Format("({0})", monthName);
                }
                row[columnName] = monthName;
            }

            return table;
        }
    }
}