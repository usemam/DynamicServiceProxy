namespace DynamicServiceProxy.Contracts
{
    public interface IUserService
    {
        AuthenticatedUser Authenticate(string login, string password);

        void ChangeUserOffice(int userId, int officeId);
    }
}
