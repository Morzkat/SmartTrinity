using Npgsql;
using System.Data;
using SmartTrinity.Core.Models;
using Microsoft.Extensions.Options;
using SmartTrinity.Shared.Core.Database;
using SmartTrinity.Shared.Infrastructure.Database;
using SmartTrinity.App.Sales.Infrastructure.Repositories;
using SmartTrinity.App.Payments.Core.Database.Repositories;

public interface ISmartSalesUnitOfWork : ISalesUnitOfWork
{
    ISmartSalesRepository SmartSalesRepository { get; }
}
public class SmartSalesUnitOfWork : SalesUnitOfWork, ISmartSalesUnitOfWork
{
    protected IDbConnection _connection;
    protected IDbTransaction _transaction;
    private readonly AppSettings _appSettings;

    public IPaymentRepository PaymentRepository { get; }
    public ISmartSalesRepository SmartSalesRepository { get; }

    public SmartSalesUnitOfWork(IOptions<AppSettings> optiona) : base(optiona)
    {
        _appSettings = optiona.Value;
        _connection = new NpgsqlConnection(_appSettings.TrinitySettings.TrinityConnectionString);

        PaymentRepository = new PaymentRepository(_connection, _transaction, _appSettings);
        SmartSalesRepository = new SmartSalesRepository(_connection, _transaction, optiona);
    }
}
