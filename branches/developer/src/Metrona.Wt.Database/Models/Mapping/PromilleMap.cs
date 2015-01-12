using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;

    public class PromilleMap : EntityTypeConfiguration<Promille>
    {
        public PromilleMap()
        {
            // Primary Key
            //this.HasKey(t => t.Id);
            this.HasKey(t => t.Monat);

            // Properties
            // Table & Column Mappings
            this.ToTable("TAB_PROMILLE");
            this.Property(t => t.Id).HasColumnName("PROMILLE_ID");
            this.Property(t => t.Monat).HasColumnName("MONAT");
            this.Property(t => t.Anteil).HasColumnName("ANTEIL");
        }
    }
}
