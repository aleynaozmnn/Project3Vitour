using Project3Vitour.Dtos.TourPlanDto;

namespace Project3Vitour.Services.TourPlanServices
{
    public interface ITourPlanService
    {
        Task<List<GetTourPlanDto>> GetTourPlanByTourIdAsync(string tourId);
        Task CreateTourPlanAsync(CreateTourPlanDto createTourPlanDto); // Yeni ekledik
        Task DeleteTourPlanAsync(string id);
        Task UpdateTourPlanAsync(UpdateTourPlanDto updateTourPlanDto);
        Task<UpdateTourPlanDto> GetByIdTourPlanAsync(string id);
    }
}