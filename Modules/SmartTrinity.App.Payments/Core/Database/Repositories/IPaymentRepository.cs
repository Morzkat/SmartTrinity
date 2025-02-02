
using SmartTrinity.Core.Database;


namespace SmartTrinity.App.Payments.Core.Database.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<int> GetLastPaymentId();
        Task<List<Payment>> GetPaymentsFromId(int paymentId);
    }
}