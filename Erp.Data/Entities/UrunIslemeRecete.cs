using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class UrunIslemeRecete
    {
        [Key]
        public int Id { get; set; }

        public int UrunId { get; set; }

        public int? OperasyonSırası { get; set; }

        [Display(Name = "Operasyon Kodu")]
        [MaxLength(20)]
        public string? OperasyonKodu { get; set; }

        [Display(Name = "Operasyon Adı")]
        [MaxLength(100)]
        public string? OperasyonAdi { get; set; }

        [Display(Name = "İş Merkezi")]
        [MaxLength(50)]
        public string? IsMerkezi { get; set; }

        [Display(Name = "TakımTüketimi")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TakimTuketimi { get; set; } = 0;

        [Display(Name = "Stok Kodu")]
        [MaxLength(20)]
        public string? StokKodu { get; set; }

        [Display(Name = "Stok Adı")]
        [MaxLength(100)]
        public string? StokAdi { get; set; }

        [Display(Name = "Birim Elektrik Tüketimi")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BirimElektrikTuketimi { get; set; } = 0; // kWh

        public virtual Urun? Urun { get; set; }
        
    }
}