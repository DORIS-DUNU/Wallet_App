<<<<<<< HEAD

=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Abstractions.Repositories;
using WalletApp.Abstractions.Services;
using WalletApp.Models.DTO;
using WalletApp.Utils;

namespace WalletApp.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionService _transactionService;

        public WalletService(IWalletRepository walletRepository, ITransactionService transactionService)
        {
            _walletRepository = walletRepository;
            _transactionService = transactionService;
        }
        
        public async Task<string> CreateWalletAsync()
        {
            var address = await _walletRepository.CreateWalletAsync();
            return address;
        }

        

    }
}
