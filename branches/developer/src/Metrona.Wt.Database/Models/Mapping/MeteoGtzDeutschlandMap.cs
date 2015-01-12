//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ViewMeteoGtzDeutschlandMap.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Metrona.Wt.Model.Meteo;

    public class MeteoGtzDeutschlandMap : EntityTypeConfiguration<MeteoGtzDeutschland>
    {
        public MeteoGtzDeutschlandMap()
        {
            this.HasKey(t =>  t.Jahr);

            // Table & Column Mappings
            this.ToTable("view_meteo_gtz_deutschland_avg");

            this.Property(t => t.Jahr).HasColumnName("jahr");
            this.Property(t => t.Monat).HasColumnName("monat");
            this.Property(t => t.Gtz).HasColumnName("GTZ");

            HasOptional(p => p.Lgtz).WithMany().HasForeignKey(p => new { p.Monat });

            HasOptional(p => p.Promille).WithMany().HasForeignKey(p => p.Monat);

            
        }
    }
}