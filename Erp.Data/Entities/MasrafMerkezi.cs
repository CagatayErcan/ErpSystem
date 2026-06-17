using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class MasrafMerkezi
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Masraf merkezi kodu zorunludur")]
        [MaxLength(20, ErrorMessage = "Masraf merkezi kodu en fazla 20 karakter olabilir")]
        public string Kod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Masraf merkezi adı zorunludur")]
        [MaxLength(100, ErrorMessage = "Masraf merkezi adı en fazla 100 karakter olabilir")]
        public string Ad { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? UstMasrafMerkezi { get; set; }

        [Required]
        [MaxLength(10)]
        public string Durum { get; set; } = "Aktif"; // Aktif / Pasif

        [MaxLength(500)]
        public string? Aciklama { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}