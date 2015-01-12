using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class EventlogMap : EntityTypeConfiguration<Eventlog>
    {
        public EventlogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AccessKey)
                .HasMaxLength(50);

            this.Property(t => t.ClientIp)
                .HasMaxLength(20);

            this.Property(t => t.Method)
                .HasMaxLength(50);

            this.Property(t => t.Parameters)
                .HasMaxLength(150);

            this.Property(t => t.SoapMessage)
                .HasMaxLength(16777215);

            this.Property(t => t.ErrMessage)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("TAB_EVENTLOG");
            this.Property(t => t.Id).HasColumnName("EventLog_id");
            this.Property(t => t.AccessKey).HasColumnName("AccessKey");
            this.Property(t => t.CreateAt).HasColumnName("crDate");
            this.Property(t => t.ClientIp).HasColumnName("ClientIP");
            this.Property(t => t.Method).HasColumnName("Method");
            this.Property(t => t.Parameters).HasColumnName("Parameters");
            this.Property(t => t.SoapMessage).HasColumnName("SoapMessage");
            this.Property(t => t.Error).HasColumnName("Error");
            this.Property(t => t.ErrMessage).HasColumnName("ErrMessage");
            this.Property(t => t.UserId).HasColumnName("UserID");
            this.Property(t => t.Region).HasColumnName("Region");
        }
    }
}
