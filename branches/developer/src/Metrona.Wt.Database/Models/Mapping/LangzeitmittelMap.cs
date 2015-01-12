using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;

    public class LangzeitmittelMap : EntityTypeConfiguration<Langzeitmittel>
    {
        public LangzeitmittelMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TAB_LANGZEITMITTEL");
            this.Property(t => t.Id).HasColumnName("Langzeitmittel_ID");
            this.Property(t => t.StationId).HasColumnName("WSTATI");
            this.Property(t => t.Hgt).HasColumnName("HGT");
            this.Property(t => t.Gtz).HasColumnName("GTZ");
            this.Property(t => t.RelHgt).HasColumnName("RelHGT");
            this.Property(t => t.RelGtz).HasColumnName("RelGTZ");
        }
    }
}
