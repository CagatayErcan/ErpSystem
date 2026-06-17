using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class Stok
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Stok kodu zorunludur")]
        [MaxLength(20, ErrorMessage = "Stok kodu en fazla 20 karakter olabilir")]
        public string StokKodu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stok adı zorunludur")]
        [MaxLength(100, ErrorMessage = "Stok adı en fazla 100 karakter olabilir")]
        public string StokAdi { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? AnaGrup { get; set; }

        [MaxLength(50)]
        public string? AltGrup { get; set; }

        [Required]
        [MaxLength(10)]
        public string StokTipi { get; set; } = "Hammadde"; // Hammadde, Sarf, Hizmet, Demirbaş, Yarı Mamul, Mamul

        [Required]
        [MaxLength(10)]
        public string AnaBirim { get; set; } = "KG"; // KG, ADET, LT, M3, SAAT

        [MaxLength(50)]
        public string? VarsayilanTedarikci { get; set; }

        [MaxLength(50)]
        public string? AlternatifTedarikci { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SonAlisFiyati { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SatisFiyati { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal KdvOrani { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumStok { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal KritikStok { get; set; } = 0;

        [MaxLength(50)]
        public string? GiderKategorisi { get; set; }

        [MaxLength(20)]
        public string? MaliyetTuru { get; set; } // Standart, Ortalama, FIFO

        [MaxLength(50)]
        public string? MasrafMerkezi { get; set; }

        [MaxLength(500)]
        public string? Aciklama { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}