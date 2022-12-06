using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using WalletApp.Abstractions.Repositories;
using WalletApp.Models.DTO;
using WalletApp.Models.Entities;
using Type = WalletApp.Models.Entities.Type;

namespace WalletApp.Infrastructure.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WalletRepository(WalletDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<string> CreateWalletAsync()
        {
            var address = GenerateAddress();
            var user = await _context.Users.Where(u => u.Id == GetId()).FirstOrDefaultAsync();
            if (user == null) return "Failed";
            GenerateHash(address, out byte[] AddressHash, out byte[] AddressKey);
            var wallet = new Wallet();
            wallet.UserId = user.Id;
            wallet.Address = address;
            wallet.AddressHash = AddressHash;
            wallet.AddressKey = AddressKey;
            wallet.Balance = 0;
            wallet.User = user;
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<bool> DepositAsync(DepositDto deposit)
        {
            var address = await _context.Wallets.Where(w => w.Address == deposit.Address).Include(u => u.User).FirstOrDefaultAsync();
            if (address == null || address.User.Id != GetId()) return false;
            address.Balance += deposit.Amount;

            var trans = new Transaction();

            trans.Type = Type.Credit;
            trans.WalletId = address.Id;
            trans.Amount = deposit.Amount;
            trans.Balance = address.Balance;
            trans.Wallet = address;
            _context.Transactions.Add(trans);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> WithdrawAsync(DepositDto withdraw)
        {
            var wallet = await _context.Wallets.Where(w => w.Address == withdraw.Address).Include(u => u.User).FirstOrDefaultAsync();
            if (wallet == null || wallet.User.Id != GetId() || wallet.Balance < withdraw.Amount) return false;
            wallet.Balance -= withdraw.Amount;

            var trans = new Transaction();

            trans.Type = Type.Debit;
            trans.WalletId = wallet.Id;
            trans.Amount = withdraw.Amount;
            trans.Balance = wallet.Balance;
            trans.Wallet = wallet;
            _context.Transactions.Add(trans);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> TransferAsync(TransferDto tranfer)
        {
            var wallet = await _context.Wallets.Where(w => w.Address == tranfer.SenderAddress).Include(u => u.User).FirstOrDefaultAsync();
            var wallet2 = await _context.Wallets.Where(w => w.Address == tranfer.RecieverAddress).Include(u => u.User).FirstOrDefaultAsync();
            if (wallet == null || wallet.User.Id != GetId() || wallet2 == null || wallet.Balance < tranfer.Amount) return false;
            wallet.Balance -= tranfer.Amount;
            wallet2.Balance += tranfer.Amount;


            var trans1 = new Transaction();
            trans1.Type = Type.Debit;
            trans1.WalletId = wallet.Id;
            trans1.Amount = tranfer.Amount;
            trans1.Balance = wallet.Balance;
            trans1.Wallet = wallet;
            _context.Transactions.Add(trans1);

            var trans2 = new Transaction();
            trans2.Type = Type.Credit;
            trans2.WalletId = wallet2.Id;
            trans2.Amount = tranfer.Amount;
            trans2.Balance = wallet2.Balance;
            trans2.Wallet = wallet2;
            _context.Transactions.Add(trans2);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<double?> GetBalanceAsync(string walletAddress)
        {

            var balance = 0.0;
            var result = await _context.Wallets.Where(x => x.Address == walletAddress)
                          .FirstOrDefaultAsync();

            if (result != null && result.UserId == GetId())
            {
                var isVerified = VerifyAddress(walletAddress, result.AddressHash, result.AddressKey);

                if (isVerified)
                {
                    var res = await _context.Wallets.FirstAsync(x => x.Address == result.Address);
                    balance = res.Balance;
                    return balance;
                }

                return null;
            }

            return null;
        }

        public async Task<List<Wallet>> GetListOfWallets()
        {
            var currentWallet = await _context.Wallets.Where(x => x.UserId == GetId())
                .AsNoTracking().ToListAsync();
            return currentWallet;
        }

        private void GenerateHash(string address, out byte[] AddressHash, out byte[] AddressKey)
        {

            using (var hash = new System.Security.Cryptography.HMACSHA512())
            {
                AddressKey = hash.Key;
                AddressHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(address));
            }
        }

        private bool VerifyAddress(string Address, byte[] AddressHash, byte[] AddressKey)
        {
            using (var hash = new System.Security.Cryptography.HMACSHA512(AddressKey))
            {
                var computedHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Address));
                return computedHash.SequenceEqual(AddressHash);
            }
        }

        private string GenerateAddress()
        {
            var address = "0x";
            var str = "ZQAx0sewCq2SWFu7yhn1jEbgTokMI3KLOPml4DiGRptrB5H8YNJUfv6cXVd9az";
            var rnd = new Random();
            for (int i = 0; i < 23; i++)
            {
                address += str[rnd.Next(0, 61)];
            }
            return address;
        }

    }
}
