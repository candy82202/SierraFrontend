using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIERRA_Server.Models.Interfaces;

namespace SIERRA_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAiController : ControllerBase
    {
        private readonly ILogger<OpenAiController> _logger;
        private readonly IOpenAiService _openAiService;
        public OpenAiController(ILogger<OpenAiController> logger, IOpenAiService openAiService)
        {
            _logger = logger;
            _openAiService = openAiService;
        }

        [HttpPost()]
        [Route("CompleteSentence")]
        public async Task<IActionResult> CompleteSentece(string text)
        {
            var result = await _openAiService.CompleteSentenceAdvance(text);
            return Ok(result);

        }
        [HttpPost()]
        [Route("AskQuestion")]
        public async Task<IActionResult> AskQuestion(string text)
        {
            var result = await _openAiService.CheckProgramingLanguage(text);
            return Ok(result);

        }
        [HttpPost()]
        [Route("AskDessert")]
        public async Task<IActionResult> AskDessert(string text)
        {
            var result = await _openAiService.AskDessertQuestion(text);
            return Ok(result);

        }
    }
}
