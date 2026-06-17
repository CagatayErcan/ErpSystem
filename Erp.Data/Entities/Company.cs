using System.ComponentModel.DataAnnotations;

namespace Erp.Data.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? TaxNumber { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}