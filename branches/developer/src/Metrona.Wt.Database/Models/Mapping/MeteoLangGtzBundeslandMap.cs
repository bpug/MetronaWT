//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MeteoLangGtzBundeslandMap.cs" company="ip-connect GmbH">
//    Copyright (c) ip-connect GmbH. All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Metrona.Wt.Database.Models.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using Metrona.Wt.Model.Meteo;

    public class MeteoLangGtzBundeslandMap : EntityTypeConfiguration<MeteoLangGtzBundesland>
    {
        public MeteoLangGtzBundeslandMap()
        {
            // Primary Key
            this.HasKey(
                t => new
                {
                    t.BundeslandId,
                    t.Monat
                });

            //// Properties
            //this.Property(t => t.BundeslandId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("view_meteo_langgtz_bundesland_avg");
            this.Property(t => t.BundeslandId).HasColumnName("BundeslandID");
            this.Property(t => t.Monat).HasColumnName("Monat");
            this.Property(t => t.Gtz).HasColumnName("GTZ");
            
        }
    }
}