using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IReviewService
    {
        List<Review> GetAllReviews();
        List<Review> GetReviewsByProdId(int id);
        bool PostReview(Review review);

    }
}