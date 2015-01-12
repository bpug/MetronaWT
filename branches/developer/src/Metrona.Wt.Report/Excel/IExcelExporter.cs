//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IExcelExporter.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Excel
{
    using System.Threading.Tasks;

    using Metrona.Wt.Model;

    public interface IExcelExporter
    {
        Task Export(CalculateRequest calculateRequest);
    }
}