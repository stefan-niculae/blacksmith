using System;
using System.ComponentModel.DataAnnotations;

namespace Blacksmith.Models
{
    public class Favorite
    {
        // Maybe it would have been better to use a composite primary key,
        // the combination of the user and link IDs
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