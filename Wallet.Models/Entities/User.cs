using Microsoft.AspNetCore.Identity;

namespace WalletApp.Models.Entities
{
    public class User:IdentityUser
    {
        public string Name { get; set; }
        public ICollection<Wallet> Wallets { get; set; }
    }
}