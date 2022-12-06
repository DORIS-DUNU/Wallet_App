using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Abstractions.Repositories;
using WalletApp.Abstractions.Services;
using WalletApp.Models.DTO;

namespace WalletApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        [HttpGet("/convert")]
        public async Task<IActionResult> ConvertCurrency(string currencyToConvert, double amount)
        {
            var result = await _transactionService.GetRateAsync(currencyToConvert, amount);
            if (result != null) return Ok(result);

            _logger.LogInformation("Unable to Convert");
            return BadRequest();

        }
    }

}