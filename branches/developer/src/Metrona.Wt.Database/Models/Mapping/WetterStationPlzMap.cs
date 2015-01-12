using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;

    public class WetterStationPlzMap : EntityTypeConfiguration<WetterStationPlz>
    {
        public WetterStationPlzMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TAB_WETTERSTATION_PLZ");
            this.Property(t => t.Id).HasColumnName("WetterstationPLZ_ID");
            this.Property(t => t.Von).HasColumnName("von");
            this.Property(t => t.Bis).HasColumnName("bis");
            this.Property(t => t.StationId).HasColumnName("WSTATI");

            // Navigation
            //this.HasRequired(t => t.Station).WithMany(p => p.Plzs).HasForeignKey(p => p.StationId);
        }
    }
}
