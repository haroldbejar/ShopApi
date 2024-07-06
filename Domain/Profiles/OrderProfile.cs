using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();
        }
    }
}
