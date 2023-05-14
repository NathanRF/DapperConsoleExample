using Dapper.Contrib.Extensions;

namespace DapperConsoleExample
{
  [Table("SalesLT.Customer")]
  public class Customer
  {
    [Key]
    public int CustomerID { get; set; }
    public bool NameStyle { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Suffix { get; set; }
    public string? CompanyName { get; set; }
    public string? SalesPerson { get; set; }
    public string? EmailAddress { get; set; }
    public string? Phone { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public Guid Rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
  }

}
