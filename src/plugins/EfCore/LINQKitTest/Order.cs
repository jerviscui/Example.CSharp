using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace LINQKitTest;

[DebuggerDisplay("Id={Id}")]
public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Amount { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    public IList<OrderDetail> Details { get; set; } = new List<OrderDetail>();
}
