using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;

    public class BundeslandPlzMap : EntityTypeConfiguration<BundeslandPlz>
    {
        public BundeslandPlzMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.HasKey(t => t.Plz);

            // Properties
            this.Property(t => t.BundeslandName)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.StationId)
                .IsRequired();
                //.HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("TAB_BUNDESLAND_PLZ");
            this.Property(t => t.Id).HasColumnName("BundeslandPlzID");
            this.Property(t => t.BundeslandName).HasColumnName("Bundesland");
            this.Property(t => t.BundeslandId).HasColumnName("BundeslandID");
            this.Property(t => t.Plz).HasColumnName("PLZ");
            this.Property(t => t.StationId).HasColumnName("WSTATI");
        }
    }
}
