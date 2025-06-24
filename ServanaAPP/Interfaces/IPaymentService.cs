namespace ServanaAPP.Interfaces
{
    public interface IPaymentService
    {
       public Task<string> HandlePaymentAsync(int requestId, string method);
    }
}
