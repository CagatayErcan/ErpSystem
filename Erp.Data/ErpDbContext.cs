using Microsoft.EntityFrameworkCore;
using Erp.Data.Entities;

namespace Erp.Data
{
    public class ErpDbContext : DbContext
    {
        public ErpDbContext(DbContextOptions<ErpDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Cari> Cariler { get; set; }
        public DbSet<User> Users { get; set; }  
        public DbSet<Stok> Stoklar { get; set; }
        public DbSet<MasrafMerkezi> MasrafMerkezleri { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<UrunErgitmeRecete> UrunErgitmeReceteleri { get; set; }
        public DbSet<KalipRecete> KalipReceteleri { get; set; }
        public DbSet<MacaRecete> MacaReceteleri { get; set; }
        public DbSet<UrunIslemeRecete> UrunIslemeReceteleri { get; set; }
        public DbSet<UrunBom> UrunBomlar { get; set; }
        public DbSet<UrunBomDetay> UrunBomDetaylar { get; set; }
        public DbSet<AlasimHammaddeOranlari> AlasimHammaddeOranlari { get; set; }
        public DbSet<KalipSarfOranlari> KalipSarfOranlari { get; set; }
        public DbSet<MacaSarfOranlari> MacaSarfOranlari { get; set; }
        public DbSet<EnerjiStandartlari> EnerjiStandartlari { get; set; }
        public DbSet<IsMerkezi> IsMerkezleri { get; set; }
        public DbSet<SatisSiparis> SatisSiparisleri { get; set; }
        public DbSet<SatisSiparisDetay> SatisSiparisDetaylari { get; set; }
        public DbSet<StokTipi> StokTipleri { get; set; }
        public DbSet<AnaGrup> AnaGruplari { get; set; }
        public DbSet<AltGrup> AltGruplari { get; set; }
        public DbSet<GiderKategorisi> GiderKategorileri { get; set; }
    }
}