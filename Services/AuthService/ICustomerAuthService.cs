namespace VirtualMenuAPI.Authentication{
  public interface ICustomerAuthService
  {
    Task<string> AddNewCustomer();
  }
}