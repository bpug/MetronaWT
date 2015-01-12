using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Meteo;

    public class MeteoGtzMap : EntityTypeConfiguration<MeteoGtz>
    {
        public MeteoGtzMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TAB_METEO_GTZ");
            this.Property(t => t.Id).HasColumnName("GTZ_ID");
            this.Property(t => t.Plz).HasColumnName("PLZ");
            this.Property(t => t.Monat).HasColumnName("Monat");
            this.Property(t => t.Jahr).HasColumnName("Jahr");
            this.Property(t => t.Gtz).HasColumnName("GTZ");
            this.Property(t => t.HeizTage).HasColumnName("HIEZTG");

            HasRequired(p => p.Promille).WithMany().HasForeignKey( p => p.Monat).WillCascadeOnDelete(false);

            HasRequired(p => p.Lgtz).WithMany().HasForeignKey(p => new { p.Monat, p.Plz}).WillCascadeOnDelete(false);

            HasRequired(p => p.BundeslandPlz).WithMany().HasForeignKey(p => new { p.Plz }).WillCascadeOnDelete(false);
            //HasMany( p=> p.)
        }
    }
}
