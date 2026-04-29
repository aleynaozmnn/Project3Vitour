using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.TourPlanDto;
using Project3Vitour.Services.TourPlanService;
using Project3Vitour.Services.TourPlanServices;
namespace Project3Vitour.Controllers
{
    public class AdminTourPlanController : Controller
    {  //Tur planalrını veritabanına kaydedecek,silecek ve güncelleyecek olan servis katmanını sınıfa tanıttım
        private readonly ITourPlanService _tourPlanService;
        public AdminTourPlanController(ITourPlanService tourPlanService)
        {
            _tourPlanService = tourPlanService;
        }

        public async Task<IActionResult> Index(string id)
        {
            //Girilen id me ait tur planalrını db den çektim
            var values = await _tourPlanService.GetTourPlanByTourIdAsync(id);
            ViewBag.tourId = id;
            return View(values);  
        }

        [HttpPost]
        //Kullanıcı formu doldurup kaydet butonuna bastığında
        public async Task<IActionResult> AddPlan(CreateTourPlanDto dto)
        {
            await _tourPlanService.CreateTourPlanAsync(dto);
            return RedirectToAction("Index", new { id = dto.TourId });
        }
        public async Task<IActionResult> DeletePlan(string id)
        {
            //O planın hangi tura ait olduğunu öğren
            var plan = await _tourPlanService.GetByIdTourPlanAsync(id);
            var tourId = plan.TourId; 
            await _tourPlanService.DeleteTourPlanAsync(id);
            return RedirectToAction("Index", new { id = tourId });
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePlan(string id)
        {
            var value = await _tourPlanService.GetByIdTourPlanAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePlan(UpdateTourPlanDto updateTourPlanDto)
        {
            await _tourPlanService.UpdateTourPlanAsync(updateTourPlanDto);
             
            return RedirectToAction("Index", new { id = updateTourPlanDto.TourId });
        }
    }
}
