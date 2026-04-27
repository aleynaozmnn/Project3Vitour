using MongoDB.Driver;
using Project3Vitour.Dtos.ImageDtos; // Yeni DTO yolu
using Project3Vitour.Dtos.TourDto;
using Project3Vitour.Entities;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IMongoCollection<Image> _imageCollection;

        public ImageService(IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _imageCollection = database.GetCollection<Image>(_databaseSettings.ImageCollectionName);
        }

        public async Task<List<ResultTourImageDto>> GetImagesByTourIdAsync(string tourId)
        {
            // Veritabanından verileri çekiyoruz
            var values = await _imageCollection.Find(x => x.TourId == tourId).ToListAsync();

            // LINQ ile daha temiz bir dönüşüm (Select kullanarak)
            return values.Select(item => new ResultTourImageDto
            {
                Id = item.Id,
                ImageUrl = item.ImageUrl,
                TourId = item.TourId
            }).ToList();
        }

        public async Task CreateImageAsync(CreateTourImageDto createTourImageDto)
        {
            var value = new Image
            {
                ImageUrl = createTourImageDto.ImageUrl,
                TourId = createTourImageDto.TourId
            };
            await _imageCollection.InsertOneAsync(value);
        }

        public async Task DeleteImageAsync(string id)
        {
            await _imageCollection.DeleteOneAsync(x => x.Id == id);
        }
    }
}