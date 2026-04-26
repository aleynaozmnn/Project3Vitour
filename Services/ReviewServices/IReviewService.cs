using Project3Vitour.Dtos.ReviewDtos;

namespace Project3Vitour.Services.ReviewServices
{
    public interface IReviewService
    {
        Task<List<ResultReviewDto>> GetAllReviews();
        Task CreateReviewAsync(CreateReviewDto createReviewDto);
        Task UpdateReviewAsync(UpdateReviewDto updateReviewDto);
        Task DeleteReviewAsync(string id);
        Task<GetReviewByIdDto> GetReviewByIdAsync(string id);
        Task<List<ResultReviewByTourIdDto>> GetAllReviewsByTourIdAsync(string id);
        Task ApproveReviewAsync(string id);
    }
}
