using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class Cari
    {
        [Key]
        [MaxLength(20)]
        public string CariKodu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cari ünvanı zorunludur")]
        [MaxLength(200, ErrorMessage = "Cari ünvanı en fazla 200 karakter olabilir")]
        public string CariUnvani { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? CariKisaAd { get; set; }

        [Required]
        [MaxLength(20)]
        public string CariTipi { get; set; } = "Müşteri"; // Müşteri / Tedarikçi / Her İkisi

        [Required]
        [MaxLength(10)]
        public string Durumu { get; set; } = "Aktif"; // Aktif / Pasif

        [MaxLength(50)]
        public string? VergiDairesi { get; set; }

        [MaxLength(20)]
        public string? VergiNo { get; set; }

        [MaxLength(11)]
        public string? TCKN { get; set; }

        [MaxLength(16)]
        public string? MersisNo { get; set; }

        [MaxLength(30)]
        public string? TicaretSicilNo { get; set; }

        [MaxLength(100)]
        public string? YetkiliKisi { get; set; }

        [MaxLength(20)]
        public string? Telefon { get; set; }

        [MaxLength(100)]
        public string? EPosta { get; set; }

        [MaxLength(100)]
        public string? WebSitesi { get; set; }

        [MaxLength(50)]
        public string? Ulke { get; set; }

        [MaxLength(50)]
        public string? Il { get; set; }

        [MaxLength(50)]
        public string? Ilce { get; set; }

        [MaxLength(10)]
        public string? PostaKodu { get; set; }

        [MaxLength(500)]
        public string? Adres { get; set; }

        [MaxLength(50)]
        public string? SatisTemsilcisi { get; set; }

        [MaxLength(50)]
        public string? Bolge { get; set; }

        [MaxLength(50)]
        public string? Sektor { get; set; }

        [MaxLength(3)]
        public string? DovizTuru { get; set; } = "TL";

        [Column(TypeName = "decimal(18,2)")]
        public decimal RiskLimiti { get; set; } = 0;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}