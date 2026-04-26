using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Services.TourServices;

namespace Project3Vitour.ViewComponents.TourViewComponents
{
    public class _AllTourListComponentPartial:ViewComponent
    {
        private readonly ITourService _tourService;

        public _AllTourListComponentPartial(ITourService tourService)
        {
            _tourService = tourService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _tourService.GetAllTourAsync();
            return View(values);
        }
    }
}
