using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace HqSoftSale.Products
{
    public class ProductDto : AuditedEntityDto<Guid>
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
