using HqSoftSale.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace HqSoftSale.OrderDetails
{
    public interface IOrdDetailAppService :
    ICrudAppService< 
    OrdDetailDto, 
    Guid,
    PagedAndSortedResultRequestDto,
    CreateUpdateOrdDetailsDto> 
    {
        Task<List<OrdDetailDto>> GetProductsByOrderDetail(string orderId);
    }
}
