using HqSoftSale.Products;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace HqSoftSale.OrderDetails
{
    public class OrdDetailDto : AuditedEntityDto<Guid>
    {
        public string OrderID { get; set; }
        public string ProductID { get; set; }
        public string? ProductName { get; set; }
        public WarehouseType Type { get; set; }
        public UnitType UnitType { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double ExtenedAmount { get; set; }
    }
}
