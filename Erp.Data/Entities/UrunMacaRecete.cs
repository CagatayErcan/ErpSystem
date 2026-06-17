using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Erp.Data.Entities
{
    public class UrunMacaRecete
    {
        [Key]
        public int Id { get; set; }

        public int UrunId { get; set; }

        [Display(Name = "Maça Adı")]
        [MaxLength(50)]
        public string? MacaAdi { get; set; }

        [Display(Name = "Maça Kodu")]
        [MaxLength(50)]
        public string? MacaKodu { get; set; }

        [Display(Name = "Maça Kullanımı/parça")]
        public int? MacaKullanimAdedi { get; set; }

        [Display(Name = "Maça Cinsi")]
        [MaxLength(30)]
        public string? MacaCinsi { get; set; } // REÇİNELİ, COLD BOX, HOT BOX, BEZİR YAĞLI

        [Display(Name = "Kum Tüketimi/maça")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal KumTuketimi { get; set; } = 0; // kg

        [Display(Name = "Reçine Tüketimi/maça")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RecineTuketimi { get; set; } = 0; // kg

        [Display(Name = "CO2 Tüketimi/maça")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Co2Tuketimi { get; set; } = 0; // kg

        [Display(Name = "Amin Gazı Tüketimi/maça")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AminGazi { get; set; } = 0; // litre

        [Display(Name = "Bezir Yağı Tüketimi/maça")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BezirYagiTuketimi { get; set; } = 0; // kg

        public virtual Urun? Urun { get; set; }
        
    }
}