using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class EnerjiStandartlari
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string IsMerkezi { get; set; } = string.Empty; // Ergime, Maçahane, Kalıphane

        [Required]
        [MaxLength(10)]
        public string Birim { get; set; } = string.Empty; // kWh/ton

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tuketim { get; set; } = 0;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}