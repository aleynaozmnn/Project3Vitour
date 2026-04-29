using Project3Vitour.Dtos.SettingsDtos;

namespace Project3Vitour.Services.SetingsService
{
    public interface ISettingsService
    {
        //Bu interface ile çalışmak isteyen herkes bu 2 işi yapmalı 

        /*Interfaceler projede kurallar kitabıdır(Haftalık yaptığım to-do list gibi).
         İçinde kod olmaz,yapılacak işlerin listesi olur*/
        Task<UpdateSettingsDto> GetSettingsAsync();


        // Ayarları güncellemek için,formdan gelen veriyi kaydettim
        Task UpdateSettingsAsync(UpdateSettingsDto updateSettingsDto);
    }
}
