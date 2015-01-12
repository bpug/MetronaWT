using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class KlimaDwdMap : EntityTypeConfiguration<KlimaDwd>
    {
        public KlimaDwdMap()
        {
            // Primary Key
            this.HasKey(t => t.KLIMA_ID);

            // Properties
            this.Property(t => t.WSTATI)
                .HasMaxLength(8);

            this.Property(t => t.DATUM)
                .HasMaxLength(8);

            this.Property(t => t.WDIENS)
                .HasMaxLength(8);

            this.Property(t => t.OPID)
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("TAB_KLIMA_TAGESWERTE_DWD");
            this.Property(t => t.KLIMA_ID).HasColumnName("KLIMA_ID");
            this.Property(t => t.WSCODE).HasColumnName("WSCODE");
            this.Property(t => t.WSTATI).HasColumnName("WSTATI");
            this.Property(t => t.DATUM).HasColumnName("DATUM");
            this.Property(t => t.QN).HasColumnName("QN");
            this.Property(t => t.TG).HasColumnName("TG");
            this.Property(t => t.TNK).HasColumnName("TNK");
            this.Property(t => t.TEMPMI).HasColumnName("TEMPMI");
            this.Property(t => t.TXK).HasColumnName("TXK");
            this.Property(t => t.UPM).HasColumnName("UPM");
            this.Property(t => t.FMK).HasColumnName("FMK");
            this.Property(t => t.FX).HasColumnName("FX");
            this.Property(t => t.SD).HasColumnName("SD");
            this.Property(t => t.NM).HasColumnName("NM");
            this.Property(t => t.RS).HasColumnName("RS");
            this.Property(t => t.PM).HasColumnName("PM");
            this.Property(t => t.WDIENS).HasColumnName("WDIENS");
            this.Property(t => t.WDITAG).HasColumnName("WDITAG");
            this.Property(t => t.HEIZTG).HasColumnName("HEIZTG");
            this.Property(t => t.HGT15).HasColumnName("HGT15");
            this.Property(t => t.GT2015).HasColumnName("GT2015");
            this.Property(t => t.AENDDT).HasColumnName("AENDDT");
            this.Property(t => t.OPID).HasColumnName("OPID");
            this.Property(t => t.GTPROM).HasColumnName("GTPROM");
            this.Property(t => t.VPM).HasColumnName("VPM");
        }
    }
}
