using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Models.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public byte[] AddressHash { get; set; }
        public byte[] AddressKey { get; set; }
        public double Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public User User { get; set; }
    }
}
