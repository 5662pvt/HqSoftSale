using AutoMapper;
using HqSoftSale.Orders;
using HqSoftSale.Products;

namespace HqSoftSale.Blazor;

public class HqSoftSaleBlazorAutoMapperProfile : Profile
{
    public HqSoftSaleBlazorAutoMapperProfile()
    {
        CreateMap<OrderDto, CreateUpdateOrderDto>();
        CreateMap<ProductDto, CreateUpdateProductDto>();
        //Define your AutoMapper configuration here for the Blazor project.
    }
}
