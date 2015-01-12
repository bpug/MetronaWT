using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class VIEW_EVENTLOGMap : EntityTypeConfiguration<VIEW_EVENTLOG>
    {
        public VIEW_EVENTLOGMap()
        {
            // Primary Key
            this.HasKey(t => t.Datum);

            // Properties
            this.Property(t => t.IP)
                .HasMaxLength(20);

            this.Property(t => t.Username)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIEW_EVENTLOG");
            this.Property(t => t.Datum).HasColumnName("Datum");
            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Anzahl).HasColumnName("Anzahl");
        }
    }
}
