using Project3Vitour.Dtos.ImageDtos;
using Project3Vitour.Dtos.TourDto; // Burayı yeni klasöre göre güncelledik

namespace Project3Vitour.Services.ImageService
{
    public interface IImageService
    {
        Task<List<ResultTourImageDto>> GetImagesByTourIdAsync(string tourId);
        Task CreateImageAsync(CreateTourImageDto createTourImageDto);
        Task DeleteImageAsync(string id); // Eksik olan metodu buraya ekledik
    }
}