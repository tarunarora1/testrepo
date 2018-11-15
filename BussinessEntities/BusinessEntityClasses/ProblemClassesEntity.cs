
namespace BusinessEntities.BusinessEntityClasses
{
    public class ProblemClassesEntity
    {
        public System.Guid ClientProblemClassId { get; set; }
        public System.Guid Client { get; set; }
        public string ProblemClassName { get; set; }
        public string ActiveFlag { get; set; }
    }

    public class ClientVendorProblemClassEntity
    {
        public System.Guid ClientVendorProblemClassesId { get; set; }
        public System.Guid ClientVendor { get; set; }
        public System.Guid ProblemClass { get; set; }
    }
}
