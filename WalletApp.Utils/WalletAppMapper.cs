using WalletApp.Models.DTO;
using WalletApp.Models.Entities;

namespace WalletApp.Utils
{
    public class WalletAppMapper
    {
        public static WalletDTO WalletToDTO(Wallet model)
        {
            var walletDTO = new WalletDTO();
            walletDTO.Id = model.Id;
            walletDTO.Address = model.Address;
            walletDTO.UserId = model.UserId;
            walletDTO.Balance = model.Balance;


            return walletDTO;
        }

        public static Wallet DTOToWallet(WalletDTO model)
        {
            var wallet = new Wallet();
            wallet.Address = model.Address;
            return wallet;
        }

        public static TransactionDTO TransactioToDTO(Transaction transaction)
        {
            var transactionDTO = new TransactionDTO();
            transactionDTO.Id = transaction.Id;
            transactionDTO.WalletId = transaction.WalletId;
            transactionDTO.Type = (Models.DTO.Type)transaction.Type;
            transactionDTO.Amount = transaction.Amount;
            transactionDTO.Balance = transaction.Balance;
            return transactionDTO;
        }
    }
}