using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Project3Vitour.ViewComponents.TourViewComponents
{
    public class _TourHeaderComponentPartial : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
