using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DesigneryCore.Interfaces;
using DesigneryCore.Services;
using Microsoft.Extensions.Logging;


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


        [HttpGet(Name = "GetAllFAQ")]
        public async Task<ActionResult<int>> getAll()
        {
            try
            {
                return 
                    _commonQuestions.getAllQuestions(1);
            }
            catch (Exception ex) { return BadRequest(ex); }
        }
    }
 




}
