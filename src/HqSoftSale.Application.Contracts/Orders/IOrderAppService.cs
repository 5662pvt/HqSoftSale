using HqSoftSale.OrderDetails;
using HqSoftSale.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HqSoftSale.Orders;

public interface IOrderAppService :
    ICrudAppService<
        OrderDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateOrderDto> 
{
    Task<Guid> CreateOrderAndOrderDetails(CreateUpdateOrderDto orderDto, CreateUpdateOrdDetailsDto orderDetailDto);
}
