using AutoMapper;
using Project3Vitour.Dtos.CategoryDtos;
using Project3Vitour.Dtos.DestinationDtos;
using Project3Vitour.Dtos.ReviewDtos;
using Project3Vitour.Dtos.SettingsDtos;
using Project3Vitour.Dtos.TourDto;
using Project3Vitour.Dtos.TourPlanDto;
using Project3Vitour.Entities;
using Project3Vitour.Dtos.SettingsDtos;

namespace Project3Vitour.Mapping
{
    public class GeneralMapping:Profile
    {
        //ctor tab ile
        public GeneralMapping()
        {
            
            CreateMap<Category,CreateCategoryDto>().ReverseMap();
            CreateMap<Category,ResultCategoryDto>().ReverseMap();
            CreateMap<Category,UpdateCategoryDto>().ReverseMap();
            CreateMap<Category,GetCategoryByIdDto>().ReverseMap();

            CreateMap<Tour,CreateTourDto>().ReverseMap();
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<Tour,UpdateTourDto>().ReverseMap();
            CreateMap<Tour,GetTourByIdDto>().ReverseMap();

            CreateMap<Review,CreateReviewDto>().ReverseMap();
            CreateMap<Review, UpdateReviewDto>().ReverseMap();
            CreateMap<Review, GetReviewByIdDto>().ReverseMap();
            CreateMap<Review, ResultReviewDto>().ReverseMap();
            CreateMap<Review, ResultReviewByTourIdDto>().ReverseMap();
            CreateMap<TourPlan,GetTourPlanDto>().ReverseMap();

            // Destination için tüm eşleşmeler
            CreateMap<Destination, ResultDestinationDto>().ReverseMap();
            CreateMap<Destination, CreateDestinationDto>().ReverseMap();
            CreateMap<Destination, UpdateDestinationDto>().ReverseMap();
            CreateMap<Destination, GetDestinationByIdDto>().ReverseMap();
            // TourPlan Maplemeleri
            CreateMap<TourPlan, GetTourPlanDto>().ReverseMap();
            CreateMap<TourPlan, CreateTourPlanDto>().ReverseMap();
            CreateMap<TourPlan, UpdateTourPlanDto>().ReverseMap();

            CreateMap<Setting, UpdateSettingsDto>().ReverseMap();

        }
    }
}
