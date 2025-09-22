
using Microsoft.Extensions.Options;
using SmartTrinity.Core.Models;
using SmartTrinity.Shared.Infrastructure.Database;

public class CustomSalesUnitOfWork : SalesUnitOfWork
{
    public CustomSalesUnitOfWork(IOptions<AppSettings> options) : base(options)
    {

    }
}