using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Models.DTO;
using WalletApp.Models.Entities;

namespace WalletApp.Abstractions.Services
{
    public interface ITransactionService
    {
        public Task<double?> ConvertCurrencyAsync(string currencyA, string currencyB, double amount);
        public Task<double?> GetRateAsync(string currencyCode, double? amount);

        public Task<IEnumerable<TransactionDTO>> GetAllUserTransactionsAsync();
        public Task<IEnumerable<TransactionDTO>> GetWalletStatementAsync(string walletAddress, int page);

    }
}
