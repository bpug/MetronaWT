using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;

    public class BundeslandMap : EntityTypeConfiguration<Bundesland>
    {
        public BundeslandMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("TAB_BUNDESLAND");
            this.Property(t => t.Id).HasColumnName("BundeslandID");
            this.Property(t => t.Name).HasColumnName("Bundesland");

            HasMany(p => p.Plzs).WithRequired(p => p.Bundesland).HasForeignKey(p => p.BundeslandId);
        }
    }
}
