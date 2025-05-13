using Dapper.Contrib.Extensions;

namespace DapperConsoleExample;

[Table("SalesLT_CustomerAddress")]
public class CustomerAddress : AdventureWorksBase
{
    public int CustomerID { get; set; }
    public int AddressID { get; set; }
    public string? AddressType { get; set; }
}

