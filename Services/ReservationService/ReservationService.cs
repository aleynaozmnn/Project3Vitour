using MongoDB.Driver;
using Project3Vitour.Dtos.ReservationDto;
using Project3Vitour.Entities;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.ReservationService
{
    public class ReservationService:IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        public ReservationService(IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _reservationCollection = database.GetCollection<Reservation>(_databaseSettings.ReservationCollectionName);

        }
        public async Task CreateReservationAsync(CreateReservationDto createReservationDto)
        {
            var reservation = new Reservation
            {
                TourId = createReservationDto.TourId,
                NameSurname = createReservationDto.NameSurname,
                Email = createReservationDto.Email,
                Phone = createReservationDto.Phone,
                PersonCount = createReservationDto.PersonCount,
                Description = createReservationDto.Description,
                ReservationDate = DateTime.Now
            };
            await _reservationCollection.InsertOneAsync(reservation);
        }
        public async Task<int> GetTotalPersonCountByTourIdAsync(string tourId)
        {
            // O tura ait tüm rezervasyonları bul ve kişi sayılarını topla
            var reservations = await _reservationCollection.Find(x => x.TourId == tourId).ToListAsync();
            return reservations.Sum(x => x.PersonCount);
        }
        public async Task<List<ResultReservationDto>> GetAllReservationsAsync()
        {
            var values = await _reservationCollection.Find(x => true).ToListAsync();

            // Manuel eşleme (Eğer AutoMapper ile ilgili bir hata alırsan en güvenli yol budur)
            return values.Select(x => new ResultReservationDto
            {
                ReservationId = x.ReservationId,
                NameSurname = x.NameSurname,
                Email = x.Email,
                Phone = x.Phone,
                PersonCount = x.PersonCount,
                ReservationDate = x.ReservationDate,
                Description = x.Description,
                TourId = x.TourId
            }).ToList();
        }
        // Rezervasyon Silme
        // Rezervasyon Silme - DOĞRU HALİ
        public async Task DeleteReservationAsync(string id)
        {
            // _database yerine yukarıda tanımladığın _reservationCollection'ı kullanmalısın
            await _reservationCollection.DeleteOneAsync(x => x.ReservationId == id);
        }

        // ID'ye Göre Getirme - DOĞRU HALİ
        public async Task<ResultReservationDto> GetReservationByIdAsync(string id)
        {
            // Burada da _reservationCollection kullanıyoruz
            var value = await _reservationCollection.Find(x => x.ReservationId == id).FirstOrDefaultAsync();

            if (value == null) return null;

            return new ResultReservationDto
            {
                ReservationId = value.ReservationId,
                NameSurname = value.NameSurname,
                Email = value.Email,
                Phone = value.Phone,
                PersonCount = value.PersonCount,
                ReservationDate = value.ReservationDate,
                TourId = value.TourId
            };
        }
    } 
}
