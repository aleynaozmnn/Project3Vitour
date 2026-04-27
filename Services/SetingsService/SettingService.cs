using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.SettingsDtos;
using Project3Vitour.Entities;
using Project3Vitour.Services.SetingsService;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.SettingsServices
{
    public class SettingsService : ISettingsService
    {
        private readonly IMongoCollection<Setting> _settingsCollection;
        private readonly IMapper _mapper;

        public SettingsService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _settingsCollection = database.GetCollection<Setting>(_databaseSettings.SettingsCollectionName);
            _mapper = mapper;
        }

        public async Task<UpdateSettingsDto> GetSettingsAsync()
        {
            var values = await _settingsCollection.Find(x => true).FirstOrDefaultAsync();
            if (values == null) return null;
            return _mapper.Map<UpdateSettingsDto>(values);
        }

        public async Task UpdateSettingsAsync(UpdateSettingsDto updateSettingsDto)
        {
            // Mevcut veriyi veritabanından çekelim (Şifre koruması için)
            var existingSetting = await _settingsCollection.Find(x => x.SettingID == updateSettingsDto.SettingID).FirstOrDefaultAsync();

            var values = _mapper.Map<Setting>(updateSettingsDto);

            
            if (string.IsNullOrEmpty(updateSettingsDto.NewPassword) && existingSetting != null)
            {
                values.NewPassword = existingSetting.NewPassword;
            }

            if (string.IsNullOrEmpty(values.SettingID))
            {
                await _settingsCollection.InsertOneAsync(values);
            }
            else
            {
                 
                await _settingsCollection.FindOneAndReplaceAsync(x => x.SettingID == values.SettingID, values);
            }
        }
    }
}