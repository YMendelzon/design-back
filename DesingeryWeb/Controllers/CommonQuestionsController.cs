using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.Extensions.Logging;
using DesigneryCommon.Models;


namespace DesingeryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonQuestionsController : ControllerBase
    {
        private readonly ICommonQuestionsService _commonQuestions;
        private readonly ILogger<CommonQuestionsController> _logger;

        public CommonQuestionsController(ILogger<CommonQuestionsController> logger,
            ICommonQuestionsService i){
            _commonQuestions = i;
            _logger = logger;


        }
        /// <summary>
        ///A function that extracts the FAQ table from the DB
        /// </summary>
        /// <param name="langId">קוד שפה של הטקסט</param>
        /// <returns>מחזיר את כל טבלת FAQ בשפה המבוקשת</returns>
        [HttpGet("GetAllFAQ/langId")]
        public async Task<ActionResult<List<CommonQuestions>>> GetAllFQA(int langId)
        {
                return  _commonQuestions.GetAllQuestions(langId);  
        }

        [HttpPost("PostFAQ")]
        public async Task<ActionResult<bool>> PostFQA(CommonQuestions cc, string a, string b)
        {

            return true;// _commonQuestions.GetAllQuestions(langId);
        }
        
        [HttpPut("PutFAQ/cqId")]
        public async Task<ActionResult<bool>> PutFQA(int cqId, int reting)
        {
            return _commonQuestions.ChangeRating(cqId, reting);
        }
    }
 
}
