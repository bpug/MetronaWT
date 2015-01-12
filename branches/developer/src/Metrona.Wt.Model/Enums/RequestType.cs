//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RequestType.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Enums
{
    using System.ComponentModel.DataAnnotations;

    using Metrona.Wt.Model.Attributes;

    public enum RequestType
    {
        [Display(Name = "PLZ")]
        [Authorize]
        Plz = 1,

        Bundesland = 2,

        Deutschland = 3
    }
}