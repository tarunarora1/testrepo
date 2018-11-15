using BusinessEntities.BusinessEntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfWork.Interface
{
    public interface IUserLogin
    {     
       List<KeyValuePair<Guid, string>> GetLoginUserRole(string UserEmailId, string application);

        bool LoginUser(LoginUserEntity item);
    }
}
