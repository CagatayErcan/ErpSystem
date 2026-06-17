using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class UrunErgitmeRecete
    {
        [Key]
        public int Id { get; set; }

        public int UrunId { get; set; }
        [Display(Name = "Stok Tipi")]
        [MaxLength(20)]
        public string? StokTipi { get; set; } // Hammadde, Sarf, Hizmet, Demirbaş, Yarı Mamul, Mamul

        [Display(Name = "Stok Adı")]
        [MaxLength(100)]
        public string? StokAdi { get; set; }

        [Display(Name = "Stok Kodu")]
        [MaxLength(20)]
        public string? StokKodu { get; set; }

        
        [Display(Name = "Miktar")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Miktar { get; set; } = 0;

        [Display(Name = "Birim")]
        [MaxLength(10)]
        public string? Birim { get; set; }

        [Display(Name = "Elektrik Tüketimi (kWh)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BirimElektrikTuketimi { get; set; } = 0;

        [Display(Name = "Ergitme Süresi (dk)")]
        public int? ErgitmeSuresi { get; set; }

        public virtual Urun? Urun { get; set; }
       
    }
}