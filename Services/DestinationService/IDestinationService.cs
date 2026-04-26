using Project3Vitour.Dtos.DestinationDtos;

namespace Project3Vitour.Services.DestinationService
{
    public interface IDestinationService
    {
        Task<List<ResultDestinationDto>> GetAllDestinationAsync();
        Task CreateDestinationAsync(CreateDestinationDto createDestinationDto);
        Task UpdateDestinationAsync(UpdateDestinationDto updateDestinationDto);
        Task DeleteDestinationAsync(string id);
        Task<GetDestinationByIdDto> GetDestinationByIdAsync(string id);
    }
}
