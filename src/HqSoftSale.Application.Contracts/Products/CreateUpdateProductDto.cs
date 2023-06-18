using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HqSoftSale.Products
{
    public class CreateUpdateProductDto
    {
        [Required]
        public string? ProductID { get; set; }

        [Required]
        [StringLength(128)]
        public string? ProductName { get; set; }

        [Required]
        public WarehouseType Type { get; set; }
    
        [Required]
        public UnitType UnitType { get; set; }

        [Required]
        public int Quanity { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public double ExtenedAmount { get; set; }

      
    }
}
