using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class KalipRecete
    {
        [Key]
        public int Id { get; set; }

        public int UrunId { get; set; }

        [MaxLength(20)]
        public string? StokTipi { get; set; }

        [MaxLength(100)]
        public string? StokAdi { get; set; }

        [MaxLength(20)]
        public string? StokKodu { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal Miktar { get; set; } = 0;

        [MaxLength(10)]
        public string? Birim { get; set; } = "KG";

        [ForeignKey("UrunId")]
        public virtual Urun? Urun { get; set; }
    }
}