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

        [HttpGet("/conversion")]
        public async Task<IActionResult> CurrencyToCurrency(string currencyA, string currencyB, double amount)
        {
            var result = await _transactionService.ConvertCurrencyAsync(currencyA, currencyB, amount);
            if (result != null) return Ok(result);

            _logger.LogInformation("Unable to convert");
            return BadRequest();

        }

        [HttpPost("/transactions")]
        public async Task<IActionResult> GetAllUserTransactions()
        {

            var result = await _transactionService.GetAllUserTransactionsAsync();
            if (result != null)
                return Ok(result);

            _logger.LogInformation("Could not get all user transaction");
            return NotFound();
        }

        [HttpPost("/statement")]
        public async Task<IActionResult> GetWalletStatement(string Address, int page)
        {
            var result = await _transactionService.GetWalletStatementAsync(Address, page);
            if (result != null)
                return Ok(result);

            _logger.LogInformation("Could not get all wallet statement");
            return NotFound();
        }
    }

}