using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages;
using FreeCourse.Shared.Services;
using FreeCourse.Shared.Dtos;
using mass=MassTransit;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Consumers
{
    public class BasketCourseNameChangedEventConsumer : mass.IConsumer<CourseNameChangedEvent>
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IBasketService _basketService;

        public BasketCourseNameChangedEventConsumer(ISharedIdentityService sharedIdentityService, IBasketService basketService)
        {
            _sharedIdentityService = sharedIdentityService;
            _basketService = basketService;
        }


        public async Task Consume(mass.ConsumeContext<CourseNameChangedEvent> context)
        {
            var allBasket = await _basketService.GetAllBasket();
            if (!allBasket.IsSuccessful)
            {
                throw new System.Exception(string.Join(",", allBasket.Errors));
            }
            allBasket.Data.ForEach(async basket =>
            {
                basket.BasketItems.ForEach(item =>
                {
                    if (item.CourseId==context.Message.CourseId)
                    {
                        item.CourseName = context.Message.UpdateName;
                    }
                });
                await _basketService.SaveOrUpdate(basket);
            });
          
        }
    }
}
