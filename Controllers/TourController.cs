using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.ReservationDto;
using Project3Vitour.Dtos.TourDto;
using Project3Vitour.Models;
using Project3Vitour.Services.CategoryServices;
using Project3Vitour.Services.ImageService;
using Project3Vitour.Services.ReservationService;
using Project3Vitour.Services.ReviewServices;
using Project3Vitour.Services.TourPlanService;
using Project3Vitour.Services.TourPlanServices;
using Project3Vitour.Services.TourServices;
using System.Threading.Tasks;
using Project3Vitour.Services.MailServices;


namespace Project3Vitour.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ITourPlanService _tourPlanService;
        private readonly IReviewService _reviewService;
        private readonly IReservationService _reservationService;
        private readonly IImageService _imageService;
        private readonly ICategoryService _categoryService;
        private readonly IMailService _mailService;
        public TourController(ITourService tourService, ITourPlanService tourPlanService, IReviewService reviewService,IReservationService reservationService,
             IImageService imageService
             , ICategoryService categoryService,
             IMailService mailService)
        {
            _tourService = tourService;
            _tourPlanService = tourPlanService;
            _reviewService = reviewService;
            _reservationService = reservationService;
            _imageService = imageService;
            _categoryService = categoryService;
            _mailService = mailService;
        }

     
        public async Task<IActionResult> TourList(int page = 1)
        {
            // Her sayfada 6 tur göster
            int pageSize = 6;
            var values = await _tourService.GetToursWithPagingAsync(page, pageSize);

            // REVİZE: Her turun güncel rezervasyon sayısını modele ekliyoruz
            foreach (var item in values)
            {
                item.CurrentReservationCount = await _reservationService.GetTotalPersonCountByTourIdAsync(item.TourId);
                if(!string.IsNullOrEmpty(item.CategoryId))
                {
                    //o id ye sahip kategorinin adını getirmek için
                    var category=await _categoryService.GetCategoryByIdAsync(item.CategoryId);
                    //Kategori bulunduysa item.categoryName içine koy
                    item.CategoryName = category != null ? category.CategoryName : "Kategorisiz";
                }
            }

            var totalTourCount = await _tourService.GetTotalTourCountAsync();

            var model = new TourPaginationViewModel
            {
                Tours = values,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalTourCount,
                TotalPages = (int)Math.Ceiling((double)totalTourCount / pageSize)
            };

            return View(model);
        }

        public async Task<IActionResult> TourSingle(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);
            if (value == null)
            {
                return RedirectToAction("TourList");
            }

           
            value.CurrentReservationCount = await _reservationService.GetTotalPersonCountByTourIdAsync(id);
            ViewBag.Plans = await _tourPlanService.GetTourPlanByTourIdAsync(id);
            ViewBag.Reviews = await _reviewService.GetAllReviewsByTourIdAsync(id);
            ViewBag.TourGallery = await _imageService.GetImagesByTourIdAsync(id);
    
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(Project3Vitour.Dtos.ReviewDtos.CreateReviewDto createReviewDto)
        {
             
            createReviewDto.ReviewDate = DateTime.Now;
            createReviewDto.Status = true;
            await _reviewService.CreateReviewAsync(createReviewDto);
            return RedirectToAction("TourSingle", new { id = createReviewDto.TourId });
        }
        [HttpGet]
        public async Task< IActionResult> Reservation(string id)
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour == null)
            {
                return RedirectToAction("TourList");
            }

            // Dinamik Veriler: Veritabanından o anki durumu çekiyoruz
            var currentBookings = await _reservationService.GetTotalPersonCountByTourIdAsync(id);

            ViewBag.TourId = id;
            ViewBag.TourName = tour.Title;         
            ViewBag.Price = tour.Price;              
            ViewBag.Capacity = tour.Capacity;       
            ViewBag.CurrentBookings = currentBookings; 

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MakeReservation(CreateReservationDto createReservationDto)
        {
            var tour = await _tourService.GetTourByIdAsync(createReservationDto.TourId);
            if (tour == null)
            {
                return Json(new { success = false, message = "Tur bulunamadı." });
            }

            // Kapasite Kontrolü (Business Logic)
            var currentBookingsCount = await _reservationService.GetTotalPersonCountByTourIdAsync(createReservationDto.TourId);

            if (currentBookingsCount + createReservationDto.PersonCount > tour.Capacity)
            {
                return Json(new { success = false, message = "Üzgünüz, seçilen kişi sayısı için yeterli kontenjan kalmamıştır!" });
            }

            // Her şey tamamsa kaydet
            await _reservationService.CreateReservationAsync(createReservationDto);
           
            try
            {
                string subject = "Rezervasyon Onayı - Vitour Seyahat";

                string participantNote = "";
                // Eğer 1'den fazla kişiyse VE açıklama girildiyse nota ekle
                if (createReservationDto.PersonCount > 1 && !string.IsNullOrEmpty(createReservationDto.Description))
                {
                    participantNote = $"\nEk Katılımcı Bilgileri: {createReservationDto.Description}";
                }

                string body = $"Merhaba {createReservationDto.NameSurname},\n\n" +
                              $"{tour.Title} turu için rezervasyonunuz başarıyla alınmıştır.\n" +
                              $"Kişi Sayısı: {createReservationDto.PersonCount}" +
                              $"{participantNote}\n\n" +
                              "Keyifli yolculuklar dileriz!";
                _mailService.SendMail(createReservationDto.Email, subject, body);

            }
            catch
            {

            }
            return Json(new { success = true, message = "Rezervasyonunuz başarıyla alındı ve onay maili gönderildi!" });
        }
        
        
    }
}
