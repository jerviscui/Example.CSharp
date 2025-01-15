using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace LINQKitTest;

[DebuggerDisplay("Id={Id}, FK={OrderId}")]
[Index(nameof(Goods))]
public class OrderDetail
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Goods { get; set; } = null!;

    [ForeignKey(nameof(OrderId))]
    public int OrderId { get; set; }
}
