namespace Core.Entities.apiBasket
{
    public class apiBasketItem
    {
        public int Id {get; set; }
        public string CategoryName {get; set; }
        public int CategoryId {get; set; }
        public int Quantity {get; set; }
        public int IndustryTypeId {get; set; }
        public int SkillLevelId {get; set; }
    }
}