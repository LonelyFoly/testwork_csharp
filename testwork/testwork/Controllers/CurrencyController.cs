using Microsoft.AspNetCore.Mvc;
using testwork.Handlers;

namespace testwork.Controllers
{
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _service;

        public CurrencyController(CurrencyService service)
        {
            _service = service;
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> GetCurrencies([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedCurrenciesAsync(page, pageSize);
            if (result == null)
                return NotFound();
            return Ok(new { page, pageSize, result });
        }

        [HttpGet("currency/{id}")]
        public async Task<IActionResult> GetCurrencyById(string id)
        {
            var currency = await _service.GetCurrencyByIdAsync(id);
            if (currency == null)
                return NotFound();

            return Ok(currency);
        }
    }
}
