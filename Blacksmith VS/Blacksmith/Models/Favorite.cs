using System;
using System.ComponentModel.DataAnnotations;

namespace Blacksmith.Models
{
    public class Favorite
    {
        [Key, ScaffoldColumn(false)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Category { get; set; }
        
        [Required, DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual Link Link { get; set; }
    }
}