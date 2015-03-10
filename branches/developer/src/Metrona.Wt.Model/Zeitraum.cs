//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Zeitraum.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model
{
    using System;

    public class Zeitraum
    {
        public string Name { get; set; }

        public  DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Raum
        {
            get
            {
                return string.Format("{0: MMM. yyyy} - {1: MMM. yyyy}", this.Start, this.End);
            }
            
        }
    }
}