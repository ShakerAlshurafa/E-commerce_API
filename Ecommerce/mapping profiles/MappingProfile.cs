using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;

namespace Ecommerce.mapping_profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(To => To.Category_Name, 
                    from => from.MapFrom(x => x.Category != null ? x.Category.Name : null));

            CreateMap<LocalUser, LocalUserDTO>().ReverseMap();
        }
    }
}
