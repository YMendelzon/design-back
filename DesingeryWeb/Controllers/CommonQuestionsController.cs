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
        //Functions get, put , post of Common Questions
        [HttpGet("GetAllFAQ")]
        public async Task<ActionResult<List<CommonQuestions>>> GetAllFQA()
        {

                return  _commonQuestions.GetAllQuestions();  
        }

        [HttpPost("PostFAQ")]
        public async Task<ActionResult<bool>> PostFQA(CommonQuestions c)
        {

            return  _commonQuestions.PostCommonQuestions(c);
        }

        [HttpPut("PutFAQ/{cqId}")]
        public async Task<ActionResult<bool>> PutFQA(int cqId, CommonQuestions c)
        {
            return _commonQuestions.PutCommonQuestions(cqId, c);
        }
    }
 
}
