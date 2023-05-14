using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DapperConsoleExample
{
  public class CustomerRepository : IDisposable
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
      const string sql = @"
					SELECT CustomerID
						,NameStyle
						,Title
						,FirstName
						,MiddleName
						,LastName
						,Suffix
						,CompanyName
						,SalesPerson
						,EmailAddress
						,Phone
						,PasswordHash
						,PasswordSalt
						,Rowguid
						,ModifiedDate
					FROM SalesLT.[Customer]";
      return await _db.QueryAsync<Customer>(sql, commandType: CommandType.Text);
    }
    public async Task<Customer> SelectCustomerById(int id)
    {
      const string sql = @"
					SELECT CustomerID
						,NameStyle
						,Title
						,FirstName
						,MiddleName
						,LastName
						,Suffix
						,CompanyName
						,SalesPerson
						,EmailAddress
						,Phone
						,PasswordHash
						,PasswordSalt
						,Rowguid
						,ModifiedDate
					FROM SalesLT.[Customer]
					WHERE CustomerID = @id";

      return await _db.QueryFirstOrDefaultAsync<Customer>(sql, new { id }, commandType: CommandType.Text);
    }
    public async Task<int> InsertCustomer(Customer customer)
    {
      const string sql = @"
					INSERT INTO SalesLT.[Customer] (
						NameStyle
						,Title
						,FirstName
						,MiddleName
						,LastName
						,Suffix
						,CompanyName
						,SalesPerson
						,EmailAddress
						,Phone
						,PasswordHash
						,PasswordSalt
						,Rowguid
						,ModifiedDate
						)
					VALUES (
						@NameStyle
						,@Title
						,@FirstName
						,@MiddleName
						,@LastName
						,@Suffix
						,@CompanyName
						,@SalesPerson
						,@EmailAddress
						,@Phone
						,@PasswordHash
						,@PasswordSalt
						,@Rowguid
						,@ModifiedDate
						);

					SELECT @@IDENTITY;";

      return await _db.QueryFirstOrDefaultAsync<int>(sql, new { NameStyle = customer.NameStyle, Title = customer.Title, FirstName = customer.FirstName, MiddleName = customer.MiddleName, LastName = customer.LastName, Suffix = customer.Suffix, CompanyName = customer.CompanyName, SalesPerson = customer.SalesPerson, EmailAddress = customer.EmailAddress, Phone = customer.Phone, PasswordHash = customer.PasswordHash, PasswordSalt = customer.PasswordSalt, Rowguid = customer.Rowguid, ModifiedDate = customer.ModifiedDate }, commandType: CommandType.Text);
    }
    public async Task UpdateCustomer(Customer customer)
    {
      const string sql = @"
					UPDATE SalesLT.[Customer]
					SET NameStyle = @NameStyle
						,Title = @Title
						,FirstName = @FirstName
						,MiddleName = @MiddleName
						,LastName = @LastName
						,Suffix = @Suffix
						,CompanyName = @CompanyName
						,SalesPerson = @SalesPerson
						,EmailAddress = @EmailAddress
						,Phone = @Phone
						,PasswordHash = @PasswordHash
						,PasswordSalt = @PasswordSalt
						,Rowguid = @Rowguid
						,ModifiedDate = @ModifiedDate
					WHERE NameStyle = @NameStyle";

      await _db.ExecuteAsync(sql, new { NameStyle = customer.NameStyle, Title = customer.Title, FirstName = customer.FirstName, MiddleName = customer.MiddleName, LastName = customer.LastName, Suffix = customer.Suffix, CompanyName = customer.CompanyName, SalesPerson = customer.SalesPerson, EmailAddress = customer.EmailAddress, Phone = customer.Phone, PasswordHash = customer.PasswordHash, PasswordSalt = customer.PasswordSalt, Rowguid = customer.Rowguid, ModifiedDate = customer.ModifiedDate }, commandType: CommandType.Text);
    }
    public async Task DeleteCustomer(Customer customer)
    {
      await using var db = new SqlConnection(_connstring);
      const string sql = @"DELETE FROM SalesLT.[Customer] WHERE NameStyle = @NameStyle";

      await db.ExecuteAsync(sql, new { customer.NameStyle }, commandType: CommandType.Text);
    }

    public async void Dispose()
    {
      await _db.DisposeAsync();
    }
  }
}
