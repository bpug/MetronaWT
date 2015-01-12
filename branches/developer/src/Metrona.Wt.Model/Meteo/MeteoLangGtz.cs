//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoLangGtz.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    using System.Collections.Generic;

    public partial class MeteoLangGtz
    {
        public double Gtz { get; set; }

        public int Id { get; set; }

        public int Monat { get; set; }

        public int Plz { get; set; }

        //public ICollection<MeteoGtz> MeteoGtzes { get; set; }

       
    }
}