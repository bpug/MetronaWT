//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CalculateRequestModel.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model
{
    using System;

    using Metrona.Wt.Model.Enums;

    public class CalculateRequest
    {
        public int Value { get; set; }
        
        public DateTime Stichtag { get; set; }

        public RequestType RequestType { get; set; }
    }
}