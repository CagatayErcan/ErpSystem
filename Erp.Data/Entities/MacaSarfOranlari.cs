using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class MacaSarfOranlari
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string MacaCinsi { get; set; } = string.Empty; // REÇİNELİ, COLD BOX, HOT BOX, BEZİR YAĞLI

        [Required]
        [MaxLength(20)]
        public string StokTipi { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string SarfKodu { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SarfAdi { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Birim { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal Miktar { get; set; } = 0;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}