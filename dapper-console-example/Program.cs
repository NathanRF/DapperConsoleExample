
using Dapper;
using DapperConsoleExample;
using System.Data;
using System.Data.SqlClient;

const string connectionString = "Server=localhost;Database=AdventureWorksLT2019;Trusted_Connection=True;";

await using (SqlConnection connection = new(connectionString))
{
  connection.Open();
  var customers = connection.Query<Customer>("select * from SalesLT.Customer (nolock)");
  foreach (var customer in customers)
  {
    Console.WriteLine(customer.FirstName + " - " + customer.EmailAddress);
  }
}
