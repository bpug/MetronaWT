//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoGtz.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Model.Meteo
{
    public partial class MeteoGtz : Entity
    {
        public double Gtz { get; set; }

        public int HeizTage { get; set; }

        public int Jahr { get; set; }

        public int Monat { get; set; }

        public int Plz { get; set; }

        public Promille Promille { get; set; }

        public MeteoLangGtz Lgtz { get; set; }

        public BundeslandPlz BundeslandPlz { get; set; }
        
    }
}