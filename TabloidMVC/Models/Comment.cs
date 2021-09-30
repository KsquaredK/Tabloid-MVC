using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Post")]
        public int PostId { get; set; }
        public Category Post { get; set; }

        [DisplayName("Author")]
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [DisplayName("Published")]
        [DataType(DataType.Date)]
        public DateTime? CreateDateTime { get; set; }
    }
}
