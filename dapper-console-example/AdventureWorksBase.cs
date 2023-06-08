using Dapper.Contrib.Extensions;

namespace DapperConsoleExample
{
  public abstract class AdventureWorksBase
  {
    public DateTime ModifiedDate { get; set; }
    public Guid Rowguid { get; set; }
  }
}