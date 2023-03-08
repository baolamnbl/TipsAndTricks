using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public int authorId { get; set; }

        public int categoryId { get; set; }

        public string categoryName { get; set; }

        public string keyWord { get; set; }

        public int postMonth { get; set; }

        public int postYear { get; set; }
        public bool PublishedOnly { get; set; }
        public bool NotPublished { get; set; }
        public string CategorySlug { get; set; }
        public string AuthorSlug { get; set; }
        public string TagSlug { get; set; }
        public string TitleSlug { get; set; }
    }
}
