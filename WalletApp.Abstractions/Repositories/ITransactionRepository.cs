using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Models.DTO;
using WalletApp.Models.Entities;

namespace WalletApp.Abstractions.Repositories
{
    public interface ITransactionRepository
    {
        public Task<IEnumerable<Transaction>> GetWalletStatementAsync(string walletAddress, int page);
        public Task<IEnumerable<IEnumerable<Transaction>>> GetAllUserTransactionsAsync();
    }
}
