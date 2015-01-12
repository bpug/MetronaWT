using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;

    public class WetterStationMap : EntityTypeConfiguration<WetterStation>
    {
        public WetterStationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });
            this.HasKey(t => new { t.StationId });

            // Properties
            this.Property(t => t.Dienst)
                .HasMaxLength(10);

            this.Property(t => t.Icaok)
                .HasMaxLength(10);

            this.Property(t => t.Ort)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("TAB_WETTERSTATION");
            this.Property(t => t.Id).HasColumnName("Wetterstation_ID");
            this.Property(t => t.StationId).HasColumnName("WSTATI");
            this.Property(t => t.Dienst).HasColumnName("WDIENS");
            this.Property(t => t.Icaok).HasColumnName("ICAOK");
            this.Property(t => t.Ort).HasColumnName("WDSORT");
            this.Property(t => t.WsCode).HasColumnName("WSCODE");
            this.Property(t => t.BundeslandId).HasColumnName("BundeslandID");

            //HasMany(t => t.Klimas).WithRequired(p => p.Station).HasForeignKey(p => p.WsCode);
            HasMany(p => p.Plzs).WithRequired(p => p.Station).HasForeignKey(p => p.StationId);
        }
    }
}
