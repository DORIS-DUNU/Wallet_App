using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WalletApp.Models.DTO;

namespace WalletApp.Abstractions.Services
{
    public interface IWalletService
    {
        public Task<string> CreateWalletAsync();
        public Task<bool> DepositAsync(DepositDto deposit);
        public Task<bool> WithdrawAsync(DepositDto withdraw);
        public Task<bool> TransferAsync(TransferDto tranfer);
        public Task<double?> GetBalanceAsync(string walletAddress, string currencyCode);
        public Task<IEnumerable<WalletDTO>> GetListOfWallets();

        //public Task<IEnumerable<TransactionDTO>> GetListOfTransactions()

    }
}
