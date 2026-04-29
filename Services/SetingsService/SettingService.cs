using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.SettingsDtos;
using Project3Vitour.Entities;
using Project3Vitour.Services.SetingsService;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.SettingsServices
{
    /*Setting Service:Mongodb ile bizzat konuşur ve veriyi işler*/
    //async anahtar kelimesi,o metodun içinde beklemeli bir iş olduğunu belirtir.
    //await:Bu İş Bitene Kadar Diğer İşlere Bak
    public class SettingsService : ISettingsService
    {
        //Veri tabanına crud yaptırabilmek için Imongocollection kasasına erişimimiz olmalı,aslında settings tablosunu kullanmamı sağlayacak.
        private readonly IMongoCollection<Setting> _settingsCollection;
        
        /*Db den gelen veriler Setting tipinde gelir.Ama bizim kullanıcıya UpdateSettingsDto
        göndermemiz gerek.Mapper şunu yapar->Settingsi->Dto ya çevirir*/
        private readonly IMapper _mapper;

        /*Constructor->Sınıfın çalıştırma düğmesidir.*/
        public SettingsService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            //MongoClient->Mongodb nin kapısını çaldı.
            var client = new MongoClient(_databaseSettings.ConnectionString);
            //GetDatabase->Vitour isimli db kutusunu açtı.
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            //_settingsCollection->Bu kutunun içindeki Settings tablosunu alıp değişken içine koydu.
            _settingsCollection = database.GetCollection<Setting>(_databaseSettings.SettingsCollectionName);
            _mapper = mapper;
        }

        public async Task<UpdateSettingsDto> GetSettingsAsync()
        {
            //Find(x=>true)->Tüm akyıtlara bak.
            //FirstOrDefaultAsync()->Bulduğun ilk şeyi getir
            var values = await _settingsCollection.Find(x => true).FirstOrDefaultAsync();
            if (values == null) return null;
            /*Mapper sayesinde ,entity içindeki teknik ham detayları alıp
            kullanıcıya gösterilecek dto paketine dönüştürür*/
            return _mapper.Map<UpdateSettingsDto>(values);
        }

        public async Task UpdateSettingsAsync(UpdateSettingsDto updateSettingsDto)
        {
             //Db deki eski veriyi sorguluyorum.Özellikle şifremi kaybetmemek için.
            var existingSetting = await _settingsCollection.Find(x => x.SettingID == updateSettingsDto.SettingID).FirstOrDefaultAsync();

            //Kullanıcıdan gelen paketi(dto'm),mongodb'nin anlayacağı ham format(settingse)geri çevirdim.
            var values = _mapper.Map<Setting>(updateSettingsDto);

            //Örneğin kullanıcı sadece ismini dğeiştirdi,şifreyi boş bıraktı diye şifrei db ye gidip boşluk olarak güncelleme,eski bilgiyi koru dedim.
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