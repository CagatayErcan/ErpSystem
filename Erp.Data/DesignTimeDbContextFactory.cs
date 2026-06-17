using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Erp.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ErpDbContext>
    {
        public ErpDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ErpDbContext>();

            // *** ŞİFRENİZİ AŞAĞIDAKİ YERE YAZIN ***
            optionsBuilder.UseSqlServer("Server=localhost\\ERP;Database=ErpDatabase;Trusted_Connection=False;User Id=sa;Password=1;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=False");

            return new ErpDbContext(optionsBuilder.Options);
        }
    }
}