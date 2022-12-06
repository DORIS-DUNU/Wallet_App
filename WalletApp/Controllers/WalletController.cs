using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WalletApp.Abstractions.Services;
using WalletApp.Models.DTO;

namespace WalletApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IWalletService _walletService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public WalletController(ILogger<WalletController> logger, IWalletService walletService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _walletService = walletService;
            this.httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<ActionResult<string>> CreateAddress()
        {
            return Ok(await _walletService.CreateWalletAsync());
        }

        [HttpPost("/deposit")]
        public async Task<ActionResult<string>> Deposit(DepositDto deposit)
        {
            var depsited = await _walletService.DepositAsync(deposit);
            if (false == depsited) return BadRequest("Unable to Deposit in this wallet");
            return Ok("Deposit successfully");
        }

        [HttpPost("/withdraw")]
        public async Task<ActionResult<string>> Withdraw(DepositDto withdraw)
        {
            var withdrawn = await _walletService.WithdrawAsync(withdraw);
            if (false == withdrawn) return BadRequest("Unable to Withdraw from this wallet");
            return Ok("Deposit successfully");
        }

        [HttpPost("/transfer")]
        public async Task<ActionResult<string>> Transfer(TransferDto transfer)
        {
            var transferred = await _walletService.TransferAsync(transfer);
            if (false == transferred) return BadRequest("Unable to transfer");
            return Ok("Withdrawal Successful");
        }

        [HttpGet("/balance")]
        public async Task<ActionResult<double>> GetBalance(string walletAddress, string currencyCode)
        {
            var result = await _walletService.GetBalanceAsync(walletAddress, currencyCode);
            if (result != null) return Ok(result);
            _logger.LogInformation("Unable to get balance");
            return BadRequest();
        }


        [HttpGet("/getAllWallets")]
        public async Task<ActionResult<double>> GetAllWallets()
        {
            var result = await _walletService.GetListOfWallets();
            if (result != null) return Ok(result);

            _logger.LogInformation("Unable to get list of wallets");
            return BadRequest();
        }


    }
}
