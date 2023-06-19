using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace HqSoftSale.Orders
{
    public class Order: FullAuditedAggregateRoot<Guid>
    {
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
   
        public string? Customer { get; set; }

        //public int Quanity { get; set; }
        //public double ExtenedAmount { get; set; }

        public OrderStatus OrderStatus { get; set; }

    }
}
