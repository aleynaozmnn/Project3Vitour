using Project3Vitour.Dtos.ReservationDto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project3Vitour.Services.ReservationService
{
    public interface IReservationService
    {
        Task CreateReservationAsync(CreateReservationDto createReservationDto);
        Task<int> GetTotalPersonCountByTourIdAsync(string tourId);
        Task<List<ResultReservationDto>> GetAllReservationsAsync();
        Task DeleteReservationAsync(string id);

        
        Task<ResultReservationDto> GetReservationByIdAsync(string id);
    }
}
