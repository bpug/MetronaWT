using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Username)
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .HasMaxLength(50);

            this.Property(t => t.WsAccessKey)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TAB_USER");
            this.Property(t => t.Id).HasColumnName("user_id");
            this.Property(t => t.Username).HasColumnName("username");
            this.Property(t => t.Password).HasColumnName("password");
            this.Property(t => t.IsAdmin).HasColumnName("admin");
            this.Property(t => t.Ws).HasColumnName("ws");
            this.Property(t => t.WsAccessKey).HasColumnName("wsAccessKey");
            this.Property(t => t.Haus).HasColumnName("haus");
        }
    }
}
