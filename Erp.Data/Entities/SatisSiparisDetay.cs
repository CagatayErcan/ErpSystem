using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class SatisSiparisDetay
    {
        [Key]
        public int Id { get; set; }

        public int SatisSiparisId { get; set; }

        [Required]
        public int StokId { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal Miktar { get; set; } = 0;

        [MaxLength(10)]
        public string? Birim { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BirimFiyat { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal IskontoOrani { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal KdvOrani { get; set; } = 20;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SatirToplami { get; set; } = 0;

        [MaxLength(500)]
        public string? Aciklama { get; set; }

        [ForeignKey("SatisSiparisId")]
        public virtual SatisSiparis? SatisSiparis { get; set; }

        [ForeignKey("StokId")]
        public virtual Stok? Stok { get; set; }
    }
}