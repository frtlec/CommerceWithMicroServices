﻿using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;
        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);
            return status ?
            Response<bool>.Success(204) :
            Response<bool>.Fail("Basket not found", 404);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(userId);
            if (String.IsNullOrEmpty(existBasket))
            {
                return Response<BasketDto>.Fail("Basket Not Found",404);
            }
            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket),200);
        }
       
        public async Task<Response<List<BasketDto>>> GetAllBasket()
        {
            var db =  _redisService.GetDb();

            var keys = _redisService.GetKeys(db);

            List<BasketDto> baskets = new();
            foreach (var userId in keys)
            {
                var basket=await GetBasket(userId);
                if (!basket.IsSuccessful)
                {
                    throw new Exception("Couldn't get basket");
                }
                baskets.Add(basket.Data);
            }


            return Response<List<BasketDto>>.Success(baskets, 200);

        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status =  await _redisService.GetDb().StringSetAsync(basketDto.UserId,JsonSerializer.Serialize(basketDto));
            return status ? 
                Response<bool>.Success(204) : 
                Response<bool>.Fail("Basket could not uptade or save", 500);
        }
    }
}
