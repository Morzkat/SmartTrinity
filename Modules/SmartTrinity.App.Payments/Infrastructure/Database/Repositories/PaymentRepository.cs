

using System.Data;
using SmartTrinity.Core.Models;
using SmartTrinity.App.Payments.Core.Database.Repositories;
using SmartTrinity.Shared.Infrastructure.Database.Repositories;
using System.Text;
using Dapper;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; }
    public AppSettings _appSettings { get; }

    public PaymentRepository(IDbConnection connection, IDbTransaction transaction, AppSettings appSettings) : base(connection, transaction)
    {
        tableId = "id";
        tableName = "pagos";
        Connection = connection;
        Transaction = transaction;
        _appSettings = appSettings;
    }

    public async Task<int> GetLastPaymentId()
    {
        var query = new StringBuilder("SELECT id, fecha_pago as date, metodo_pago as method, dato_otro as other_data, tipo_otro as other_type, ")
            .Append("venta_sistema as system_sale, venta_fabricada as user_sale, tarjeta as card, placa as plate, sale_id, cuadre_id as tally_id, ")
            .Append("turno as shift, amount, itbis, verifone_type, payment_id, cliente as Client, rnc, bombero as fuel_station_attendant, money ")
            .Append("FROM pagos;");

        List<Payment> payments = await SqlMapper.QueryAsync<Payment>(_dbSet, query.ToString(), transaction: _transaction);
        return payments.Count == 0 ? 0 : (int)payments[0].Id;
    }

    public async Task<List<Payment>> GetPaymentsFromId(int paymentId)
    {
        var parameters = new { paymentId, limit = 100 };
        var query = new StringBuilder("SELECT id, fecha_pago as date, metodo_pago as method, dato_otro as other_data, tipo_otro as other_type, ")
            .Append("venta_sistema as system_sale, venta_fabricada as user_sale, tarjeta as card, placa as plate, sale_id, cuadre_id as tally_id, ")
            .Append("turno as shift, amount, itbis, verifone_type, payment_id, cliente as Client, rnc, bombero as fuel_station_attendant, money ")
            .Append("FROM pagos")
            .Append("WHERE id > @paymentId")
            .Append("ORDER BY id DESC LIMIT @limit");

        List<Payment> payments = await SqlMapper.QueryAsync<Payment>(_dbSet, query.ToString(), parameters, transaction: _transaction);
        return payments;
    }

    public async Task<int> AddToCentralServer(int stationId, Payment payment)
    {
         var query = new StringBuilder("INSERT INTO pagos ") 
            .Append("fecha_pago, metodo_pago, dato_otro, tipo_otro, ")
            .Append("venta_sistema, venta_fabricada, tarjeta, placa, sale_id, cuadre_id, ")
            .Append("turno, amount, itbis, verifone_type, payment_id, cliente, rnc, bombero, money) ")
            .Append(" VALUES ")
            .Append("( @Date, @Method, @OtherData, @OtherType, @SystemSale, @UserSale, @Card, @Plate, @SaleId, @TallyId, ")
            .Append(" @Shift, @Amount, @Itbis, @VeriphoneType, @PaymentId, @Client, @Rnc, @FuelStationAttendant, @Money)")
            ;

            var r = await SqlMapper.ExecuteAsync(_dbSet, query.ToString(), payment, transaction: _transaction);
            
        return 0;
    }
}