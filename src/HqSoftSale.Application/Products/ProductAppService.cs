using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace HqSoftSale.Products;

[RemoteService(IsEnabled = true)]
public class ProductAppService :
    CrudAppService<
        Product, 
        ProductDto, 
        Guid, 
        PagedAndSortedResultRequestDto,
        CreateUpdateProductDto>,
    IProductAppService 
{
    public ProductAppService(
        IRepository<Product, Guid> repository)
        : base(repository)
    {
    }
}
