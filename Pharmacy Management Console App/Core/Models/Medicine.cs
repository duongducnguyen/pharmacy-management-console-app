using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Core.Models
{
    public class Medicine
    {
        public Medicine()
        {
            OrderDetails = new HashSet<OrderDetail>();
            PurchaseDetails = new HashSet<PurchaseDetail>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public string Unit { get; set; }

        public DateTime ExpiryDate { get; set; }

        public int CategoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }


}