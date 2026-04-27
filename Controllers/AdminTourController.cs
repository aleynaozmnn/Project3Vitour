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
        private readonly ITourService _tourService;
        private readonly IImageService _imageService;
        private readonly IReservationService _reservationService;
        private readonly ICategoryService _categoryService;

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
            var values = await _tourService.GetAllTourAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.CategoryList = (from x in categories
                                    select new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                                    {
                                        Text = x.CategoryName,
                                        Value = x.CategoryId.ToString()
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
            // 1. Silinecek resmi bul ki hangi tura ait olduğunu bilelim (Geri dönmek için)
            // Eğer ImageService içinde GetById gibi bir metodun yoksa Referer kullanmaya devam edebiliriz 
            // ama en garantisi budur:
            await _imageService.DeleteImageAsync(id);

            TempData["SuccessMessage"] = "Resim başarıyla kaldırıldı.";

            // Request.Headers["Referer"] bazen güvenilmezdir. 
            // Eğer resim silindikten sonra sayfa yenilenmiyorsa manuel yönlendirme en iyisidir.
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> DeleteTour(string id)
        {
            await _tourService.DeleteTourAsync(id);
            return RedirectToAction("TourList");
        }
        public async Task<IActionResult> ChangeStatus(string id)
        {
            await _tourService.ChangeStatusAsync(id);
            TempData["SuccessMessage"] = "Tur durumu güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTour(string id)
        {
            // 1. Mevcut tur verisini çekiyoruz
            var value = await _tourService.GetTourByIdAsync(id);

            if (value == null)
            {
                return RedirectToAction("TourList");
            }

            // 2. KATEGORİLERİ ÇEKİP VIEW'A GÖNDERİYORUZ (Dropdown için kritik adım)
            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.CategoryList = categories.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString()
            }).ToList();

            // 3. Veriyi DTO'ya mapliyoruz
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
                CategoryId = value.CategoryId // Mevcut kategorinin seçili gelmesi için
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTour(UpdateTourDto updateTourDto)
        {
            if (!ModelState.IsValid)
            {
                // Eğer validation kullanıyorsan kategorileri tekrar doldurmalısın
                var categories = await _categoryService.GetAllCategoryAsync();
                ViewBag.CategoryList = categories.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();
                return View(updateTourDto);
            }

            await _tourService.UpdateTourAsync(updateTourDto);

            // SweetAlert'in yakalaması için mesaj gönderiyoruz
            TempData["SuccessMessage"] = "Güncelleme işlemi başarıyla tamamlandı.";

            return RedirectToAction("Index"); // TourList yerine Index'e yönlendirmek daha standarttır
        }

        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllTourAsync();
            var categories = await _categoryService.GetAllCategoryAsync();

            foreach (var tour in tours)
            {
                // Rezervasyon sayılarını doldur
                tour.CurrentReservationCount = await _reservationService.GetTotalPersonCountByTourIdAsync(tour.TourId);

                // Kategori ID'sinden Kategori Adını bulup Description veya yeni bir alana geçici atayabiliriz
                var cat = categories.FirstOrDefault(x => x.CategoryId == tour.CategoryId);
                // Not: Eğer ResultTourDto içinde CategoryName alanı açarsan çok daha şık olur.
            }

            ViewBag.TotalTours = tours.Count;
            ViewBag.TotalTravelers = tours.Sum(x => x.CurrentReservationCount);
            ViewBag.TotalEarning = tours.Sum(x => (decimal)x.CurrentReservationCount * x.Price);

            double totalCapacity = tours.Sum(x => (double)x.Capacity);
            double totalReserved = tours.Sum(x => (double)x.CurrentReservationCount);

            ViewBag.AvgFullness = totalCapacity > 0
                ? ((totalReserved / totalCapacity) * 100).ToString("F1")
                : "0.0";

            return View(tours);
        }

        [HttpGet]
        public async Task<IActionResult> AddImage(string id)
        {
            // Mevcut resimleri çek
            var images = await _imageService.GetImagesByTourIdAsync(id);

            // Resimleri ViewBag ile sayfaya gönder
            ViewBag.CurrentImages = images;

            var model = new CreateTourImageDto { TourId = id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(CreateTourImageDto createTourImageDto)
        {
            var currentImages = await _imageService.GetImagesByTourIdAsync(createTourImageDto.TourId);

            // Buradaki sınırı 6 yapmayı unutma, kodunda 100 kalmış
            if (currentImages != null && currentImages.Count >= 6)
            {
                TempData["ErrorMessage"] = "Maksimum fotoğraf limitine (6) ulaşıldı.";
                return RedirectToAction("AddImage", new { id = createTourImageDto.TourId });
            }

            await _imageService.CreateImageAsync(createTourImageDto);
            TempData["SuccessMessage"] = "Görsel başarıyla galeriye eklendi.";

            // BURASI KRİTİK: Seni kullanıcı sayfasına değil, admin resim ekleme sayfasına geri atmalı
            return RedirectToAction("AddImage", new { id = createTourImageDto.TourId });
        }
        public async Task<IActionResult> DeleteReservation(string id)
        {
            await _reservationService.DeleteReservationAsync(id);
            return RedirectToAction("ReservationList");
        }

        public async Task<IActionResult> ExportToExcel(string id)
        {
            // 1. Rezervasyonu çek
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();

            // 2. KRİTİK NOKTA: Tur detayını çekerek Tur Adını alıyoruz
            var tour = await _tourService.GetTourByIdAsync(reservation.TourId);
            string tourName = tour != null ? tour.Title : "Bilinmeyen Tur";

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Rezervasyon Detay");

                
                worksheet.Cell(1, 1).Value = "VİTOUR REZERVASYON RAPORU";
                worksheet.Range("A1:B1").Merge().Style.Font.Bold = true;

                worksheet.Cell(3, 1).Value = "SEÇİLEN TUR:"; // İşte patronun istediği o bilgi!
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

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Rapor_{reservation.NameSurname.Replace(" ", "_")}.xlsx");
                }
            }
        }

        public async Task<IActionResult> ReservationDetail(string id)
        {
            var value = await _reservationService.GetReservationByIdAsync(id);
            return View(value);
        }
    }
}