using DapperConsoleExample;

const string connectionString = "Data Source=DapperConsoleExample.db";

await using (var db = new CustomerRepository(connectionString))
{
  await db.Init();

  var customers = await db.SelectCustomer();

  var exampleCustomer = customers.First();
  Console.Write(exampleCustomer.FirstName + " - " + exampleCustomer.EmailAddress);

  var originalName = exampleCustomer.FirstName;

  exampleCustomer.FirstName = originalName + "_edited";
  await db.UpdateCustomer(exampleCustomer);

  var newCustomer = await db.SelectCustomerById(exampleCustomer.CustomerID);
  Console.Write(" Altered to: ");
  Console.WriteLine(newCustomer.FirstName + " - " + newCustomer.EmailAddress);

  Console.WriteLine("Undoing update...");
  exampleCustomer.FirstName = originalName;
  await db.UpdateCustomer(exampleCustomer);
  var customerRevertedChanges = await db.SelectCustomerById(exampleCustomer.CustomerID);
  Console.Write("Reverted to: ");
  Console.WriteLine(customerRevertedChanges.FirstName + " - " + customerRevertedChanges.EmailAddress);

  Console.WriteLine("Deleting customer " + customerRevertedChanges.FirstName);
  await db.DeleteCustomer(customerRevertedChanges);

  Console.WriteLine("Undoing delete...");
  await db.InsertCustomer(customerRevertedChanges);
  Console.WriteLine("Success");
}