using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blacksmith.Models
{
    public class Link
    {
        [Key, ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string Address { get; set; }

        [StringLength(1000), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        public virtual User Submitter { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Favorite> Favorites { get; set; }

        // Use services to provide favicon and thumbnail
        [NotMapped]
        public string Favicon => $@"http://www.google.com/s2/favicons?domain_url={Address}";
        [NotMapped]
        public string Thumbnail => $@"http://images.shrinktheweb.com/xino.php?stwembed=1&stwxmax=320&stwymax=240&stwaccesskeyid=c7fc29b382dca49&stwurl=http://{Address}";
    }
}