using Project3Vitour.Dtos.TourDto;

namespace Project3Vitour.Services.ImageService
{
    public interface IImageService
    {
        Task<List<ResultTourImageDto>> GetImagesByTourIdAsync(string tourId);
        Task CreateImageAsync(CreateTourImageDto createTourImageDto);

    }
}
