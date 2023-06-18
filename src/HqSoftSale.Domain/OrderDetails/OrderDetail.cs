using HqSoftSale.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace HqSoftSale.OrderDetails
{
    public class OrderDetail :  FullAuditedAggregateRoot<Guid>
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
