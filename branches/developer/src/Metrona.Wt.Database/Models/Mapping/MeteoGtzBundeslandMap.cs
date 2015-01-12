//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ViewMeteoGtzBundeslandMap.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Meteo;

    public class MeteoGtzBundeslandMap : EntityTypeConfiguration<MeteoGtzBundesland>
    {
        public MeteoGtzBundeslandMap()
        {
            // Primary Key
            //this.HasKey(
            //    t => new
            //    {
            //        t.BundeslandId,
            //        t.Jahr,
            //        t.Monat
            //    });

            this.HasKey(t => t.Jahr);

            // Properties
            //this.Property(t => t.BundeslandId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("view_meteo_gtz_bundesland_avg");
            this.Property(t => t.BundeslandId).HasColumnName("BundeslandID");
            this.Property(t => t.Jahr).HasColumnName("Jahr");
            this.Property(t => t.Monat).HasColumnName("Monat");
            this.Property(t => t.Gtz).HasColumnName("GTZ");

            HasRequired(p => p.Promille).WithMany().HasForeignKey(p => p.Monat).WillCascadeOnDelete(false);

            HasOptional(p => p.Lgtz).WithMany().HasForeignKey(p => new { p.BundeslandId, p.Monat, });
            
        }
    }
}