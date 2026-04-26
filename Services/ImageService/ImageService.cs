using MongoDB.Driver;
using Project3Vitour.Dtos.TourDto;
using Project3Vitour.Entities;
using Project3Vitour.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Project3Vitour.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IMongoCollection<Image> _imageCollection;
        public ImageService(IDatabaseSettings _databaseSettings)
        {
            var client=new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _imageCollection = database.GetCollection<Image>(_databaseSettings.ImageCollectionName);

        }
        public async Task<List<ResultTourImageDto>> GetImagesByTourIdAsync(string tourId)
        {
            // MongoDB sorgusu: TourId alanı, dışarıdan gelen tourId'ye eşit olanları getir
            var values = await _imageCollection.Find(x => x.TourId == tourId).ToListAsync();

            // Gelen ham verileri (Entity), pakete (DTO) dönüştürüyoruz
            var result = new List<ResultTourImageDto>();
            foreach (var item in values)
            {
                result.Add(new ResultTourImageDto
                {
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    TourId = item.TourId
                });
            }
            return result;
        }
        public async Task CreateImageAsync(CreateTourImageDto createTourImageDto)
        {
            // DTO'dan gelen veriyi veritabanı nesnesine (Entity) dönüştürüyoruz
            var value = new Image
            {
                ImageUrl = createTourImageDto.ImageUrl,
                TourId = createTourImageDto.TourId
            };

            // MongoDB'ye kaydediyoruz
            await _imageCollection.InsertOneAsync(value);
        }
        public async Task DeleteImageAsync(string id)
        {
            await _imageCollection.DeleteOneAsync(x => x.Id == id);
        }

    }
}
