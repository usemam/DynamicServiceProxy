using DynamicServiceProxy.Contracts;

namespace DynamicServiceProxy.Client
{
    public class ClientUserService : Proxy<IUserService>
    {
        public AuthenticatedUser Authenticate(string login, string password)
        {
            // do something
            return new AuthenticatedUser();
        }
    }
}
