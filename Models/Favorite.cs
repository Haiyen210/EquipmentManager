using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManager.Models
{
    [Table("Favorite")]
    public class Favorite
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "int")]
        public int ProductId { get; set; }
        [Column(TypeName = "int")]
        public int AccountId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ModiredDate { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Product? Product { get; set; }
    }
}
