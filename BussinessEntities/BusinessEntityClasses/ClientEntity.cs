namespace BusinessEntities.BusinessEntityClasses
{
    public class ClientEntities
    {
        public System.Guid ClientId { get; set; }
        public string ClientName { get; set; }
    }

    public class ChangePasswordEntity
    {
        public string _OldPass { get; set; }
        public string _NewPass { get; set; }
        public string _loggedInOktaUserID { get; set; }
    }
}
