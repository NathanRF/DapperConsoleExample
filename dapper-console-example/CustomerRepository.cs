using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using System.Data.SQLite;

namespace DapperConsoleExample
{
  public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
  {
    public override Guid Parse(object value)
    {
      Guid.TryParse(value.ToString(), out Guid result);
      return result;
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
      parameter.Value = value.ToString();
    }
  }

  public class CustomerRepository : IAsyncDisposable
  {
    private readonly string _connstring;
    private readonly SQLiteConnection _db;

    public CustomerRepository(string connstring)
    {
      _connstring = connstring;
      _db = new SQLiteConnection(_connstring);
    }

    public async Task<IEnumerable<Customer>> SelectCustomer()
    {
      string sql =
        @"SELECT 
            C.CustomerID,
            C.NameStyle,
            C.Title,
            C.FirstName,
            C.MiddleName,
            C.LastName,
            C.Suffix,
            C.CompanyName,
            C.SalesPerson,
            C.EmailAddress,
            C.Phone,
            C.PasswordHash,
            C.PasswordSalt,
            C.Rowguid,
            C.ModifiedDate,
            CA.AddressID,
            CA.CustomerID,
            CA.AddressType,
            CA.Rowguid,
            CA.ModifiedDate
          FROM SalesLT_Customer C
          JOIN SalesLT_CustomerAddress CA
            ON CA.CustomerID = C.CustomerID";
      return await _db.QueryAsync<Customer, CustomerAddress, Customer>(sql,
        (customer, address) =>
        {
          customer.CustomerAddress = address;
          return customer;
        }, splitOn: "AddressID"); //TODO columns with the same name might be conflicting
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


    public async Task Init()
    {
      SqlMapper.AddTypeHandler(new GuidTypeHandler());

      await InitCustomer();
      await InitCustomerAddress();

      await SeedCustomer();
      await SeedCustomerAddress();
    }

    private async Task<int> InitCustomer()
    {
      var sql = """
        
        CREATE TABLE IF NOT EXISTS SalesLT_Customer (
            CustomerID INTEGER PRIMARY KEY AUTOINCREMENT,
            NameStyle INTEGER NOT NULL,
            Title TEXT,
            FirstName TEXT,
            MiddleName TEXT,
            LastName TEXT,
            Suffix TEXT,
            CompanyName TEXT,
            SalesPerson TEXT,
            EmailAddress TEXT,
            Phone TEXT,
            PasswordHash TEXT,
            PasswordSalt TEXT,
            ModifiedDate TEXT NOT NULL,
            Rowguid TEXT NOT NULL COLLATE NOCASE
        );
        
        """;
      return await _db.ExecuteAsync(sql);
    }

    private async Task<int> InitCustomerAddress()
    {
      var sql = """
        
        CREATE TABLE IF NOT EXISTS SalesLT_CustomerAddress (
            CustomerID INTEGER NOT NULL,
            AddressID INTEGER NOT NULL,
            AddressType TEXT,
            ModifiedDate TEXT NOT NULL,
            Rowguid TEXT NOT NULL COLLATE NOCASE,
            PRIMARY KEY (CustomerID, AddressID),
            FOREIGN KEY (CustomerID) REFERENCES SalesLT_Customer(CustomerID)
        );
        
        """;
      return await _db.ExecuteAsync(sql);
    }

    private async Task SeedCustomer()
    {
      var sql =
        """
        INSERT OR IGNORE INTO SalesLT_Customer (
            CustomerID,
            NameStyle,
            Title,
            FirstName,
            MiddleName,
            LastName,
            Suffix,
            CompanyName,
            SalesPerson,
            EmailAddress,
            Phone,
            PasswordHash,
            PasswordSalt,
            Rowguid,
            ModifiedDate
        ) 
        VALUES
        (1, 0, 'Mr.', 'Orlando', 'N.', 'Gee', NULL, 'A Bike Store', 'adventure-works\\pamela0', 'orlando0@adventure-works.com', '245-555-0173', 'L/Rlwxzp4w7RWmEgXX+/A7cXaePEPcp+KwQhl2fJL7w=', '1KjXYs4=', '3F5AE95E-B87D-4AED-95B4-C3797AFCB74F', '2005-08-01 00:00:00.000'),
        
        (2, 0, 'Mr.', 'Keith', NULL, 'Harris', NULL, 'Progressive Sports', 'adventure-works\\david8', 'keith0@adventure-works.com', '170-555-0127', 'YPdtRdvqeAhj6wyxEsFdshBDNXxkCXn+CRgbvJItknw=', 'fs1ZGhY=', 'E552F657-A9AF-4A7D-A645-C429D6E02491', '2006-08-01 00:00:00.000'),
        
        (3, 0, 'Ms.', 'Donna', 'F.', 'Carreras', NULL, 'Advanced Bike Components', 'adventure-works\\jillian0', 'donna0@adventure-works.com', '279-555-0130', 'LNoK27abGQo48gGue3EBV/UrlYSToV0/s87dCRV7uJk=', 'YTNH5Rw=', '130774B1-DB21-4EF3-98C8-C104BCD6ED6D', '2005-09-01 00:00:00.000'),

        (4, 0, 'Ms.', 'Janet', 'M.', 'Gates', NULL, 'Modular Cycle Systems', 'adventure-works\\jillian0', 'janet1@adventure-works.com', '710-555-0173', 'ElzTpSNbUW1Ut+L5cWlfR7MF6nBZia8WpmGaQPjLOJA=', 'nm7D5e4=', 'FF862851-1DAA-4044-BE7C-3E85583C054D', '2006-07-01 00:00:00.000'),

        (5, 0, 'Mr.', 'Lucy', NULL, 'Harrington', NULL, 'Metropolitan Sports Supply', 'adventure-works\\shu0', 'lucy0@adventure-works.com', '828-555-0186', 'KJqV15wsX3PG8TS5GSddp6LFFVdd3CoRftZM/tP0+R4=', 'cNFKU4w=', '83905BDC-6F5E-4F71-B162-C98DA069F38A', '2006-09-01 00:00:00.000'),

        (6, 0, 'Ms.', 'Rosmarie', 'J.', 'Carroll', NULL, 'Aerobic Exercise Company', 'adventure-works\\linda3', 'rosmarie0@adventure-works.com', '244-555-0112', 'OKT0scizCdIzymHHOtyJKQiC/fCILSooSZ8dQ2Y34VM=', 'ihWf50M=', '1A92DF88-BFA2-467D-BD54-FCB9E647FDD7', '2007-09-01 00:00:00.000'),

        (7, 0, 'Mr.', 'Dominic', 'P.', 'Gash', NULL, 'Associated Bikes', 'adventure-works\\shu0', 'dominic0@adventure-works.com', '192-555-0173', 'ZccoP/jZGQm+Xpzc7RKwDhS11YFNybwcPVRYTSNcnSg=', 'sPoUBSQ=', '03E9273E-B193-448E-9823-FE0C44AEED78', '2006-07-01 00:00:00.000'),

        (10, 0, 'Ms.', 'Kathleen', 'M.', 'Garza', NULL, 'Rural Cycle Emporium', 'adventure-works\\josé1', 'kathleen0@adventure-works.com', '150-555-0127', 'Qa3aMCxNbVLGrc0b99KsbQqiVgwYDfHcsK9GZSUxcTM=', 'Ls05W3g=', 'CDB6698D-2FF1-4FBA-8F22-60AD1D11DABD', '2006-09-01 00:00:00.000'),

        (11, 0, 'Ms.', 'Katherine', NULL, 'Harding', NULL, 'Sharp Bikes', 'adventure-works\\josé1', 'katherine0@adventure-works.com', '926-555-0159', 'uRlorVzDGNJIX9I+ehTlRK+liT4UKRgWhApJgUMC2d4=', 'jpHKbqE=', '750F3495-59C4-48A0-80E1-E37EC60E77D9', '2005-08-01 00:00:00.000'),

        (12, 0, 'Mr.', 'Johnny', 'A.', 'Caprio', 'Jr.', 'Bikes and Motorbikes', 'adventure-works\\garrett1', 'johnny0@adventure-works.com', '112-555-0191', 'jtF9jBoFYeJTaET7x+eJDkd7BzMz15Wo9odbGPBaIak=', 'wVLnvHo=', '947BCAF1-1F32-44F3-B9C3-0011F95FBE54', '2006-08-01 00:00:00.000')
        """;

      await _db.ExecuteAsync(sql);
    }

    private async Task SeedCustomerAddress()
    {
      var sql =
        """
        INSERT OR IGNORE INTO SalesLT_CustomerAddress (
            CustomerID,
            AddressID,
            AddressType,
            ModifiedDate,
            Rowguid
        )
        VALUES
        (1, 1001, 'Shipping', '2005-08-02 00:00:00.000', '3F5AE95E-B87D-4AED-95B4-C3797AFCB74F'),

        (1, 1002, 'Billing', '2005-08-09 00:00:00.000', '3F5AE95E-B87D-4AED-95B4-C3797AFCB74F'),

        (2, 2001, 'Home', '2006-08-02 00:00:00.000', 'E552F657-A9AF-4A7D-A645-C429D6E02491'),

        (2, 2002, 'Office', '2006-08-05 00:00:00.000', 'E552F657-A9AF-4A7D-A645-C429D6E02491'),

        (3, 3001, 'Shipping', '2005-09-02 00:00:00.000', '130774B1-DB21-4EF3-98C8-C104BCD6ED6D'),

        (3, 3002, 'Headquarters', '2005-09-03 00:00:00.000', '130774B1-DB21-4EF3-98C8-C104BCD6ED6D'),

        (12, 12001, 'Warehouse', '2006-08-02 00:00:00.000', '947BCAF1-1F32-44F3-B9C3-0011F95FBE54'),

        (12, 12002, 'Showroom', '2006-08-05 00:00:00.000', '947BCAF1-1F32-44F3-B9C3-0011F95FBE54')
        """;

      await _db.ExecuteAsync(sql);
    }
  }
}
