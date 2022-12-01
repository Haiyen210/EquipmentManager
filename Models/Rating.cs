using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManager.Models
{
    [Table("Rating")]
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "int")]
        public int ProductId { get; set; }
        [Column(TypeName = "int")]
        public int AccountId { get; set; }
        [Column(TypeName = "float")]
        public double? Star { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModiredDate { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Product? Product { get; set; }
    }
}
