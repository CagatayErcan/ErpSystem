using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class MacaRecete
    {
        [Key]
        public int Id { get; set; }

        public int UrunId { get; set; }

        [MaxLength(50)]
        public string? MacaAdi { get; set; }

        [MaxLength(50)]
        public string? MacaKodu { get; set; }

        public int? KullanimAdedi { get; set; }

        [MaxLength(30)]
        public string? MacaCinsi { get; set; }

        public int? MacaCevrimSuresi { get; set; } // dk

        [ForeignKey("UrunId")]
        public virtual Urun? Urun { get; set; }
    }
}