using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class MasrafMerkezi
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string? Kod { get; set; } // Otomatik: MM-0001

        [Required(ErrorMessage = "Masraf merkezi adı zorunludur")]
        [MaxLength(100)]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ana grup seçimi zorunludur")]
        [MaxLength(50)]
        public string? AnaGrup { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Durum { get; set; } = "Aktif";

        [MaxLength(500)]
        public string? Aciklama { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}