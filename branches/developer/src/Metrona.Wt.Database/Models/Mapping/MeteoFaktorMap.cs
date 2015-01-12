using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Meteo;

    public class MeteoFaktorMap : EntityTypeConfiguration<MeteoFaktor>
    {
        public MeteoFaktorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TAB_METEO_MFAKTOR");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Monat).HasColumnName("Monat");
            this.Property(t => t.Faktor).HasColumnName("MFaktor");
        }
    }
}
