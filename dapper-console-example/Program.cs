using DapperConsoleExample;

const string connectionString = "Data Source=(localdb)\\ProjectModels;Database=AdventureWorksLT2019;Integrated Security = True;Trusted_Connection=True;";

using (var db = new CustomerRepository(connectionString))

foreach (var customer in await db.SelectCustomer())
{
  Console.WriteLine(customer.FirstName + " - " + customer.EmailAddress);
}