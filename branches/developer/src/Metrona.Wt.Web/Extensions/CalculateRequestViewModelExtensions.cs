//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CalculateRequestViewModelExtensions.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Web.Extensions
{
    using Metrona.Wt.Core.Extensions;
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Enums;
    using Metrona.Wt.Web.Models;

    public static class CalculateRequestViewModelExtensions
    {
        public static CalculateRequest ToModel(this CalculateRequestViewModel source, bool firstDayOfMonth = true)
        {
            var result = new CalculateRequest
            {
                Stichtag = firstDayOfMonth ? source.Date.GetFirstDayOfMonth() : source.Date,
                RequestType = source.RequestType,
                Value = source.RequestType == RequestType.Bundesland ? source.BundeslandId.GetValueOrDefault() : source.Plz.GetValueOrDefault()
            };
            return result;
        }

        
    }
}