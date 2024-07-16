using Microsoft.AspNetCore.Mvc.Rendering;

namespace bbs.Models
{
    public class PostViewModel
    {
        public List<Post> Posts { get; set; }
        public string SortOrder { get; set; }
        public IEnumerable<SelectListItem> SortOptions { get; set; }
        public string SearchString { get; set; }

        public PostViewModel()
        {
            SortOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "content_asc", Text = "内容 (昇順)" },
            new SelectListItem { Value = "content_desc", Text = "内容 (降順)" },
            new SelectListItem { Value = "date_asc", Text = "投稿日時 (昇順)" },
            new SelectListItem { Value = "date_desc", Text = "投稿日時 (降順)" },
            new SelectListItem { Value = "user_asc", Text = "ユーザー (昇順)" },
            new SelectListItem { Value = "user_desc", Text = "ユーザー (降順)" }
        };
        }
    }

}
