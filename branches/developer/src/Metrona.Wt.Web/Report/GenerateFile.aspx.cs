//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GenerateFile.aspx.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.Report
{
    using System;
    using System.Web.UI;

    using Metrona.Wt.Model;
    using Metrona.Wt.Reports.Excel;
    using Metrona.Wt.Reports.Pdf;

    using Microsoft.Practices.Unity;

    public partial class GenerateFile : Page
    {
        [Dependency]
        public IExcelExporter ExcelExporter { get; set; }

        [Dependency]
        public IPdfReport PdfReport { get; set; }

        private CalculateRequest calculateRequest
        {
            get
            {
                return SessionData.CalculateRequest;
            }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            //if (this.calculateRequest == null || !this.User.Identity.IsAuthenticated)
            if (this.calculateRequest == null)
            {
                return;
            }
            string type = this.Request.QueryString["type"];
            switch (type)
            {
                case "btnPrintPDF":
                {
                    var logo = this.Server.MapPath("~/img/bm_logo.jpg");
                    await
                        this.PdfReport.Download(
                            this.calculateRequest,
                            SessionData.TemperaturDrillMonat,
                            logo,
                            "Witterungstelegramm.pdf");
                }
                    break;
                case "btnExportExecl":
                    await this.ExcelExporter.Export(this.calculateRequest);
                    break;
            }
        }
    }
}