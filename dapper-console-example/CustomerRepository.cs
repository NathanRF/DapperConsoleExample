using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace DapperConsoleExample
{
  public class CustomerRepository : IAsyncDisposable
  {
    private readonly string _connstring;
    private readonly SqlConnection _db;

    public CustomerRepository(string connstring)
    {
      _connstring = connstring;
      _db = new SqlConnection(_connstring);
    }

    public async Task<IEnumerable<Customer>> SelectCustomer()
    {
      string sql =
        @"SELECT *
          FROM [SalesLT].[Customer] C
          JOIN [SalesLT].[CustomerAddress] CA
            ON CA.CustomerID = C.CustomerID";
      return await _db.QueryAsync<Customer, CustomerAddress, Customer>(sql,
        (customer, address) => {
          customer.CustomerAddress = address;
          return customer;
        }, splitOn: "CustomerID");
    }
    public async Task<Customer> SelectCustomerById(int id)
    {
      return await _db.GetAsync<Customer>(id);
    }
    public async Task<int> InsertCustomer(Customer customer)
    {
      return await _db.InsertAsync(customer);
    }
    public async Task UpdateCustomer(Customer customer)
    {
      await _db.UpdateAsync(customer);
    }
    public async Task DeleteCustomer(Customer customer)
    {
      await _db.DeleteAsync(customer);
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
      await _db.DisposeAsync();
    }
  }
}
