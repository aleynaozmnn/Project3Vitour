using Project3Vitour.Dtos.SettingsDtos;

namespace Project3Vitour.Services.SetingsService
{
    public interface ISettingsService
    {
        Task<UpdateSettingsDto> GetSettingsAsync();

        // Ayarları güncellemek için (Formdan gelen veriyi kaydeder)
        Task UpdateSettingsAsync(UpdateSettingsDto updateSettingsDto);
    }
}
