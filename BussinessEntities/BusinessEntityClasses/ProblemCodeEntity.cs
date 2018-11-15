
namespace BusinessEntities.BusinessEntityClasses
{
    public class ProblemCodeEntity
    {
        public System.Guid ClientProblemCodeId { get; set; }
        public System.Guid Client { get; set; }
        public System.Guid ClientProblemClass { get; set; }
        public string ProblemCodeName { get; set; }
        public string ActiveFlag { get; set; }
    }
}
