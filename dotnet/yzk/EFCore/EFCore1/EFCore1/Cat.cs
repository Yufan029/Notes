using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore1
{
    [Table("T_Cats")]
    public class Cat
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(22)]
        public string Name { get; set; }
    }
}
