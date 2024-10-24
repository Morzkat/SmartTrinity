using SmartTrinity.App.Pumps.Core.Database.Repositories;
using SmartTrinity.Core.Database;

namespace SmartTrinity.App.Pumps.Core.Database
{

    public interface IPumpsUnitOfWork : IUnitOfWork
    {
        IPumpsRepository PumpsRepository { get; }
        IGenericConfigValuesRepository GenericConfigValuesRepository { get; }
    }

}