using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WalletApp.Models.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Type
    {
        Debit, Credit
    }
    public class Transaction
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public int WalletId { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public Wallet Wallet { get; set; }
    }
}
