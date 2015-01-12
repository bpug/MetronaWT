using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class VIEW_MONATDATA_TAGESWERTE_DWDMap : EntityTypeConfiguration<VIEW_MONATDATA_TAGESWERTE_DWD>
    {
        public VIEW_MONATDATA_TAGESWERTE_DWDMap()
        {
            // Primary Key
            this.HasKey(t => t.MoTG);

            // Properties
            this.Property(t => t.WDIMON)
                .HasMaxLength(7);

            this.Property(t => t.MoTG)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VIEW_MONATDATA_TAGESWERTE_DWD");
            this.Property(t => t.WSCODE).HasColumnName("WSCODE");
            this.Property(t => t.WDIMON).HasColumnName("WDIMON");
            this.Property(t => t.MoTG).HasColumnName("MoTG");
            this.Property(t => t.MoTEMPMI).HasColumnName("MoTEMPMI");
            this.Property(t => t.MoHEIZTG).HasColumnName("MoHEIZTG");
            this.Property(t => t.MoTEMPMI_HT).HasColumnName("MoTEMPMI_HT");
            this.Property(t => t.MoHGT15).HasColumnName("MoHGT15");
            this.Property(t => t.MoGT2015).HasColumnName("MoGT2015");
            this.Property(t => t.MoPM).HasColumnName("MoPM");
            this.Property(t => t.MoTNK).HasColumnName("MoTNK");
            this.Property(t => t.MoTXK).HasColumnName("MoTXK");
            this.Property(t => t.MoUPM).HasColumnName("MoUPM");
            this.Property(t => t.MoVPM).HasColumnName("MoVPM");
            this.Property(t => t.MoFMK).HasColumnName("MoFMK");
            this.Property(t => t.MoFX).HasColumnName("MoFX");
            this.Property(t => t.MoNM).HasColumnName("MoNM");
            this.Property(t => t.MoSD).HasColumnName("MoSD");
            this.Property(t => t.MoRS).HasColumnName("MoRS");
        }
    }
}
