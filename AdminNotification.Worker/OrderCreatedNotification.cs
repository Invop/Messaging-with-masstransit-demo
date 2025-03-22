using Contracts.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminNotification.Worker
{
    public class OrderCreatedNotification : IConsumer<OrderCreated>
    {
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {

            await Task.Delay(1000);
            Console.WriteLine(context.ReceiveContext.InputAddress);
            Console.WriteLine($"Admin Notification -I just consumed a message with OrderId {context.Message.OrderId}, " +
                $"that was created at:{context.Message.CreatedAt}");
        }
    }
}
