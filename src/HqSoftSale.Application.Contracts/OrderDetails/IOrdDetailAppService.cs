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
    ICrudAppService< //Defines CRUD methods
    OrdDetailDto, //Used to show order detail
    Guid, //Primary key of the order detail entity
    PagedAndSortedResultRequestDto, //Used for paging/sorting
    CreateUpdateOrdDetailsDto> //Used to create/update a order detail
    {
        Task<List<OrdDetailDto>> GetProductsByOrderDetail(string orderId);
    }
}
