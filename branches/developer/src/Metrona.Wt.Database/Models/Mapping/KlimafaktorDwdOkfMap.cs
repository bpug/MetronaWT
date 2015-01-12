using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class KlimafaktorDwdOkfMap : EntityTypeConfiguration<KlimafaktorDwdOkf>
    {
        public KlimafaktorDwdOkfMap()
        {
            // Primary Key
            this.HasKey(t => t.Klimafaktor_ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("TAB_KLIMAFAKTOR_DWD_OKF");
            this.Property(t => t.Klimafaktor_ID).HasColumnName("Klimafaktor_ID");
            this.Property(t => t.PLZ).HasColumnName("PLZ");
            this.Property(t => t.DATUM_VON).HasColumnName("DATUM_VON");
            this.Property(t => t.DATUM_BIS).HasColumnName("DATUM_BIS");
            this.Property(t => t.KF).HasColumnName("KF");
        }
    }
}
