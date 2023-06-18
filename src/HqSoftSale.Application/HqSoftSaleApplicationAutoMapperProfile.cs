using AutoMapper;
using HqSoftSale.OrderDetails;
using HqSoftSale.Orders;
using HqSoftSale.Products;

namespace HqSoftSale;

public class HqSoftSaleApplicationAutoMapperProfile : Profile
{
    public HqSoftSaleApplicationAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();


        CreateMap<Order, OrderDto>();
        CreateMap<CreateUpdateOrderDto, Order>();

        CreateMap<OrderDetail, OrdDetailDto>();
        CreateMap<CreateUpdateOrdDetailsDto, OrderDetail>();
    }
}
