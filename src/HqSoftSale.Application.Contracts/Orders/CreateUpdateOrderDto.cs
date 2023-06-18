using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HqSoftSale.Orders
{
    public class CreateUpdateOrderDto
    {
        [Required]
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Required]
        public string? Customer { get; set; }
        [Required]
        public int Quanity { get; set; }
        [Required]
        public double ExtenedAmount { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Open;   
    }
}