using Project3Vitour.Dtos.TourDto;
namespace Project3Vitour.Models
{
    public class TourPaginationViewModel
    {
        public List<ResultTourDto> Tours { get; set; } = new List<ResultTourDto>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
