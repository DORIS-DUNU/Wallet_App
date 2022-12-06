using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApp.Models.DTO;

namespace WalletApp.Abstractions.Services
{
    public interface IUserService
    {
        public Task<object> LoginAsync(LoginModel model);

        public Task<string> Register(UserDTO user);
       
    }
}
