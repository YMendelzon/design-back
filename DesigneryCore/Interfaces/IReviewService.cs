using DesigneryCommon.Models;

namespace DesigneryCore.Interfaces
{
    public interface IReviewService
    {
        List<Review> GetAllReviews();
        Review GetReviewsByProdId(int id);
        bool PostReview(int productID, int userId, int rating, string comment);

    }
}