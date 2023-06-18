using HqSoftSale.OrderDetails;
using HqSoftSale.Orders;
using HqSoftSale.Products;
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
using Volo.Abp.ObjectMapping;

namespace HqSoftSale.Orders;

[RemoteService(IsEnabled = true)]
public class OrderAppService :
    CrudAppService<
        Order, 
        OrderDto, 
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateOrderDto>, 
    IOrderAppService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<OrderDetail> _orderDetailRepository;
        
    public OrderAppService(
        IRepository<Order, Guid> repository,
        IRepository<Order> orderRepository,
        IRepository<Product> productRepository,
        IRepository<OrderDetail> orderDetailRepository)
        : base(repository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _orderDetailRepository = orderDetailRepository;
    }

    public override async Task<OrderDto> GetAsync(Guid id)
    {  
        var queryable = await Repository.GetQueryableAsync();
        var query = from Order in queryable
                    where Order.Id == id
                    select new { Order };
    
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Order), id);
        }

        var OrderDto = ObjectMapper.Map<Order, OrderDto>(queryResult.Order);
        return OrderDto;
    }

    public override async Task<PagedResultDto<OrderDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await Repository.GetQueryableAsync();
    
        var query = from Order in queryable
                    select new { Order };

        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
       
        var queryResult = await AsyncExecuter.ToListAsync(query);

        var OrderDtos = queryResult.Select(x =>
        {
            var OrderDto = ObjectMapper.Map<Order, OrderDto>(x.Order);
            return OrderDto;
        }).ToList();

        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<OrderDto>(
            totalCount,
            OrderDtos
        );
    }

    private static string NormalizeSorting(string sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"Order.{nameof(Order.OrderNumber)}";
        }
        return $"Order.{sorting}";
    }

    public async Task<List<ProductDto>> GetProductsByOrderDetails(string orderId, string productId)
    {
        var orderDetail = await _orderDetailRepository.FirstOrDefaultAsync(od => od.OrderID == orderId && od.ProductID == productId);
        if (orderDetail == null)
        {
            return new List<ProductDto>();
        }
        var products = await _productRepository.GetListAsync(p => p.ProductID == orderDetail.ProductID);
        return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    }



    //public async Task<List<ProductDto>> GetProductsByOrderDetail(string orderId)
    //{
    //    var orderDetails = await _orderDetailRepository.GetListAsync(od => od.OrderID == orderId);

    //    var products = new List<ProductDto>();


    //    // Vòng lặp để truy vấn danh sách sản phẩm tương ứng với mỗi chi tiết đơn hàng trong danh sách
    //    foreach (var orderDetail in orderDetails)
    //    {
    //        var product = await _productRepository.FirstOrDefaultAsync(p => p.ProductID == orderDetail.ProductID);

    //        if (product != null)
    //        {
    //            products.Add(ObjectMapper.Map<Product, ProductDto>(product));
    //        }
    //    }
    //    return products;
    //}

    //public async Task<List<ProductDto>> GetProductsByOrderDetail(string orderId, string productId)
    //{
    //    var orderDetail = await _orderDetailRepository.FirstOrDefaultAsync(od =>
    //        od.OrderID == orderId && od.ProductID == productId);

    //    if (orderDetail == null)
    //    {
    //        throw new UserFriendlyException("Order detail not found.");
    //    }

    //    var products = await _productRepository.GetListAsync(p =>
    //        p.ProductID == orderDetail.ProductID);

    //    return ObjectMapper.Map<List<Product>, List<ProductDto>>(products);
    //}

    public async Task<Guid> CreateOrderAndOrderDetails(CreateUpdateOrderDto orderDto, CreateUpdateOrdDetailsDto orderDetailDto)
    {   
        var order = new Order
        {
            OrderNumber = orderDto.OrderNumber,
            OrderDate = orderDto.OrderDate,
            Customer =orderDto.Customer,
            Quanity = orderDto.Quanity,
            ExtenedAmount = orderDto.ExtenedAmount,
            OrderStatus = orderDto.OrderStatus,
        };
        var orderDetail = new OrderDetail
        {
            OrderID = order.OrderNumber,
            ProductID = orderDetailDto.ProductID,
            ProductName =  orderDetailDto.ProductName,
            UnitType = orderDetailDto.UnitType,
            Type = orderDetailDto.Type,
            Quantity = orderDetailDto.Quantity,
            Price = orderDetailDto.Price,
            ExtenedAmount = orderDetailDto.ExtenedAmount
        }; 
        await _orderRepository.InsertAsync(order);
        await _orderDetailRepository.InsertAsync(orderDetail);       
        return order.Id;
    }
}
