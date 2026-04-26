using Project3Vitour.Dtos.TourDto;

namespace Project3Vitour.Services.TourServices
{
    //ekleme silme güncelleme vs işlemlerinin methodlarını tutacağız
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllTourAsync();
        Task CreateTourAsync(CreateTourDto createTourDto);
        Task UpdateTourAsync(UpdateTourDto updateTourDto);
        Task DeleteTourAsync(string id);
        Task<GetTourByIdDto> GetTourByIdAsync(string id);
        Task<List<ResultTourDto>> GetToursWithPagingAsync(int page,int pageSize);
        Task<int> GetTotalTourCountAsync();
        Task ChangeStatusAsync(string id);


    }
}
