
using SmartTrinity.Shared.Core.Database;
using SmartTrinity.App.Payments.Core.Database.Repositories;

public interface ICustomSalesUnitOfWork : ISalesUnitOfWork
{
   IPaymentRepository PaymentRepository { get; }
}
