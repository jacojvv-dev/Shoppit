using API.Responses.Cart;
using API.Responses.Product;
using AutoMapper;
using Domain.Entities;

namespace API.Mapping
{
    public class CartItemMapping : Profile
    {
        public CartItemMapping()
        {
            CreateMap<CartItem, CartItemResponse>();
        }
    }
}