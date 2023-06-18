using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace HqSoftSale.OrderDetails
{
    public class GetOrdDetailListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? OrderID { get; set; }
    }
}
