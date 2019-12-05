using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartApp.Model
{
    [Table("tb_t_user")]
    public class User
    {
        [Key] //for inisialization as primary key
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Auth Auth { get; set; }

        public User() { } //parameterless

        public User(string name, string email, string pass, Auth auth) //parameter
        {
            this.Name = name;
            this.Email = email;
            this.Password = pass;
            this.Auth = auth;
        }
    }
}
