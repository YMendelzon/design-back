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


        //get all reviews function - async
        [HttpGet("GetReviews")]
        public async Task<ActionResult<List<Review>>> GetReiews()
        {
            try
            {
                // called the function from the review's interface 
                return _reviewService.GetAllReviews();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //get by prodID
        [HttpGet("GetReviewByProd/{prodId}")]
        public async Task<ActionResult<Review>> GetReviewByProd(int prodId)
        {
            try
            {
                return _reviewService.GetReviewsByProdId(prodId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //add review
        [HttpPost]
        public async Task<ActionResult<bool>> PostReview(int prodId, int userId, int rating, string comment)
        {
            try
            {
                return _reviewService.PostReview(prodId, userId, rating, comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
