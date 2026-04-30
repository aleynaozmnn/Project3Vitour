using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.DestinationDtos;
using Project3Vitour.Services.DestinationService;
using DocumentFormat.OpenXml.Spreadsheet;
using Project3Vitour.Services.TourServices;

namespace Project3Vitour.Controllers
{
    public class DestinationController : Controller
    {
        private readonly IDestinationService _destinationService;
        private readonly ITourService _tourService;

        public DestinationController(IDestinationService destinationService,
            ITourService tourService)
        {
            _destinationService = destinationService;
            _tourService = tourService;
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
        public async Task<IActionResult>ExportToExcel()
        {
            //Servicekatmanına gidip mongodbmdeki tüm destinasyonlarını liste oalrak çektim
            var values=await _destinationService.GetAllDestinationAsync();

            //Bilgisayarın hafızasında boş bir Excel dosyası oluşturdum.
            using (var workbook=new XLWorkbook())
            {
                //Dosyanın içine boş bir sayfa(sheet)ekledim
                var worksheet = workbook.Worksheets.Add("Destinasyon Listesi");
                //Cell(satır,sütun)->Cell(x,y) mantıgında yerleştirdim bilgileri.
                worksheet.Cell(1, 1).Value = "Şehir";;
                worksheet.Cell(1, 2).Value = "Ülke";
                worksheet.Cell(1, 3).Value = "Fiyat";
                worksheet.Cell(1, 4).Value = "Kapasite";
                worksheet.Cell(1, 5).Value = "Süre";
                for(int i=0;i<values.Count;i++)
                {
                    worksheet.Cell(i+2, 1).Value = values[i].City;
                    worksheet.Cell(i+2, 2).Value = values[i].Country;
                    worksheet.Cell(i+2, 3).Value = values[i].Price;
                    worksheet.Cell(i+2, 4).Value = values[i].Capacity;
                    worksheet.Cell(i+2, 5).Value = values[i].DayNight;
                }
                //Hazırladığım exceli kullanıcı indirecekse bunu dosya hale getirmeye çalıştım
                //MemoryStream->Hayali dosya gibidir.Oluşturdugum excel dosyasını Ramdeki geçici alana kaydettim.
                using (var stram=new MemoryStream())
                {
                    workbook.SaveAs(stram);
                    //Ramdeki byte yığınını aldım->ToArray ile
                    var content=stram.ToArray();
                    //Dile Methodu sayesinde kullanıcının klasörüne indir emri verdim.
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Destinasyon_Rapor.xlsx");
                }

            }
        }
        public async Task<IActionResult>ImportFromTours()
        {
            var tours = await _tourService.GetAllTourAsync();
            foreach(var item in tours)
            {
                //Eğer bu isimde bir destinasyon zaten varsa pas geçiyorum
                var allDestinations = await _destinationService.GetAllDestinationAsync();
                if (allDestinations.Any(x => x.City == item.Title)) continue;
                var newDestination = new CreateDestinationDto
                {
                    City = item.Title,
                    Country = item.Badge,
                    Price = item.Price,
                    Capacity = item.Capacity,
                    DayNight = item.DayCount.ToString() + "Gün",
                    ImageUrl = item.CoverImageUrl
                };
                await _destinationService.CreateDestinationAsync(newDestination);
            }
            return RedirectToAction("Index");
        }
    }
}
