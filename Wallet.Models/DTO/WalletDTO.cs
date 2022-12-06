using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Models.Entities;

namespace WalletApp.Models.DTO
{
    public class WalletDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        //public string SecurityKey { get; set; }
        public double Balance { get; set; }
        // public ICollection<Transaction> Transactions { get; set; }
    }
}
