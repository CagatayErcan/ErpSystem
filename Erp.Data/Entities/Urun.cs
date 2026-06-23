using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class Urun
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Ürün Kodu")]
        [Required(ErrorMessage = "Ürün kodu zorunludur")]
        [MaxLength(20)]
        public string UrunKodu { get; set; } = string.Empty;

        [Display(Name = "Ürün Adı")]
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [MaxLength(100)]
        public string UrunAdi { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? MusteriKisaAd { get; set; }  // CariKisaAd ile eşleşecek

        //[ForeignKey("MusteriKisaAd")]
        //public virtual Cari? Musteri { get; set; }

        [Display(Name = "Alaşım")]
        [MaxLength(50)]
        public string? Alasim { get; set; }

        [Display(Name = "Alaşım Normu")]
        [MaxLength(50)]
        public string? AlasimNormu { get; set; }

        [Display(Name = "Ürün Ağırlığı (kg)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18,3)")]
        public decimal UrunAgirligi { get; set; } = 0;

        [Display(Name = "Ürün Kalıp No")]
        [MaxLength(50)]
        public string? UrunKalipNo { get; set; }

        [Display(Name = "Kalıp Cinsi")]
        [MaxLength(50)]
        public string? KalipCinsi { get; set; }

        [Required(ErrorMessage = "Kalıp ağırlığı zorunludur")]
        [Display(Name = "Kalıp Ağırlığı (kg)")]
        [Column(TypeName = "decimal(18,3)")]
        public decimal KalipAgirligi { get; set; } = 0;

        [Display(Name = "Parça Adeti/Kalıp")]
        public int? UrunParcaAdeti { get; set; }

        [Display(Name = "Kalıp Çevrim Süresi (dk)")]
        public int? KalipCevrimSuresi { get; set; }

        [Display(Name = "Besleyici Stok Kodu")]
        [MaxLength(50)]
        public string? BesleyiciStokKodu { get; set; }

        [Display(Name = "Besleyici Adeti/Kalıp")]
        public int? BesleyiciAdeti { get; set; }

        [Display(Name = "Fire Oranı (%)")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal FireOrani { get; set; } = 0;

        [Display(Name = "Açıklama")]
        [MaxLength(500)]
        public string? Aciklama { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }

        // Navigation Properties
        public virtual ICollection<UrunErgitmeRecete> ErgitmeReceteleri { get; set; } = new List<UrunErgitmeRecete>();
        public virtual ICollection<KalipRecete> KalipReceteleri { get; set; } = new List<KalipRecete>();
        public virtual ICollection<UrunIslemeRecete> IslemeReceteleri { get; set; } = new List<UrunIslemeRecete>();
        public virtual ICollection<UrunBom> UrunBomlar { get; set; } = new List<UrunBom>();
        public virtual ICollection<MacaRecete> MacaReceteleri { get; set; } = new List<MacaRecete>();
    }
}