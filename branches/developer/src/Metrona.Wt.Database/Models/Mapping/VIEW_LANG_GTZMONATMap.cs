using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class VIEW_LANG_GTZMONATMap : EntityTypeConfiguration<VIEW_LANG_GTZMONAT>
    {
        public VIEW_LANG_GTZMONATMap()
        {
            // Primary Key
            this.HasKey(t => t.WSCODE);

            // Properties
            this.Property(t => t.WSCODE)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VIEW_LANG_GTZMONAT");
            this.Property(t => t.WSCODE).HasColumnName("WSCODE");
            this.Property(t => t.MONAT).HasColumnName("MONAT");
            this.Property(t => t.LZGT2015).HasColumnName("LZGT2015");
        }
    }
}
