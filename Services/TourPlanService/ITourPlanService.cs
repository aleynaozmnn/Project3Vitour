using Project3Vitour.Dtos.TourPlanDto;

namespace Project3Vitour.Services.TourPlanServices
{
    public interface ITourPlanService
    {
        //İlgili turun tur planını çekicez bu sayede
        Task<List<GetTourPlanDto>> GetTourPlanByTourIdAsync(string tourId);
        Task CreateTourPlanAsync(CreateTourPlanDto createTourPlanDto); 
        Task DeleteTourPlanAsync(string id);
        Task UpdateTourPlanAsync(UpdateTourPlanDto updateTourPlanDto);
        //X.gün planını getir demek için kullanıcam
        Task<UpdateTourPlanDto> GetByIdTourPlanAsync(string id);
    }
}