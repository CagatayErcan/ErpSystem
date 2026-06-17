using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class UrunBom
    {
        [Key]
        public int Id { get; set; }

        public int UrunId { get; set; }

        [Display(Name = "Revizyon No")]
        [MaxLength(10)]
        public string RevizyonNo { get; set; } = "REV-001";

        [Display(Name = "Revizyon Tarihi")]
        public DateTime RevizyonTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Revizyon Sebebi")]
        [MaxLength(500)]
        public string? RevizyonSebebi { get; set; }

        [Display(Name = "Oluşturan")]
        [MaxLength(50)]
        public string? Olusturan { get; set; }

        // Navigation Property
        [ForeignKey("UrunId")]
        public virtual Urun? Urun { get; set; }
    }
}