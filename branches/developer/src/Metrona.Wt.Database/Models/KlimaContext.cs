using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Metrona.Wt.Database.Models.Mapping;

namespace Metrona.Wt.Database.Models
{
    using Metrona.Wt.Model;
    using Metrona.Wt.Model.Klima;
    using Metrona.Wt.Model.Meteo;

    public partial class KlimaContext : DbContext
    {
        static KlimaContext()
        {
            System.Data.Entity.Database.SetInitializer<KlimaContext>(new KlimaContextInitializer());
        }

        public KlimaContext()
            : base("Name=Brunata.KlimaContext")
        {
        }

        public DbSet<Bundesland> Bundeslands { get; set; }
        public DbSet<BundeslandPlz> BundeslandPlzes { get; set; }
        public DbSet<Eventlog> TAB_EVENTLOG { get; set; }
        public DbSet<EventlogDay> TAB_EVENTLOG_DAY { get; set; }
        public DbSet<Klima> Klimas { get; set; }
        public DbSet<KlimaDwd> KlimaDwds { get; set; }
        public DbSet<KlimafaktorDwd> KlimafaktorDwds { get; set; }
        public DbSet<KlimafaktorDwdOkf> KlimafaktorDwdOkfs { get; set; }
        public DbSet<LangzeitGtzDwd> TAB_LANGZEITGTZ_TAGESWERT_DWD { get; set; }
        public DbSet<Langzeitmittel> Langzeitmittels { get; set; }
        public DbSet<MeteoGtz> MeteoGtzs { get; set; }
        public DbSet<MeteoLangGtz> MeteoLangGtzes { get; set; }
        public DbSet<MeteoFaktor> MeteoMfaktors { get; set; }
        public DbSet<Promille> Promilles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WetterStation> WetterStations { get; set; }
        public DbSet<WetterStationPlz> WetterStationPlzes { get; set; }
        public DbSet<VIEW_EVENTLOG> VIEW_EVENTLOG { get; set; }
        public DbSet<KlimaTemperaturBundesland> KlimaTemperaturBundeslands { get; set; }
        public DbSet<KlimaTemperaturDeutschland> KlimaTemperaturDeutschlands { get; set; }
        public DbSet<VIEW_LANG_GTZMONAT> VIEW_LANG_GTZMONAT { get; set; }
        public DbSet<MeteoGtzBundesland> MeteoGtzBundeslands { get; set; }
        public DbSet<MeteoGtzDeutschland> MeteoGtzDeutschlands { get; set; }

        public DbSet<MeteoLangGtzBundesland> view_meteo_langgtz_bundesland_avg { get; set; }
        public DbSet<VIEW_MONATDATA> VIEW_MONATDATA { get; set; }
        public DbSet<VIEW_MONATDATA_TAGESWERTE_DWD> VIEW_MONATDATA_TAGESWERTE_DWD { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BundeslandMap());
            modelBuilder.Configurations.Add(new BundeslandPlzMap());
            modelBuilder.Configurations.Add(new EventlogMap());
            modelBuilder.Configurations.Add(new EventlogDayMap());
            modelBuilder.Configurations.Add(new KlimaMap());
            modelBuilder.Configurations.Add(new KlimaDwdMap());
            modelBuilder.Configurations.Add(new KlimafaktorDwdMap());
            modelBuilder.Configurations.Add(new KlimafaktorDwdOkfMap());
            modelBuilder.Configurations.Add(new TAB_LANGZEITGTZ_TAGESWERT_DWDMap());
            modelBuilder.Configurations.Add(new LangzeitmittelMap());
            modelBuilder.Configurations.Add(new MeteoGtzMap());
            modelBuilder.Configurations.Add(new MeteoLangGtzMap());
            modelBuilder.Configurations.Add(new MeteoFaktorMap());
            modelBuilder.Configurations.Add(new PromilleMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new WetterStationMap());
            modelBuilder.Configurations.Add(new WetterStationPlzMap());
            modelBuilder.Configurations.Add(new VIEW_EVENTLOGMap());
            modelBuilder.Configurations.Add(new KlimaTemperaturBundeslandMap());
            modelBuilder.Configurations.Add(new KlimaTemperaturDeutschlandMap());
            modelBuilder.Configurations.Add(new VIEW_LANG_GTZMONATMap());
            modelBuilder.Configurations.Add(new MeteoGtzBundeslandMap());
            modelBuilder.Configurations.Add(new MeteoGtzDeutschlandMap());
            modelBuilder.Configurations.Add(new MeteoLangGtzBundeslandMap());
            modelBuilder.Configurations.Add(new MeteoLangGtzDeutschlandMap());
            modelBuilder.Configurations.Add(new VIEW_MONATDATAMap());
            modelBuilder.Configurations.Add(new VIEW_MONATDATA_TAGESWERTE_DWDMap());
        }
    }
}
