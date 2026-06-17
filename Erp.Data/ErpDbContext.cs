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
        public DbSet<UrunMacaRecete> UrunMacaReceteleri { get; set; }
        public DbSet<UrunIslemeRecete> UrunIslemeReceteleri { get; set; }
        public DbSet<UrunBom> UrunBomlar { get; set; }
        public DbSet<UrunBomDetay> UrunBomDetaylar { get; set; }
    }
}