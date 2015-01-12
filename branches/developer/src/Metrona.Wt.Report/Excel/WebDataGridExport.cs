//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WebDataGridExport.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Excel
{
    using Infragistics.Web.UI.GridControls;

    public class WebDataGridExport
    {
        public int ColumnOffset { get; set; }

        public WebDataGrid Grid { get; set; }

        public int RowOffset { get; set; }
    }
}