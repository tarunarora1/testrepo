
namespace BusinessEntities.BusinessEntityClasses
{
    public class ClientUsersEntity
    {
        public System.Guid ClientUserID { get; set; }
        public System.Guid ClientID { get; set; }
        public System.Guid RoleID { get; set; }
        public System.Guid UserID { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email{ get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }


    }
}
