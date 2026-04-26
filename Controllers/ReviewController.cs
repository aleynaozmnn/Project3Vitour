using Microsoft.AspNetCore.Mvc;
using Project3Vitour.Dtos.ReviewDtos;
using Project3Vitour.Services.ReviewServices;

namespace Project3Vitour.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IActionResult CreateReview(string id)
        {
            var model = new CreateReviewDto { TourId = id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewDto createReviewDto)
        {
            createReviewDto.Status = false;
            await _reviewService.CreateReviewAsync(createReviewDto);
            return RedirectToAction("Index", "Default");
        }

        public async Task<IActionResult> GetReviewByTourId(string id)
        {
            var values = await _reviewService.GetAllReviewsByTourIdAsync(id);
            return View(values);
        }

        // --- BURASI ESKİ HALİNE DÖNDÜ ---
        public async Task<IActionResult> ReviewList()
        {
            var values = await _reviewService.GetAllReviews();
            return View(values);
        }

        public async Task<IActionResult> ApproveReview(string id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review != null)
            {
                var updateDto = new UpdateReviewDto
                {
                    ReviewId = review.ReviewId,
                    NameSurname = review.NameSurname,
                    Detail = review.Detail,
                    Score = review.Score,
                    ReviewDate = review.ReviewDate,
                    TourId = review.TourId,
                    Status = true
                };
                await _reviewService.UpdateReviewAsync(updateDto);
            }
            return RedirectToAction("ReviewList");
        }

        public async Task<IActionResult> DeleteReview(string id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return RedirectToAction("ReviewList");
        }
    }
}