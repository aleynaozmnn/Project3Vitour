using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.CategoryDtos;
using Project3Vitour.Services.CategoryServices;

namespace Project3Vitour.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Listeleme sayfan (Burası muhtemelen eksikti, ekliyorum)
        public async Task<IActionResult> Index()
        {
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            createCategoryDto.CategoryStatus = true;
            await _categoryService.CreateCategoryAsync(createCategoryDto);

            // ARTIK INDEX VAR! Oraya uçuyoruz.
            return RedirectToAction("Index");
        }
        // Parametreyi string yaptık ki Service ile eşleşsin
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            // 1. Veriyi çekiyoruz
            var value = await _categoryService.GetCategoryByIdAsync(id);

            // 2. Güvenlik Kontrolü: Eğer veri null geldiyse Index'e geri gönder (Hata almamak için)
            if (value == null)
            {
                return RedirectToAction("Index");
            }

            // 3. Mapping: Sadece veri varsa buraya geçecek
            var updateDto = new UpdateCategoryDto
            {
                CategoryId = value.CategoryId, // Buradaki isimlerin DTO ile birebir aynı olduğundan emin ol
                CategoryName = value.CategoryName,
                CategoryStatus = value.CategoryStatus
            };

            return View(updateDto);
        }

        // 3. GÜNCELLEME İŞLEMİNİ YAPMA (POST)
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return RedirectToAction("Index");
        }
    }
}