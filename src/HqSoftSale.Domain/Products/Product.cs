using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace HqSoftSale.Products
{
    public class Product : AuditedAggregateRoot<Guid>
    {
        public string? ProductID { get; set; }
        public string? ProductName { get; set; }
        public WarehouseType Type { get; set; }
        public UnitType UnitType { get; set; }
        public int Quanity { get; set; }
        public double Price { get; set; }
        public double ExtenedAmount { get; set; }
    }
}
