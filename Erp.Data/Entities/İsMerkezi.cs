using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class IsMerkezi
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "İş merkezi kodu zorunludur")]
        [MaxLength(20, ErrorMessage = "İş merkezi kodu en fazla 20 karakter olabilir")]
        public string Kod { get; set; } = string.Empty;

        [Required(ErrorMessage = "İş merkezi adı zorunludur")]
        [MaxLength(100, ErrorMessage = "İş merkezi adı en fazla 100 karakter olabilir")]
        public string Ad { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Aciklama { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}