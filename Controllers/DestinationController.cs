using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.DestinationDtos;
using Project3Vitour.Services.DestinationService;

namespace Project3Vitour.Controllers
{
    public class DestinationController : Controller
    {
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }
      
        public async Task<IActionResult> Index()
        {
            var values = await _destinationService.GetAllDestinationAsync();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateDestination()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>CreateDestination(CreateDestinationDto createDestinationDto)
        {
            if (!ModelState.IsValid) // Eğer yukarıdaki kurallara uyulmadıysa...
            {
                return View(createDestinationDto); // Sayfayı hatalarla birlikte geri yükle
            }
            await _destinationService.CreateDestinationAsync(createDestinationDto);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult>DeleteDestination(string id)
        {
            await _destinationService.DeleteDestinationAsync(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult>UpdateDestination(string id)
        {
            var value = await _destinationService.GetDestinationByIdAsync(id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult>UpdateDestination(UpdateDestinationDto updateDestinationDto)
        {
            await _destinationService.UpdateDestinationAsync(updateDestinationDto);
            return RedirectToAction("Index");
        }
    }
}
