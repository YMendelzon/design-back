using DesigneryCommon.Models;
using DesigneryCore.Interfaces;
using DesigneryDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        //for what the logger here??
        private readonly ILogger<ReviewController> _logger;
        private readonly IReviewService _reviewService;
        public ReviewController(ILogger<ReviewController> logger, IReviewService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }



        [HttpGet("GetReviews")]
        public async Task<ActionResult<List<Review>>> GetReiews()
        { 
            return _reviewService.GetAllReviews();
        }

        [HttpGet("GetReviewByProd/{prodId}")]
        public async Task<ActionResult<List<Review>>> GetReviewByProd(int prodId)
        {
            return _reviewService.GetReviewsByProdId(prodId);
        }

        [HttpPost("AddReview")]
        public async Task<ActionResult<bool>> PostReview(Review review)
        {
            return _reviewService.PostReview(review);
        }
    }
}
