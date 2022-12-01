using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManager.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(10)")]
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        [MaxLength(5, ErrorMessage = "Mã sản phẩm tối đa 5 ký tự")]
        [DisplayName("Mã sản phẩm")]
        public string Code { get; set; } = null!;
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [MinLength(5, ErrorMessage = "Tên sản phẩm ít nhất là 5 ký tự")]
        [MaxLength(150, ErrorMessage = "Tên sản phẩm tối đa 150 ký tự")]
        [DisplayName("Tên sản phẩm")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "varchar(max)")]
        //[Required(ErrorMessage = "Ảnh không được để trống")]
        [DisplayName("Ảnh")]
        public string? Images { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        [Required(ErrorMessage = "Mô tả không được để trống")]
        [DisplayName("Mô tả")]
        public string? Description { get; set; }
        [Column(TypeName = "float")]
        [Required(ErrorMessage = "Giá tiền không được để trống")]
        //[Range(minimum: 100, maximum: 100000000000, ErrorMessage = "Lương tối thiếu phải là 100, tối đa 100 tỷ")]
        [DisplayName("Giá tiền")]
        public double? Price { get; set; }
        [Column(TypeName = "float")]
        [DisplayName("Giá khuyến mại")]
        public double? SalePrice { get; set; }
        [Column(TypeName = "bit")]
        [DisplayName("Trạng thái")]
        public bool? Status { get; set; }
        [Column(TypeName = "int")]
        public int CategoryId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModiredDate { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }


        public Product()
        {
            Favorites = new HashSet<Favorite>();
            OrderDetails = new HashSet<OrderDetail>();
            Ratings = new HashSet<Rating>();
        }
    }
}
