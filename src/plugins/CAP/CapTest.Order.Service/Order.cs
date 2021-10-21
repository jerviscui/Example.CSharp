using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapTest.Order.Service
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; private set; }

        [StringLength(50)]
        public string Number { get; private set; }

        public Order(int id, string number)
        {
            Id = id;
            Number = number;
        }
    }
}
