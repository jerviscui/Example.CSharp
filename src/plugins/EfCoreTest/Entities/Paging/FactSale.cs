using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCoreTest.Paging;

[Table("fact_sales")]
public class FactSale
{
    //(date_id int, product_id int, store_id int,
    //quantity int, unit_price numeric(7,2), other_data char(1000))

    [Column("id")]
    public int Id { get; set; }

    [Column("date_id")]
    public int DateId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("store_id")]
    public int StoreId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("unit_price", TypeName = "numeric(7, 2)")]
    public decimal UnitPrice { get; set; }

    [Column("other_data", TypeName = "char(1000)")]
    [StringLength(1000)]
    public string OtherData { get; set; } = null!;
}
