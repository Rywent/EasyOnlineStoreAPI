using AutoMapper;
using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Models;
using EasyOnlineStore.Persistence.Repositories;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Models.Carts;
using EasyOnlineStore.Domain.Models.Orders;


namespace EasyOnlineStore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ProductRepository _productRepository;
        private readonly CartRepository _cartRepository;
        private readonly OrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(OrderRepository orderRepository, ProductRepository productRepository,CartRepository cartRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<List<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<List<OrderResponse>>(orders ?? []);
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if(order == null) 
                throw new NotFoundException(nameof(Order), id);

            return _mapper.Map<OrderResponse>(order);

        }

        public async Task<List<OrderResponse>> GetByPageAsync(int page, int pageSize)
        {
            var orders = await _orderRepository.GetByPageAsync(page, pageSize);
            return _mapper.Map<List<OrderResponse>>(orders ?? []);
        }
        public async Task<OrderResponse> CreateOrderAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart == null)
                throw new NotFoundException(nameof(Cart), cartId);

            var productIds = cart.Items.Select(i => i.ProductId).Distinct().ToArray();
            var products = await _productRepository.GetByIdsAsync(productIds);
            var productsDict = products.ToDictionary(p => p.Id);

            foreach (var cartItem in cart.Items)
            {
                if (!productsDict.TryGetValue(cartItem.ProductId, out var product))
                    throw new NotFoundException(nameof(Product), cartItem.ProductId);
                if (product.Quantity < cartItem.Quantity)
                    throw new InsufficientStockException(product, cartItem.Quantity);
            }

            var newOrder = new Order
            {
                 Id = Guid.NewGuid(),
                 OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd-HHmmss}",
                 CreatedDate = DateTime.UtcNow,
                 Status = OrderStatus.Pending,
                 Items = cart.Items
                .Where(cartItem => productsDict.ContainsKey(cartItem.ProductId))
                .Select(cartItem => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = productsDict[cartItem.ProductId].Price,
                    WarehouseId = productsDict[cartItem.ProductId].WarehouseId
                }).ToList()
            };


            foreach (var orderItem in newOrder.Items)
            {
                productsDict[orderItem.ProductId].Quantity -= orderItem.Quantity;
                
            }

            foreach (var product in productsDict.Values)
                await _productRepository.UpdateAsync(product);

            await _orderRepository.CreateAsync(newOrder);
            await _cartRepository.ClearCartAsync(cartId);

            return _mapper.Map<OrderResponse>(newOrder);
        }

        public async Task<OrderResponse> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException(nameof(Order), orderId);

            order.Status = status;

            var updatedOrder = await _orderRepository.UpdateAsync(order);

            if (updatedOrder == null)
                throw new InvalidOperationException("Failed to update order");
            return _mapper.Map<OrderResponse>(updatedOrder);
        }

        public async Task<OrderResponse> CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException(nameof(Order), orderId);

            if (order.Status != OrderStatus.Pending)
                throw new InvalidOrderStatusException(order.Status, "cancel");


            var productIds = order.Items.Select(i => i.ProductId).Distinct().ToArray();
            var products = await _productRepository.GetByIdsAsync(productIds);

            foreach (var item in order.Items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
                    
            }

            order.Status = OrderStatus.Cancelled;
            var cancelledOrder = await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderResponse>(cancelledOrder);
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException(nameof(Order), orderId);
            return await _orderRepository.RemoveAsync(orderId);

        }

    }
}
