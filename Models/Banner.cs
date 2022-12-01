using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManager.Models
{
    [Table("Banner")]
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(10)")]
        [DisplayName("Mã banner")]
        [Required(ErrorMessage = "Mã banner không được để trống")]
        [MaxLength(5, ErrorMessage = "Mã banner không vượt quá 5 ký tự")]
        public string Code { get; set; } = null!;
        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("Tên banner")]
        [Required(ErrorMessage = "Tên banner không được để trống")]
        [MinLength(5, ErrorMessage = "Tên banner ít nhất là 5 ký tự")]
        [MaxLength(50, ErrorMessage = "Tên banner tối đa 50 ký tự")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "varchar(max)")]
        //[Required(ErrorMessage = "Ảnh không được để trống")]
        [DisplayName("Ảnh")]
        public string? Images { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }
        [Column(TypeName = "bit")]
        [DisplayName("Trạng thái")]
        public bool? Status { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModiredDate { get; set; }

        public List<Banner> GetBannersList()
        {
            List<Banner> banner = new List<Banner>();
            return banner;
        }
    }
}
