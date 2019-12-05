using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartApp.Model
{
    [Table("tb_m_auth")]
    public class Auth
    {
        [Key] //for inisialization as primary key
        public int Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

        public Auth() { } //parameterless

        public Auth(string role, string email) //parameter
        {
            this.Role = role;
            this.Email = email;
        }
    }
}
