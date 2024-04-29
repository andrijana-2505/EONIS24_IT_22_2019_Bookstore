using AutoMapper;
using BackendBookstore.DTOs.CreateDTO;
using BackendBookstore.DTOs.ReadDTO;
using BackendBookstore.DTOs.UpdateDTO;
using BackendBookstore.Models;

namespace BackendBookstore.Profiles
{
    public class AllProfiles : Profile
    {
           public AllProfiles()
            {
                CreateMap<Address, AddressCreateDto>();
                CreateMap<AddressCreateDto, Address>();
                CreateMap<Address, AddressReadDto>();
                CreateMap<Address, Address>();
                CreateMap<Address, AddressUpdateDto>();
                CreateMap<AddressUpdateDto, Address>();

                CreateMap<Book, BookUpdateDto>();
                CreateMap<BookUpdateDto, Book>();
                CreateMap<Book, BookReadDto>();
                CreateMap<Book, Book>();
                CreateMap<Book, BookUpdateDto>();
                CreateMap<BookUpdateDto, Book>();

                CreateMap<Category, CategoryCreateDto>();
                CreateMap<CategoryCreateDto, Category>();
                CreateMap<Category, CategoryReadDto>();
                CreateMap<Category, Category>();
                CreateMap<Category, CategoryUpdateDto>();
                CreateMap<CategoryUpdateDto, Category>();

                CreateMap<Orderitem, OrderItemCreateDto>();
                CreateMap<OrderItemCreateDto, Orderitem>();
                CreateMap<Orderitem, OrderItemReadDto>();
                CreateMap<Orderitem, Orderitem>();
                CreateMap<Orderitem, OrderItemUpdateDto>();
                CreateMap<OrderItemUpdateDto, Orderitem>();

                CreateMap<Order, OrderCreateDto>();
                CreateMap<OrderCreateDto, Order>();
                CreateMap<Order, OrderReadDto>();
                CreateMap<Order, Order>();
                CreateMap<OrderReadDto, Order>();
                CreateMap<Order, OrderUpdateDto>();
                CreateMap<OrderUpdateDto, Order>();

                CreateMap<Review, ReviewCreateDto>();
                CreateMap<ReviewCreateDto, Review>();
                CreateMap<Review, ReviewReadDto>();
                CreateMap<Review, Review>();
                CreateMap<Review, ReviewUpdateDto>();
                CreateMap<ReviewUpdateDto, Review>();

                CreateMap<User, UserCreateDto>();
                CreateMap<UserCreateDto, User>();
                CreateMap<User, UserReadDto>();
                CreateMap<User, User>();
                CreateMap<UserUpdateDto, User>();
                CreateMap<User, UserUpdateDto>();
                CreateMap<User, UserReadDto2>();
        }

    }
}
