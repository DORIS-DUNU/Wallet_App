using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Models.DTO
{
    public class TransferDto
    {
        public string SenderAddress { get; set; }
        public string RecieverAddress { get; set; }
        public double Amount { get; set; }

    }
}
