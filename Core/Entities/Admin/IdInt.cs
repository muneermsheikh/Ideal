using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Admin
{
    [NotMapped]
    public class IdInt
    {
        public int Id {get; set; }
    }

    [NotMapped]
    public class strObject
    {
        public string Name {get; set;}
    }
}