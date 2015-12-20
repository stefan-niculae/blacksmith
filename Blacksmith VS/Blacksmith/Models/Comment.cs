using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Blacksmith.Models
{
    public class Comment
    {
        [Key, ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(500), DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required/*, ForeignKey("Submitter")*/]
        public string SubmitterId { get; set; }
//        public virtual User Submitter { get; set; }
        
        [Required, ForeignKey("Link")]
        public int LinkId { get; set; }
        public virtual Link Link { get; set; }
    }
}