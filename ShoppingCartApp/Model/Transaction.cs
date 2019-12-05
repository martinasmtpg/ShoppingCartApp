using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartApp.Model
{
    [Table("tb_m_transaction")]
    public class Transaction
    {
        [Key] //for inisialization as primary key
        public int Id { get; set; }
        public int Total { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public Transaction()
        {
            this.OrderDate = DateTimeOffset.Now.LocalDateTime;
        } //parameterless

        public Transaction(int total) //parameter
        {
            this.Total = total;
        }
    }
}
