using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore1
{
    [Table("BirdsAnno")]
    public class Bird
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Number { get; set; }
    }
}
