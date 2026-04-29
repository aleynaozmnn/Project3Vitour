using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.TourPlanDto;
using Project3Vitour.Entities;
using Project3Vitour.Services.TourPlanServices;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.TourPlanService
{
    public class TourPlanService : ITourPlanService
    {
        private readonly IMongoCollection<TourPlan> _tourPlanCollection;
        private readonly IMapper _mapper;

        public TourPlanService(IMapper mapper,IDatabaseSettings databaseSettings)
        {
            var client=new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _tourPlanCollection = database.GetCollection<TourPlan>("TourPlans");
            _mapper = mapper;
        }

        public async Task<List<GetTourPlanDto>> GetTourPlanByTourIdAsync(string tourId)
        {
            var values = await _tourPlanCollection
                .Find(x => x.TourId == tourId)
                .ToListAsync();
            return _mapper.Map<List<GetTourPlanDto>>(values);
        }
        public async Task CreateTourPlanAsync(CreateTourPlanDto createTourPlanDto)
        {
            /*CreateTourPlan içindkei verileri alıp,db nesnesi TourPlan'a kopyalar.
             InsertOneAsync ile tek hamlede mongodb'ye fırlator*/
            var value = _mapper.Map<TourPlan>(createTourPlanDto);
            await _tourPlanCollection.InsertOneAsync(value);
        }

        public async Task DeleteTourPlanAsync(string id)
        {
            await _tourPlanCollection.DeleteOneAsync(x => x.TourPlanId == id);
        }
        public async Task UpdateTourPlanAsync(UpdateTourPlanDto updateTourPlanDto)
        {
            var value = _mapper.Map<TourPlan>(updateTourPlanDto);
            /*FindOneAndReplaceAsync: Gidip ilgili id ye sahip planı bulur ve 
            veritabanındaki halini, benim gönderdiğim yeni value nesnesiyle tamamen değiştirir.*/
            await _tourPlanCollection.FindOneAndReplaceAsync(x => x.TourPlanId == updateTourPlanDto.TourPlanId, value);
        }

        public async Task<UpdateTourPlanDto> GetByIdTourPlanAsync(string id)
        {
            var value = await _tourPlanCollection.Find(x => x.TourPlanId == id).FirstOrDefaultAsync();
            /*AutoMapper, eğer isimler aynıysa
            (her iki tarafta da Title varsa) onları otomatik eşleştirir.*/
            return _mapper.Map<UpdateTourPlanDto>(value);
        }


    }
}
