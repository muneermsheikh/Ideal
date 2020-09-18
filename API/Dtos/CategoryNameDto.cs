namespace API.Dtos
{
    public class CategoryNameDto
    {
        public CategoryNameDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id {get; set;}
        public string Name {get; set; }
    }
}