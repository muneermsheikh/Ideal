using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public int Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string IndustryType { get; set; }

        [Required]
        public string SkillLevel { get; set; }
    }
}