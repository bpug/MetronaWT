using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Meteo;

    public class MeteoLangGtzMap : EntityTypeConfiguration<MeteoLangGtz>
    {
        public MeteoLangGtzMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.HasKey(t => new{ t.Monat, t.Plz});

            // Properties
            // Table & Column Mappings
            this.ToTable("TAB_METEO_LANGGTZ");
            this.Property(t => t.Id).HasColumnName("LANGGTZ_ID");
            this.Property(t => t.Plz).HasColumnName("PLZ");
            this.Property(t => t.Monat).HasColumnName("Monat");
            this.Property(t => t.Gtz).HasColumnName("GTZ");
        }
    }
}
