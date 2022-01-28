using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderdbContext;

        public CourseNameChangedEventConsumer(OrderDbContext orderdbContext)
        {
            _orderdbContext = orderdbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItems = await _orderdbContext.OrderItems.Where(x=>x.ProductId==context.Message.CourseId).ToListAsync();
            orderItems.ForEach(x =>
            {
                x.UpdateOrderItem(context.Message.UpdateName, x.PictureUrl, x.Price);
            });

            await _orderdbContext.SaveChangesAsync();

        }
    }
}
