using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class EventlogDayMap : EntityTypeConfiguration<EventlogDay>
    {
        public EventlogDayMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Ip)
                .HasMaxLength(20);

            this.Property(t => t.Username)
                .HasMaxLength(50);

            this.Property(t => t.Method)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TAB_EVENTLOG_DAY");
            this.Property(t => t.Id).HasColumnName("EVENTLOG_ID");
            this.Property(t => t.Date).HasColumnName("Datum");
            this.Property(t => t.Ip).HasColumnName("IP");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Count).HasColumnName("Anzahl");
            this.Property(t => t.Method).HasColumnName("Method");
        }
    }
}
