using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class SatisSiparis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string SiparisNo { get; set; } = string.Empty; // Otomatik: SS-0001

        [Required]
        public DateTime SiparisTarihi { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string? CariKodu { get; set; }  // Cari Kartındaki CariKodu ile eşleşecek

        [MaxLength(50)]
        public string? MusteriSiparisNo { get; set; }

        public DateTime? TerminTarihi { get; set; }

        [MaxLength(500)]
        public string? TeslimatAdresi { get; set; }

        [MaxLength(50)]
        public string? SatisTemsilcisi { get; set; }

        [MaxLength(3)]
        public string? DovizTuru { get; set; } = "TL";

        [Column(TypeName = "decimal(18,4)")]
        public decimal Kur { get; set; } = 0;

        [MaxLength(50)]
        public string? OdemeSekli { get; set; }

        [MaxLength(50)]
        public string? TeslimatSekli { get; set; } // EXW, FCA, FOB, CIF, DAP, DDP, vb.

        [MaxLength(500)]
        public string? Aciklama { get; set; }

        [MaxLength(30)]
        public string? Durum { get; set; } = "Planlama Onayı Bekliyor";

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Property - CariKodu ile ilişki
        [ForeignKey("CariKodu")]
        public virtual Cari? Cari { get; set; }

        public virtual ICollection<SatisSiparisDetay>? Detaylar { get; set; }
    }
}