namespace Core.Entities.Masters
{
  public class DomainSub: BaseEntity
    {
        public DomainSub()
        {
        }

        public DomainSub(string domainSubName)
        {
            DomainSubName = domainSubName;
        }

        public string DomainSubName { get; set; }
    }
}