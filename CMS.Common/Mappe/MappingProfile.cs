using AutoMapper;
using CMS_Common.Model;
using CMS_WT_API.Dtos;

namespace CMS_Common.Mappe
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<ProductImage, ProductImageModel>();
            CreateMap<ProductContent, ProductContentModel>();
            CreateMap<Product, ProductModel>();
            CreateMap<ProductMetaData, ProductMetaDataModel>();
            CreateMap<ProductPrice, ProductPriceModel>();
            CreateMap<Category, CategoryModel>();
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
            .ConvertUsing(typeof(PagedResultConverter<,>));

            CreateMap<Article, ArticleModel>();
            CreateMap<Article, ArticleModel>().ReverseMap();

            CreateMap<Role, RoleModel>().ReverseMap();
            CreateMap<RolePermission, RolePermissionModel>().ReverseMap();
            CreateMap<UserRole, UserRoleModel>()
            .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
            .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.RoleID));

            CreateMap<UserRoleModel, UserRole>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.RoleID, opt => opt.MapFrom(src => src.RoleID))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

           
        }
       

        public class PagedResultConverter<TSource, TDestination> : ITypeConverter<PagedResult<TSource>, PagedResult<TDestination>>
        {
            public PagedResult<TDestination> Convert(PagedResult<TSource> source, PagedResult<TDestination> destination, ResolutionContext context)
            {
                var mappedData = context.Mapper.Map<List<TSource>, List<TDestination>>(source.Items);

                return new PagedResult<TDestination>(mappedData, source.TotalRow, source.PageSize, source.PageIndex, source.KeyWord);
            }
        }

    }

}
