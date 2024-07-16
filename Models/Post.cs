using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace bbs.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("内容")]
        public string ?Content { get; set; }

        [Required]
        public string ?UserId { get; set; }

        [Required]
        [DisplayName("ユーザー")]
        public string ?UserName { get; set; }

        [Required]
        [DisplayName("投稿日時")]
        public DateTime CreatedAt { get; set; }
    }
}
