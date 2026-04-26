using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.TourDto;
using Project3Vitour.Entities;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;

        public TourService(IMapper mapper,IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database=client.GetDatabase(_databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task CreateTourAsync(CreateTourDto createTourDto)
        {
            var values = _mapper.Map<Tour>(createTourDto);
            await _tourCollection.InsertOneAsync(values);
            
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x=>x.TourId==id);
        }

        public async Task<List<ResultTourDto>> GetAllTourAsync()
        {
            var values = await _tourCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<int> GetTotalTourCountAsync()
        {
            var counter = await _tourCollection.CountDocumentsAsync(x => true);
            return (int)counter;
        }

        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            var values = await _tourCollection.Find(x => x.TourId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourByIdDto>(values);
        }

        public async Task<List<ResultTourDto>> GetToursWithPagingAsync(int page, int pageSize)
        {
            //await:veri tabanından cevap alana akdar bekle
            //pageSize:Bir sayfada kaç tane tur olsun,gösterilsin
            var values = await _tourCollection.Find(x => true)//Filtreleme yapmadan bana her şeyi getir
                .Skip((page - 1) * pageSize)//Eğer 2. sayfadaysan ve her sayfada 6 tur varsa ilk 6 turu görmezden gel/skip 
                .Limit(pageSize)//Durdugun yerden itibaren bana sadece pageSize kadar tur getir
                .ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
             
        }

        public async Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            var values=_mapper.Map<Tour>(updateTourDto);
            await _tourCollection.FindOneAndReplaceAsync(x => x.TourId == updateTourDto.TourId, values);
        }
        public async Task ChangeStatusAsync(string id)
        {
            var tour = await _tourCollection.Find(x => x.TourId == id).FirstOrDefaultAsync();
            if (tour != null)
            {
                // Mevcut durumun tersini alıyoruz
                tour.IsStatus = !tour.IsStatus;
                await _tourCollection.FindOneAndReplaceAsync(x => x.TourId == id, tour);
            }
        }
    }
}
