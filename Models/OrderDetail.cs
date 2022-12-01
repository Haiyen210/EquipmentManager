using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManager.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "int")]
        public int ProductId { get; set; }
        [Column(TypeName = "int")]
        public int OrdersId { get; set; }
        [Column(TypeName = "int")]
        public int? Quantity { get; set; }
        [Column(TypeName = "float")]
        public double? Price { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModiredDate { get; set; }

        public virtual Order? Orders { get; set; }
        public virtual Product? Product { get; set; }
    }
}
