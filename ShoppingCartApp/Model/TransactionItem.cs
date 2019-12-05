using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartApp.Model
{
    [Table("tb_t_TransactionItem")]
    public class TransactionItem
    {
        [Key] //for inisialization as primary key
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; }
        public Transaction Transaction { get; set; }

        public TransactionItem() { } //parameterless

        public TransactionItem(int quantity, Item item, Transaction transaction) //parameter
        {
            this.Quantity = quantity;
            this.Item = item;
            this.Transaction = transaction;
        }
    }
}
