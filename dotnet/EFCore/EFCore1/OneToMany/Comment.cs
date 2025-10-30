using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneToMany
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public Article Article { get; set; } // Navigation property.


        // if you want to get the ArticleId only by one select to Comments table,
        // you can add this ArticleId manually, and mark it as foreign key in the configuration of the table.
        // Otherwise there's no need to declare this property and the EFCore will generate the ArticleId in the Comment table automatically.
        public int ArticleId { get; set; }

    }
}
