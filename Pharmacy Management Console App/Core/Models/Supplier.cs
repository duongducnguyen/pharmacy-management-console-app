using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Core.Models
{
    public class Supplier
    {
        public Supplier()
        {
            Purchases = new HashSet<Purchase>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public virtual ICollection<Purchase> Purchases { get; set; }
    }


}