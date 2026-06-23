using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class AlasimHammaddeOranlari
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string AlasimSinifi { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string StokTipi { get; set; } = string.Empty; // Hammadde, Sarf, vb.

        [Required]
        [MaxLength(20)]
        public string HammaddeKodu { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string HammaddeAdi { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Oran { get; set; } = 0;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}