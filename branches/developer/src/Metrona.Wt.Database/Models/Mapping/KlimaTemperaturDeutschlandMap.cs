using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Metrona.Wt.Database.Models.Mapping
{
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Klima;

    public class KlimaTemperaturDeutschlandMap : EntityTypeConfiguration<KlimaTemperaturDeutschland>
    {
        public KlimaTemperaturDeutschlandMap()
        {
            // Primary Key
            this.HasKey(t => t.Datum);

            // Properties
            // Table & Column Mappings
            this.ToTable("view_klima_temp_deutschland_avg");
            this.Property(t => t.Datum).HasColumnName("WDITAG");
            this.Property(t => t.Temperatur).HasColumnName("TEMPMI");
        }
    }
}
