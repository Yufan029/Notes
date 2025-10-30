using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneToMany
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public List<Comment> Comments = new List<Comment>();    // Navigation property. 
    }
}
