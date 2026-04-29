using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.TourDto;
using Project3Vitour.Entities;
using Project3Vitour.Services.CategoryServices;
using Project3Vitour.Services.ImageService;
using Project3Vitour.Services.ReservationService;
using Project3Vitour.Services.TourServices;
using System.IO;
using System.Threading.Tasks;
using Project3Vitour.Dtos.ImageDtos;

namespace Project3Vitour.Controllers
{
    public class AdminTourController : Controller
    {
        //Çoklu servis enjeksiyonu.Controller burda 4 farklı servisi  birden yanına çağırıyor.
        private readonly ITourService _tourService;
        private readonly IImageService _imageService;
        private readonly IReservationService _reservationService;
        private readonly ICategoryService _categoryService;

        //Constructor'ım.Controller bu servislerin methodlarını çağırır.Bir turu yönetmek için bu 4 bilgiye ihtiyaç vardır
        public AdminTourController(ITourService tourService, IImageService imageService,
            IReservationService reservationService,
            ICategoryService categoryService)
        {
            _tourService = tourService;
            _imageService = imageService;
            _reservationService = reservationService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> TourList()
        {
            //Tur service'e gider,tüm turları getirip view,sayfama basarım.
            var values = await _tourService.GetAllTourAsync();
            var categories = await _categoryService.GetAllCategoryAsync();
            foreach (var tour in values)
            {
                
                var cat = categories.FirstOrDefault(x => x.CategoryId == tour.CategoryId);
                if (cat != null)
                {
                    tour.CategoryName = cat.CategoryName;
                }
            }
            ViewBag.CategoryList = categories.Select(x => x.CategoryName).Distinct().ToList();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            /*Gidip serviceten aldığım ham kategori verisini <select>(açılır lsite combobox gibi)etiketinin anlayacağı
            SelectListItem formatına çeviririm*/

            //Viewbag:Controllerdan View(sayfama) küçük çanta içinde veri göndermemdir.
            ViewBag.CategoryList = (from x in categories
                                    select new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                                    {
                                        Text = x.CategoryName,//Kullanıcının göreceği isim
                                        Value = x.CategoryId.ToString()//Arkada db ye kayıt edilecek Id'm.
                                    }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto createTourDto)
        {
            await _tourService.CreateTourAsync(createTourDto);
            TempData["SuccessMessage"] = "Tur başarıyla sisteme eklendi!";
            return RedirectToAction("TourList");
        }
        public async Task<IActionResult> DeleteImage(string id)
        {
            //Gelen id ye sahip fotoyu siler.
            await _imageService.DeleteImageAsync(id);

            TempData["SuccessMessage"] = "Resim başarıyla kaldırıldı.";

            //Kullanıcı hangi sayfadan sil butonuna bastıysa,işlem bitince otomatik o sayfaya yolla.
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> DeleteTour(string id)
        {
            //Url den gelen id bilgisini service göndericem.
            await _tourService.DeleteTourAsync(id);
            //İşlem bitince kullanıcıyı listeye yollarım.
            return RedirectToAction("TourList");
        }
        public async Task<IActionResult> ChangeStatus(string id)
        {
            //Turu silmek yerine aktif,pasif yapar
            await _tourService.ChangeStatusAsync(id);
            // SweetAlert'in yakalaması için mesaj gönderiyoruz
            TempData["SuccessMessage"] = "Tur durumu güncellendi.";
            return RedirectToAction("TourList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTour(string id)
        {
            // Güncellenecek turun tüm mevcut bilgilerini db den çekerim
            var value = await _tourService.GetTourByIdAsync(id);

            if (value == null)
            {
                return RedirectToAction("TourList");
            }

           
            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.CategoryList = categories.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString()
            }).ToList();

            //Db den gelen entity verilerini ,formda kullanacağım UpdateTourDto nesnesine tek tek yerleştirdidm,Form açıldığında kutular dolu geelcek artık.
            var model = new UpdateTourDto
            {
                TourId = value.TourId,
                Title = value.Title,
                Description = value.Description,
                Price = value.Price,
                Capacity = value.Capacity,
                DayCount = value.DayCount,
                CoverImageUrl = value.CoverImageUrl,
                Badge = value.Badge,
                IsStatus = value.IsStatus,
                MapLocationImageUrl = value.MapLocationImageUrl,
                CategoryId = value.CategoryId  
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTour(UpdateTourDto updateTourDto)
        {
            if (!ModelState.IsValid)
            {
                 
                var categories = await _categoryService.GetAllCategoryAsync();
                ViewBag.CategoryList = categories.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();
                return View(updateTourDto);
            }

            await _tourService.UpdateTourAsync(updateTourDto);
            TempData["SuccessMessage"] = "Güncelleme işlemi başarıyla tamamlandı.";
            return RedirectToAction("Index");  
        }

        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllTourAsync();
            var categories = await _categoryService.GetAllCategoryAsync();

            foreach (var tour in tours)
            {
                //Tüm turları almıştık,bu tura kaç kişi rezervasyon yaptı diye bakıyoruz
                tour.CurrentReservationCount = await _reservationService.GetTotalPersonCountByTourIdAsync(tour.TourId);
                var cat = categories.FirstOrDefault(x => x.CategoryId == tour.CategoryId);
                if (cat != null)
                {
                    tour.CategoryName = cat.CategoryName;  
                }
            }

            ViewBag.TotalTours = tours.Count;
            ViewBag.TotalTravelers = tours.Sum(x => x.CurrentReservationCount);

            //Her turun kişi*fiyat ile akzancını hesaplarım
            ViewBag.TotalEarning = tours.Sum(x => (decimal)x.CurrentReservationCount * x.Price);

            double totalCapacity = tours.Sum(x => (double)x.Capacity);
            double totalReserved = tours.Sum(x => (double)x.CurrentReservationCount);
            //Total kapasite/toplam rezervasyon oranını bulup turun doluluk oranını hesaplarım
            ViewBag.AvgFullness = totalCapacity > 0
                ? ((totalReserved / totalCapacity) * 100).ToString("F1")
                : "0.0";

            return View(tours);
        }

        [HttpGet]
        public async Task<IActionResult> AddImage(string id)
        {
            //Id ye göre mevcut resimleri çek
            var images = await _imageService.GetImagesByTourIdAsync(id);

            // Resimleri ViewBag(lüçük çantam) ile sayfaya gönder
            ViewBag.CurrentImages = images;
            var model = new CreateTourImageDto { TourId = id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(CreateTourImageDto createTourImageDto)
        {
            var currentImages = await _imageService.GetImagesByTourIdAsync(createTourImageDto.TourId);

             
            if (currentImages != null && currentImages.Count >= 6)
            {
                TempData["ErrorMessage"] = "Maksimum fotoğraf limitine (6) ulaşıldı.";
                return RedirectToAction("AddImage", new { id = createTourImageDto.TourId });
            }

            await _imageService.CreateImageAsync(createTourImageDto);
            TempData["SuccessMessage"] = "Görsel başarıyla galeriye eklendi.";

            
            return RedirectToAction("AddImage", new { id = createTourImageDto.TourId });
        }
        public async Task<IActionResult> DeleteReservation(string id)
        {
            await _reservationService.DeleteReservationAsync(id);

            
            TempData["SuccessMessage"] = "Rezervasyon başarıyla silindi.";

            return RedirectToAction("ReservationList");
        }

        public async Task<IActionResult> ExportToExcel(string id)
        {
             
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();

 
            var tour = await _tourService.GetTourByIdAsync(reservation.TourId);
            string tourName = tour != null ? tour.Title : "Bilinmeyen Tur";

            //Boş bir excel dosyası oluşturur
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Rezervasyon Detay");

                //Excelin 1.satır 1.sütununa bu başlığı yazar
                worksheet.Cell(1, 1).Value = "VİTOUR REZERVASYON RAPORU";
                worksheet.Range("A1:B1").Merge().Style.Font.Bold = true;

                worksheet.Cell(3, 1).Value = "SEÇİLEN TUR:"; 
                worksheet.Cell(3, 2).Value = tourName;
                worksheet.Cell(3, 2).Style.Font.Bold = true;
                worksheet.Cell(3, 2).Style.Font.FontColor = XLColor.DarkGreen;

                worksheet.Cell(4, 1).Value = "GEZGİN ADI:";
                worksheet.Cell(4, 2).Value = reservation.NameSurname;

                worksheet.Cell(5, 1).Value = "İLETİŞİM:";
                worksheet.Cell(5, 2).Value = reservation.Email;

                worksheet.Cell(6, 1).Value = "KİŞİ SAYISI:";
                worksheet.Cell(6, 2).Value = reservation.PersonCount;

                worksheet.Cell(7, 1).Value = "REZERVASYON TARİHİ:";
                worksheet.Cell(7, 2).Value = reservation.ReservationDate.ToString("dd.MM.yyyy");

                //Çalışma sayfasındaki tüm sütunalı içindeki mentin uzunluğuna göre otomatik genişlett
                worksheet.Columns().AdjustToContents();

                //Hazırlanan bu sanal dosyayı bir veri akışına çevirir.
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    //Bu veriyi, tarayıcıya xlsx formatında bir dosya olarak gönderir.
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Rapor_{reservation.NameSurname.Replace(" ", "_")}.xlsx");
                }
            }
        }
        public async Task<IActionResult> ReservationList()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            var tours = await _tourService.GetAllTourAsync();
            foreach (var res in reservations)
            {
                var tour = tours.FirstOrDefault(x => x.TourId == res.TourId);
                // ReservationDto içindeki Description alanına tur adını atıyoruz
                res.Description = tour != null ? tour.Title : "Tur Bulunamadı";
            }

            
            return View(reservations);
        }

        public async Task<IActionResult> ReservationDetail(string id)
        {
            
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();
            var tour = await _tourService.GetTourByIdAsync(reservation.TourId);
            reservation.Description = tour != null ? tour.Title : "Tur Bilgisi Bulunamadı";
            return View(reservation);
        }
        
        private async Task FillCategoryViewBag()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.CategoryList = categories.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString()
            }).ToList();
        }
    }
}