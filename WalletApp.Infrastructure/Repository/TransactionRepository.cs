using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WalletApp.Abstractions.Repositories;
using WalletApp.Models.Entities;

namespace WalletApp.Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly WalletDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionRepository(WalletDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IEnumerable<IEnumerable<Transaction>>> GetAllUserTransactionsAsync()
        {

            var currentWallet = _context.Wallets.Where(x => x.UserId == GetId())
               .Include(s => s.User)
               .Include(e => e.Transactions)
               .AsNoTracking();

            var listOfTransactions = new List<ICollection<Transaction>>();

            foreach (var item in currentWallet)
            {
                listOfTransactions.Add(item.Transactions);
            }

            return listOfTransactions;
        }


        public async Task<IEnumerable<Transaction>> GetWalletStatementAsync(string walletAddress, int page)
        {
            var currentWallet = await _context.Wallets
                .Include(s => s.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Address == walletAddress);

            if (currentWallet == null || GetId() != currentWallet.UserId)
            {
                return null;
            }

            if (GetId() != currentWallet.UserId)
            {
                return null;
            }
            var result = _context.Transactions.Where(x => x.WalletId == currentWallet.Id).ToList();
            var pageResults = 5f;
            var pageCount = Math.Ceiling(result.Count() / pageResults);

            var paginatedResult = result.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();
            await _context.SaveChangesAsync();

            return paginatedResult;
        }
    }
}
