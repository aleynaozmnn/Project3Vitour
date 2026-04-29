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

       
        public async Task<IActionResult> Index()
        {
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }
        //Boş bir kategori eklem formu açar.

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            //Yeni bir kategori eklediğimde,otomatik Aktif yapıyorum
            createCategoryDto.CategoryStatus = true;
            await _categoryService.CreateCategoryAsync(createCategoryDto);
            TempData["CategorySuccess"] = "Yeni kategori başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(string id)
        {
            //O id ye sahip kategoriyi db den kalıcı oalrak siler.
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
             
            var value = await _categoryService.GetCategoryByIdAsync(id);

             
            if (value == null)
            {
                return RedirectToAction("Index");
            }

             
            var updateDto = new UpdateCategoryDto
            {
                CategoryId = value.CategoryId, // Buradaki isimlerin DTO ile birebir aynı olduğundan emin ol
                CategoryName = value.CategoryName,
                CategoryStatus = value.CategoryStatus
            };

            return View(updateDto);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            // Şunu ekle:
            TempData["CategorySuccess"] = "Kategori güncellendi.";
            return RedirectToAction("Index");
        }
    }
}