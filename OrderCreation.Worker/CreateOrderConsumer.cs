﻿using AutoMapper;
using Contracts.Events;
using Contracts.Models;
using MassTransit;
using MassTransit.Transports;
using Orders.Domain.Entities;
using Orders.Service;
using OrdersApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCreation.Worker
{
    public class CreateOrderConsumer : IConsumer<OrderModel>
    {
        private readonly IMapper mapper;
        private readonly IOrderService orderService;

        public CreateOrderConsumer(IMapper mapper, IOrderService orderService)
        {
            this.mapper = mapper;
            this.orderService = orderService;
        }


        public async Task Consume(ConsumeContext<OrderModel> context)
        {
            Console.WriteLine($"I got a command to create an order: {context.Message}");

            var orderToAdd = mapper.Map<Order>(context.Message);
            var createdOrder = await orderService.AddOrderAsync(orderToAdd);

            var notifyOrderCreated = context.Publish(new OrderCreated()
            {
                CreatedAt = createdOrder.OrderDate,
                Id = createdOrder.Id,
                OrderId = createdOrder.OrderId,
                TotalAmount = createdOrder.OrderItems.Sum(p => p.Price * p.Quantity)

            }, context =>
            {
                context.Headers.Set("my-custom-header", "value");
               context.TimeToLive = TimeSpan.FromSeconds(1);
            });

            await Task.CompletedTask;
        }
    }
}
