using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManager.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "bit")]
        public bool? Status { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

    }
}
