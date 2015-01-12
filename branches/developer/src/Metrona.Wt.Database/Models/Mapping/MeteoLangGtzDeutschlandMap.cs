//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoLangGtzDeutschlandMap.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Metrona.Wt.Model.Meteo;

    public class MeteoLangGtzDeutschlandMap : EntityTypeConfiguration<MeteoLangGtzDeutschland>
    {
        public MeteoLangGtzDeutschlandMap()
        {
            // Primary Key
            this.HasKey(t => t.Monat);

            // Table & Column Mappings
            this.ToTable("view_meteo_langgtz_deutschland_avg");
            this.Property(t => t.Monat).HasColumnName("Monat");
            this.Property(t => t.Gtz).HasColumnName("GTZ");
        }
    }
}