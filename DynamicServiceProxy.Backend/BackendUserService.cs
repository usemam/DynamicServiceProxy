using DynamicServiceProxy.Contracts;

namespace DynamicServiceProxy.Backend
{
    public class BackendUserService : IUserService
    {
        public AuthenticatedUser Authenticate(string login, string password)
        {
            // do something here
            return new AuthenticatedUser();
        }

        public void ChangeUserOffice(int userId, int officeId)
        {
            // do something here
        }
    }
}
