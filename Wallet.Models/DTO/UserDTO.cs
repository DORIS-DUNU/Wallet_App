using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Models.DTO
{
    public class UserDTO

    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
