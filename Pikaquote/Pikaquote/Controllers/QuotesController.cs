using System.Net;
using Microsoft.AspNetCore.Mvc;
using Pikaquote.Core.Abstractions;
using Pikaquote.Core.Models;

namespace Pikaquote.Controllers
{
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IQuotesService _quotesService;

        public QuotesController(IQuotesService quotesService)
        {
            _quotesService = quotesService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(QuoteDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 503)]
        [Route("api/[controller]")]
        public async Task<ActionResult> GetQuote([FromQuery] string dictionary = "")
        {
            var quote = await _quotesService.GetQuoteAsync(dictionary);

            if (quote == null)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }

            return Ok(quote);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<QuotesDictionaryDto>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 503)]
        [Route("api/Dictionaries")]
        public async Task<ActionResult> GetDictionaries()
        {
            var dictionaries = await _quotesService.GetDictionariesAsync();

            if (!dictionaries.Any())
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }

            return Ok(dictionaries);
        }
    }
}
