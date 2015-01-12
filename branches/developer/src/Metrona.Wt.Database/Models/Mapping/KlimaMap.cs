using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
   
    using Metrona.Wt.Model.Klima;

    public class KlimaMap : EntityTypeConfiguration<Klima>
    {
        public KlimaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.WsDienst)
                .HasMaxLength(8);

            this.Property(t => t.OpId)
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("TAB_KLIMA");
            this.Property(t => t.Id).HasColumnName("KLIMA_ID");
            this.Property(t => t.WsCode).HasColumnName("WSCODE");
            this.Property(t => t.WsDienst).HasColumnName("WDIENS");
            this.Property(t => t.Datum).HasColumnName("WDITAG");
            this.Property(t => t.Temperatur).HasColumnName("TEMPMI");
            this.Property(t => t.IsHeizTag).HasColumnName("HEIZTG");
            this.Property(t => t.Hgt15).HasColumnName("HGT15");
            this.Property(t => t.Gt2015).HasColumnName("GT2015");
            this.Property(t => t.AendDatum).HasColumnName("AENDDT");
            this.Property(t => t.OpId).HasColumnName("OPID");
            this.Property(t => t.Gtprom).HasColumnName("GTPROM");
            this.Property(t => t.Pm).HasColumnName("PM");
            this.Property(t => t.Txk).HasColumnName("TXK");
            this.Property(t => t.Tnk).HasColumnName("TNK");
            this.Property(t => t.Vpm).HasColumnName("VPM");
            this.Property(t => t.Upm).HasColumnName("UPM");
            this.Property(t => t.Fmk).HasColumnName("FMK");
            this.Property(t => t.Fx).HasColumnName("FX");
            this.Property(t => t.Nm).HasColumnName("NM");
            this.Property(t => t.Sd).HasColumnName("SD");
            this.Property(t => t.Rs).HasColumnName("RS");

            //relationship  
            //HasRequired(t=>t.Station).WithMany( c => c.Klimas).HasForeignKey( mc => mc.WsCode).WillCascadeOnDelete(false);
        }
    }
}
