using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class TAB_LANGZEITGTZ_TAGESWERT_DWDMap : EntityTypeConfiguration<LangzeitGtzDwd>
    {
        public TAB_LANGZEITGTZ_TAGESWERT_DWDMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TAB_LANGZEITGTZ_TAGESWERT_DWD");
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Wscode).HasColumnName("WSCODE");
            this.Property(t => t.Monat).HasColumnName("MONAT");
            this.Property(t => t.Lzgt2015).HasColumnName("LZGT2015");
        }
    }
}
