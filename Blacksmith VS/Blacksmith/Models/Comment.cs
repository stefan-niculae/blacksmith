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

//        [Required]
//        public string SubmitterId;
        //[ForeignKey("Id")]
        public virtual User Submitter { get; set; }

        //[Required]
        //public int LinkId;
        //[ForeignKey("LinkId")]
        public virtual Link Link { get; set; }
    }
}