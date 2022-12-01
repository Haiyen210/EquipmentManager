using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManager.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(10)")]
        [DisplayName("Mã danh mục")]
        [Required(ErrorMessage = "Mã danh mục không được để trống")]
        [StringLength(4, ErrorMessage = "Mã danh mục không vượt quá 4 ký tự")]
        public string Code { get; set; } = null!;
        [DisplayName("Tên danh mục")]
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Tên Danh mục không được để trống")]
        [MinLength(5, ErrorMessage = "Tên Danh mục ít nhất là 5 ký tự")]
        [MaxLength(50, ErrorMessage = "Tên Danh mục tối đa 50 ký tự")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "bit")]
        [DisplayName("Trạng thái")]
        public bool? Status { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModiredDate { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public Category()
        {
            Products = new HashSet<Product>();
        }

    }
}
