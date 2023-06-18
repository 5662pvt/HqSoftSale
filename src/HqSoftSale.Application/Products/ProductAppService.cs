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
        Product, //The Product entity
        ProductDto, //Used to show Products
        Guid, //Primary key of the Product entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateProductDto>, //Used to create/update a Product
    IProductAppService //implement the IProductAppService
{
    public ProductAppService(
        IRepository<Product, Guid> repository)
        : base(repository)
    {
    }

    public override async Task<ProductDto> GetAsync(Guid id)
    {
        //Get the IQueryable<Product> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join Products and authors
        var query = from Product in queryable
                    where Product.Id == id
                    select new { Product };

        //Execute the query and get the Product with author
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Product), id);
        }

        var ProductDto = ObjectMapper.Map<Product, ProductDto>(queryResult.Product);
        return ProductDto;
    }

    public override async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Product> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join Products and authors
        var query = from Product in queryable
                    select new { Product };

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of ProductDto objects
        var ProductDtos = queryResult.Select(x =>
        {
            var ProductDto = ObjectMapper.Map<Product, ProductDto>(x.Product);
            return ProductDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<ProductDto>(
            totalCount,
            ProductDtos
        );
    }

    private static string NormalizeSorting(string sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"Product.{nameof(Product.ProductName)}";
        }

        return $"Product.{sorting}";
    }

}
