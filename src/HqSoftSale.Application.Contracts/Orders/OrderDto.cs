using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace HqSoftSale.Orders
{
    public class OrderDto : AuditedEntityDto<Guid>
    {  
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; } 
        public string? Customer { get; set; }   
        public OrderStatus OrderStatus { get; set; }
        //public int Quanity { get; set; }
        //public double ExtenedAmount { get; set; }
        public bool IsSelected { get; set; }
    }
}
