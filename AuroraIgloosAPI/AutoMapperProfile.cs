//using AutoMapper;
//using AuroraIgloosAPI.Models;
//using AuroraIgloosAPI.DTOs;
//namespace AuroraIgloosAPI
//{
//    public class AutoMapperProfile : Profile
//    {
//        public AutoMapperProfile()
//        {
//            CreateMap<Address, AddressDTO>();

//            CreateMap<User, UserDTO>()
//                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

//            CreateMap<Customer, CustomerDTO>()
//                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

//            CreateMap<Employee, EmployeeDTO>()
//                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
//                .ForMember(dest => dest.EmployeeRole, opt => opt.MapFrom(src => src.Role));

//            CreateMap<EmployeeRole, EmployeeRoleDTO>();

//            CreateMap<Igloo, IglooDTO>();

//            CreateMap<Booking, BookingDTO>()
//                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
//                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee))
//                .ForMember(dest => dest.Igloo, opt => opt.MapFrom(src => src.Igloo))
//                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod));



//        }
//    }
//}
