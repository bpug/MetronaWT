//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IPdfReport.cs.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Reports.Pdf
{
    using System.Threading.Tasks;

    using Metrona.Wt.Model;

    public interface IPdfReport
    {
        Task<IReporter> GetReporter(CalculateRequest calcRequest, int drillMonth, string logoPath);

        Task Download(CalculateRequest calcRequest, int drillMonth, string logoPath, string fileName);
    }
}