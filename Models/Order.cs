using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManager.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("Tên người nhận")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "varchar(150)")]
        [DisplayName("Địa chỉ email")]
        public string Email { get; set; } = null!;
        [Column(TypeName = "varchar(10)")]
        [DisplayName("Số điện thoại")]
        public string Phone { get; set; } = null!;
        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("Địa chỉ")]
        public string Address { get; set; } = null!;
        [Column(TypeName = "nvarchar(max)")]
        [DisplayName("Ghi chú đơn hàng")]
        public string? Note { get; set; }
        [Column(TypeName = "int")]
        public int? AccountId { get; set; }
        [Column(TypeName = "int")]
        [DisplayName("Tổng số lượng")]
        public int? TotalQuantity { get; set; }
        [Column(TypeName = "float")]
        [DisplayName("Tổng tiền")]
        public double? TotalPrice { get; set; }
        [Column(TypeName = "tinyint")]
        [DisplayName("Trạng thái")]
        public byte Status { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModiredDate { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
    }
}
