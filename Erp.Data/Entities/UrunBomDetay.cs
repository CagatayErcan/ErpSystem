using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class UrunBomDetay
    {
        [Key]
        public int Id { get; set; }

        public int UrunBomId { get; set; }

        [Display(Name = "Stok Kodu")]
        [MaxLength(20)]
        public string? StokKodu { get; set; }

        [Display(Name = "Stok Adı")]
        [MaxLength(100)]
        public string? StokAdi { get; set; }

        [Display(Name = "Miktar")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Miktar { get; set; } = 0;

        [Display(Name = "Birim")]
        [MaxLength(10)]
        public string? Birim { get; set; }

        [Display(Name = "Fire Oranı (%)")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal FireOrani { get; set; } = 0;

        [Display(Name = "Kaynak")]
        [MaxLength(20)]
        public string? Kaynak { get; set; } // Ergitme, Maça, İşleme

        [ForeignKey("UrunBomId")]
        public virtual UrunBom? UrunBom { get; set; }
    }
}